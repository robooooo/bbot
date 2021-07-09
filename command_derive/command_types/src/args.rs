use anyhow::anyhow;
use serenity::model::prelude::*;

/// Required parsing logic for command proc-macro.
pub trait ParseArg: Sized {
    fn parse_arg(val: ApplicationCommandInteractionDataOptionValue) -> anyhow::Result<Self>;
}

impl ParseArg for i64 {
    fn parse_arg(val: ApplicationCommandInteractionDataOptionValue) -> anyhow::Result<Self> {
        match val {
            ApplicationCommandInteractionDataOptionValue::Integer(i) => Ok(i),
            _ => Err(anyhow!("Interaction data had incorrect type")),
        }
    }
}

impl ParseArg for PartialChannel {
    fn parse_arg(val: ApplicationCommandInteractionDataOptionValue) -> anyhow::Result<Self> {
        match val {
            ApplicationCommandInteractionDataOptionValue::Channel(chan) => Ok(chan),
            _ => Err(anyhow!("Interaction data had incorrect type")),
        }
    }
}

// let inner: fn(i32) = |a| {};
// // let v = inter.data.unwrap().options[0].resolved.unwrap();
// let v = match v {
//     // ApplicationCommandInteractionDataOptionValue::Integer(i) => i,
//     _ => panic!(),
// };

// // inner(v as i32);
