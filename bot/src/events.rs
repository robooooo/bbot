use framework::{Arg, Command, Kind};
use serenity::{
    async_trait,
    model::{
        interactions::{Interaction, InteractionResponseType},
        prelude::*,
    },
    prelude::*,
};
use std::future::Future;

pub struct Handler;

#[async_trait]
impl EventHandler for Handler {
    async fn interaction_create(&self, ctx: Context, interaction: Interaction) {
        dbg!(&interaction);
        let _ = interaction
            .create_interaction_response(&ctx.http, |resp| {
                resp.kind(InteractionResponseType::ChannelMessageWithSource)
                    .interaction_response_data(|m| m.content("Ping!"))
            })
            .await;
    }

    async fn ready(&self, ctx: Context, _: Ready) {
        println!("Connected!");

        // let commands: Vec<Command> = Vec::new();
        let commands: Vec<Command> = vec![Command {
            payload: Box::new(|interaction: &mut Interaction| {
                println!("{:#?}", interaction);
            }),
            name: "pingc",
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
        }];

        let testing = GuildId(517848417049772033u64);
        // Discord requires us to register our slash commands on startup.
        // Let's do that here.
        for command in commands {
            testing
                .create_application_command(&ctx.http, |f| {
                    f.name(command.name).description(command.desc);
                    for arg in command.args {
                        f.create_option(|f| {
                            f.name(arg.name)
                                .description(arg.desc)
                                .kind(arg.kind)
                                .required(arg.required)
                        });
                    }
                    f
                })
                .await
                .unwrap();
        }
    }
}
