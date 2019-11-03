## BBot For Discord

## BBot

BBot is a a general-purpose discord bot with the killer feature of backing up pins to a dedicated channel in order to effectively bypass the 50-pin limit imposed by discord. It's written in C# with DSharpPlus and can be accessed using the `$` prefix or by mention.

### Commands

+ **Backup** backs up all the pins in the current channel to another channel.
+ **Changelog** posts the version information for BBot.
+ **Help** lists all commands or display help for a certain command.
+ **Random** Generates a random number between two bounds.
+ **Roll** simulates a roll for repeated digits or 'dubs'.
+ **SCP** links to an SCP or tale from the SCP wiki.
+ **Search** searches for a query on google.

### The Backup Command

The backup command is the main feature of this bot and the reason it was written. It can take up to 50 pins from a channel and post them using a stylish embed in the channel of your choice, which makes it perfect for server administration. It supports text, images, videos and users using plugins such as citador. It provides direct links to posts so you can go back in case anything goes wrong, or if you're just curious about what went down when.

![Before Backup](https://i.imgur.com/JjKjsTJ.png)

![After Backup](https://i.imgur.com/H1lJ4sB.png)

### Try it out!

[Add BBot to your server right now!](https://discordapp.com/oauth2/authorize?client_id=362666654452416524&scope=bot&permissions=92224)

Or check it out online (coming soon)