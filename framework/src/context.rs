use serenity::{client::Context};

use crate::Command;

/// Contains information about the caller and invocation of a command
pub struct CommandContext {
    pub client_context: Context,
    pub command: Command,
}