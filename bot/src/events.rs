use framework::Command;
use serenity::{
    async_trait,
    model::{interactions::Interaction, prelude::*},
    prelude::*,
};

pub struct Handler;

#[async_trait]
impl EventHandler for Handler {
    async fn interaction_create(&self, _: Context, interaction: Interaction) {
        let name = match interaction.data {
            Some(ref val) => &val.name,
            None => return,
        };

        // This might be a performance concern in the future? Should be fine for now.
        for command in inventory::iter::<Command> {
            if command.name == name {
                // TODO: E.H.
                (command.payload)(interaction).unwrap();
                return;
            }
        }
        // TODO: This should probably be moved to some utilities on CommandContext.
        // let _ = interaction
        //     .create_interaction_response(&ctx.http, |resp| {
        //         resp.kind(InteractionResponseType::ChannelMessageWithSource)
        //             .interaction_response_data(|m| m.content("Ping!"))
        //     })
        //     .await;
    }

    async fn ready(&self, ctx: Context, _: Ready) {
        println!("Connected!");

        let testing = GuildId(517848417049772033u64);
        // Discord requires us to register our slash commands on startup.
        // Let's do that here.
        for command in inventory::iter::<Command> {
            testing
                .create_application_command(&ctx.http, |f| {
                    f.name(command.name).description(command.desc);
                    for arg in &command.args {
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
