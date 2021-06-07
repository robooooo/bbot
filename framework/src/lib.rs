mod command;
mod args;
mod context;

pub use command::Command;
pub use args::Arg;
pub use context::CommandContext;

use serenity::model::interactions::ApplicationCommandOptionType;
pub type Kind = ApplicationCommandOptionType;
