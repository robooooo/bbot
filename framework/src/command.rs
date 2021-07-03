use crate::Arg;
use serenity::model::interactions::Interaction;

// Register a central collection of commands, via the inventory crate
inventory::collect!(Command);

#[derive(Clone)]
pub struct Command {
    pub name: &'static str,
    pub desc: &'static str,
    pub args: Vec<Arg>,
    pub payload: Box<fn(&mut Interaction)>,
}
