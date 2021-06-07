mod commands;
mod config;
mod consts;
mod events;

use crate::events::Handler;
use config::Config;
use serenity::{framework::StandardFramework, prelude::*};
use std::{fs::File, io::Read};

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    let mut config = File::open("config.toml")?;
    let mut buf = String::new();
    config.read_to_string(&mut buf)?;
    let config: Config = toml::from_str(&buf)?;

    let framework = StandardFramework::new();

    let mut client = Client::builder(&config.tokens.discord)
        .event_handler(Handler)
        // TODO: Move into config
        .application_id(724806362898366481u64)
        .framework(framework)
        .await?;
    client.start().await.map_err(Into::into)
}
