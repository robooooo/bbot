use std::any::{Any, TypeId};

use proc_macro::TokenStream;
use proc_macro2::TokenStream as TokenStream2;
use quote::quote;
use syn::FnArg;


/// Simple procedural that handles command-parsing boilerplate.
/// This macro can be applied to functions whose arguments are all types supported by discord.
/// It transforms the function into a
#[proc_macro_attribute]
pub fn command(_attr: TokenStream, input: TokenStream) -> TokenStream {
    let input = syn::parse_macro_input!(input as syn::ItemFn);
    let mut types = input.sig.inputs.iter();

    // Take the first argument and assert that it's a CommandContext
    let first = types.next();
    // if !matches(first, Some(arg) if arg.type_id == TypeId::of::<i32>()) {
    //     panic!()
    // }

    // Map each type to code that parses it from a
    let types: Result<Vec<_>, _> = input.sig.inputs.iter().map(transform).collect();
    let types = types.unwrap();

    let res = quote! {
        #(#types);*
        #input
    };
    res.into()
}

fn transform(arg: &FnArg) -> Result<TokenStream2, ()> {
    let id = arg.type_id();
    if id == TypeId::of::<&str>() {
        Ok(quote! {})
    } else if id == TypeId::of::<i32>() {
        Ok(quote! {})
    } else {
        Err(())
    }
}
