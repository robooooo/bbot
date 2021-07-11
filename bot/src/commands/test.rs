use command_derive::command;
use framework::{Arg, Command, CommandContext, Kind};
use serenity::model::{channel::PartialChannel};

#[command]
fn command(ctx: CommandContext, data: i64, location: PartialChannel) -> anyhow::Result<()> {
    println!("{:?}", data);
    println!("{:?}", location);
    Ok(())
}

inventory::submit! {
    Command {
        payload: command,
        name: "pingc3",
        desc: "Sends a ping! To a channel!",
        args: vec![
            Arg {
                name: "int",
                desc: "Ping with this!",
                required: true,
                kind: Kind::Integer,
            },
            Arg {
                name: "chan",
                desc: "Ping to here!",
                required: true,
                kind: Kind::Channel,
            },
        ],
    }
}
