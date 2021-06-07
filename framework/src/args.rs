use crate::Kind;

#[derive(Clone, Debug)]
pub struct Arg {
    pub name: &'static str,
    pub desc: &'static str,
    pub required: bool,
    pub kind: Kind,
}
