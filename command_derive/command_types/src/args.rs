use serenity::model::interactions::Interaction;

/// Required parsing logic for command proc-macro.
pub trait ParseArg {
    fn parse_arg(inter: &Interaction) -> Self;
}

impl ParseArg for i32 {
    fn parse_arg(_: &Interaction) -> i32 {
        0
    }
}