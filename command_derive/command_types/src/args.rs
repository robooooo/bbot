use anyhow::anyhow;
use serenity::model::prelude::*;

/// Required parsing logic for command proc-macro.
pub trait ParseArg: Sized {
    fn parse_arg(val: ApplicationCommandInteractionDataOptionValue) -> anyhow::Result<Self>;
}

/// Macro for blanket impl of parse_arg over (almost) all possible types
/// Note that that the `Member` variant is a special case, due to DM/guild conflicts.
macro_rules! parse_arg_impl {
    ($typ:ty, $variant:path) => {
        impl ParseArg for $typ {
            fn parse_arg(
                val: ApplicationCommandInteractionDataOptionValue,
            ) -> anyhow::Result<Self> {
                match val {
                    $variant(inner) => Ok(inner),
                    // TODO: E.H. Include expected and actual
                    _ => Err(anyhow!("Interaction data had incorrect type")),
                }
            }
        }
    };
}

type Value = ApplicationCommandInteractionDataOptionValue;
parse_arg_impl!(String, Value::String);
parse_arg_impl!(i64, Value::Integer);
parse_arg_impl!(bool, Value::Boolean);
parse_arg_impl!(PartialChannel, Value::Channel);
parse_arg_impl!(Role, Value::Role);

/// Note that this is a special case not covered by the impl macro.
/// This is just because the enum variant contains two values.
/// One an always present `User` struct, then an optional `PartialMember`.
/// The `PartialMember` is not present in DMs, but we disable DM commands.
impl ParseArg for PartialMember {
    fn parse_arg(val: ApplicationCommandInteractionDataOptionValue) -> anyhow::Result<Self> {
        match val {
            Value::User(_, Some(inner)) => Ok(inner),
            Value::User(_, None) => unreachable!("Expected commands to be disabled in DMs"),
            // TODO: E.H. Include expected and actual
            _ => Err(anyhow!("Interaction data had incorrect type")),
        }
    }
}
