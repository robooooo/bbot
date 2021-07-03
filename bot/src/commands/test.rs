use framework::{Command, Arg, Kind};
use serenity::model::interactions::Interaction;

inventory::submit! {
    Command {
        payload: Box::new(|interaction: &mut Interaction| {
            println!("{:#?}", interaction);
        }),
        name: "pingc2",
        desc: "Sends a ping! To a channel!",
        args: vec![
            Arg {
                name: "channel",
                desc: "Put it here!",
                required: true,
                kind: Kind::Channel,
            },
            Arg {
                name: "integer",
                desc: "Ping with this!",
                required: false,
                kind: Kind::Integer,
            },
        ],
    }
}