#![feature(proc_macro_diagnostic)]

use proc_macro::TokenStream;
use quote::quote;

/// Simple procedural that handles command-parsing boilerplate.
/// This macro can be applied to functions whose arguments are all types supported by discord.
/// It transforms a function to contain parsing logic
#[proc_macro_attribute]
pub fn command(_attr: TokenStream, input: TokenStream) -> TokenStream {
    let input = syn::parse_macro_input!(input as syn::ItemFn);
    let mut types = input.sig.inputs.iter();

    // // Take the return type and assert that it's a Result
    // if input.sig.output.type_id() != TypeId::of::<anyhow::Result<()>>() {
    //     quote_spanned! {input.sig.output.span()=> compile_error!("expected first type to be `anyhow::Result<()>`")};
    //     // Diagnostic::new(Level::Error, "mismatched types")
    //     //     .span_error(input.sig.span(), "expected `anyhow::Result<()>`")
    //     //     .emit();
    // }
    // // Take the first argument and assert that it's a CommandContext
    let _ = types.next();
    // if !matches!(first, Some(arg) if arg.type_id() == TypeId::of::<CommandContext>()) {
    //     quote_spanned! {input.sig.span()=> compile_error!("expected first type to be `CommandContext`")};
    // }

    // Repeatedly call `ParseArg::parse_arg` as many times as we have arguments.
    // Later, this will be expanded in argument-position in our macro.
    let types: Vec<_> = types
        .into_iter()
        .map(|_| {
            // Quote:
            quote! {
                ::command_derive::ParseArg::parse_arg(&inter)?
            }
        })
        .collect();

    let vis = &input.vis;
    let ident = &input.sig.ident;
    let res = quote! {
        #vis fn #ident (inter: Interaction) -> ::anyhow::Result<()> {
            // let it_data = inter.data;
            // let it_data = it_data.ok_or(::anyhow::anyhow!("top-level command returned no data"))?;
            // let mut it_data = it_data.options.iter();

            let context = ::framework::CommandContext{};
            
            // Define and call inner function with same name as outer (i.e. #ident)
            #input
            return #ident (context , #(#types),*);
        }
    };
    res.into()
}

