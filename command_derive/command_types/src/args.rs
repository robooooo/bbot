use std::panic;

use anyhow::anyhow;
use serenity::model::{interactions::Interaction, prelude::*};

/// Required parsing logic for command proc-macro.
pub trait ParseArg: Sized {
    fn parse_arg(inter: &Interaction) -> anyhow::Result<Self>;
}

impl ParseArg for i32 {
    fn parse_arg(inter: &Interaction) -> anyhow::Result<Self> {
        // let val = inter.data.as_ref().ok_or(anyhow!("Interaction cannot hold data"))?;
        // match val {
        //     ApplicationCommandInteractionDataOptionValue::Integer(val) => val,
        //     _ => anyhow!("Interaction data had incorrect type")
        // }
    }
}

impl ParseArg for Channel {
    fn parse_arg(_: &Interaction) -> anyhow::Result<Self> {
        panic!()
    }
}

// let inner: fn(i32) = |a| {};
// // let v = inter.data.unwrap().options[0].resolved.unwrap();
// let v = match v {
//     // ApplicationCommandInteractionDataOptionValue::Integer(i) => i,
//     _ => panic!(),
// };

// // inner(v as i32);
