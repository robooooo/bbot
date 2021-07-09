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

    // Ignore the first type; which we will assume is a CommandContext
    // TODO: In future, we may want to assert this.
    let _ = types.next();

    // Repeatedly call `ParseArg::parse_arg` as many times as we have arguments.
    // Later, this will be expanded in argument-position in our macro.
    let types: Vec<_> = types
        .into_iter()
        .map(|_| {
            // Note: parse_arg has overloaded return type, so we don't care about the argument type
            quote! {{
                let next = values.next().ok_or(::anyhow::anyhow!("Not enough arguments provided to interaction"))?;
                let next = next.resolved.ok_or(::anyhow::anyhow!("Object could not be resolved, perhaps it was malformed?"))?;

                ::command_derive::ParseArg::parse_arg(next)?
            }}
        })
        .collect();

    // use anyhow::anyhow;
    // let inter: Interaction = panic!();
    // let values = inter
    //     .data
    //     .ok_or(anyhow!("Interaction cannot hold data"))
    //     .unwrap();
    // let b = values.options.into_iter().next().unwrap();

    let vis = &input.vis;
    let ident = &input.sig.ident;
    let res = quote! {
        #vis fn #ident (inter: Interaction) -> ::anyhow::Result<()> {
            // let it_data = inter.data;
            // let it_data = it_data.ok_or(::anyhow::anyhow!("top-level command returned no data"))?;
            // let mut it_data = it_data.options.iter();

            let context = ::framework::CommandContext{};

            // Find iterator of resolved interaction argument values
            let values = inter.data.ok_or(::anyhow::anyhow!("Internal interaction cannot hold data"))?;
            let mut values = values.options.into_iter();

            // Define and call inner function with same name as outer (i.e. #ident)
            #input
            return #ident (context , #(#types),*);
        }
    };
    res.into()
}
