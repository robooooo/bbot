#![feature(proc_macro_diagnostic)]

use framework::CommandContext;
use proc_macro::{TokenStream};
use proc_macro2::{TokenStream as TokenStream2, Span as Span2};
use quote::{quote, quote_spanned};
use serenity::model::interactions::Interaction;
use std::any::{Any, TypeId};
use syn::{spanned::Spanned, FnArg};

/// Simple procedural that handles command-parsing boilerplate.
/// This macro can be applied to functions whose arguments are all types supported by discord.
/// It transforms a function to contain parsing logic
#[proc_macro_attribute]
pub fn command(_attr: TokenStream, input: TokenStream) -> TokenStream {
    let input = syn::parse_macro_input!(input as syn::ItemFn);
    let mut types = input.sig.inputs.iter();

    // Take the return type and assert that it's a Result
    if input.sig.output.type_id() != TypeId::of::<anyhow::Result<()>>() {
        quote_spanned! {input.sig.output.span()=> compile_error!("Expected first type to be `anyhow::Result<()>`")};
        // Diagnostic::new(Level::Error, "mismatched types")
        //     .span_error(input.sig.span(), "expected `anyhow::Result<()>`")
        //     .emit();
    }
    // Take the first argument and assert that it's a CommandContext
    let first = types.next();
    if !matches!(first, Some(arg) if arg.type_id() == TypeId::of::<CommandContext>()) {
        quote_spanned! {input.sig.span()=> compile_error!("Expected first type to be `CommandContext`")};
    }

    // Map each type to code that parses it from the interactionn
    let types: Result<Vec<_>, _> = types.map(transform).collect();
    let types = match types {
        Ok(ty) => ty,
        Err(span) => {
            return syn::Error::new(span, "unsupported type").to_compile_error().into();
        }
    };

    // let inter: Interaction;
    // let it_data = inter.data;
    // let it_data = it_data.ok_or("freakin error handling".into())?;
    // let it_data = it_data.options.iter();

    let vis = &input.vis;
    let ident = &input.sig.ident;
    let res = quote! {
        use anyhow::anyhow;

        #vis fn #ident (inter: Interaction) -> ::anyhow::Result<()> {
            let it_data = inter.data;
            let it_data = it_data.ok_or(anyhow!("top-level command returned no data"))?;
            let mut it_data = it_data.options.iter();

            let context = framework::CommandContext{};

            #input
            #ident (context , #(#types);*)
        }
    };
    res.into()
}

// // let v = inter.data.unwrap().options[0].resolved.unwrap();
// let v = match v {
//     // ApplicationCommandInteractionDataOptionValue::Integer(i) => i,
//     _ => panic!(),
// };

/// Transform each type into some code that can parse into the given type (with error handling)
fn transform(arg: &FnArg) -> Result<TokenStream2, Span2> {
    let id = arg.type_id();
    let variant = if id == TypeId::of::<i32>() {
        quote! {}
    } else {
        return Err(arg.span());
    };
    let res = quote! {{
        match it_data.next() {
            Some(Some(#variant (value))) => value,
            Some(Some(_)) => return Err("wrong type eh"),
            _ => return Err("not enough data eh")
        }
    }};
    Ok(res)
}
