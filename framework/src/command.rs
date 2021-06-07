use crate::Arg;
use serenity::model::interactions::Interaction;

#[derive(Clone)]
pub struct Command {
    pub name: &'static str,
    pub desc: &'static str,
    pub args: Vec<Arg>,
    pub payload: Box<fn(&mut Interaction)>,
}
