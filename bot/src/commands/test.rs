use framework::{Arg, Command, CommandContext, Kind};
use serenity::model::interactions::Interaction;
use command_derive::command;

#[command]
fn command(ctx: CommandContext, data: i64) -> anyhow::Result<()> {
    println!("{}", data);
    Ok(())
}

inventory::submit! {
    Command {
        payload: command,
        name: "pingc2",
        desc: "Sends a ping! To a channel!",
        args: vec![
            Arg {
                name: "integer",
                desc: "Ping with this!",
                required: false,
                kind: Kind::Integer,
            },
        ],
    }
}
