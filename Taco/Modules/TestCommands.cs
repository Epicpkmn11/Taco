﻿using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using RevoltApi;
using RevoltApi.Channels;
using RevoltBot.Attributes;
using RevoltBot.CommandHandling;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Console = Log73.Console;

namespace RevoltBot.Modules
{
    public class TestModule : ModuleBase
    {
        [Command("test", "test-alias")]
        [Summary("Test command.")]
        public async Task TestCommand()
        {
            await ReplyAsync("test");
        }

        [Command("whois")]
        [Summary("Retrieve information about a user.")]
        public async Task WhoIs()
        {
            var user = GetMention(Args);
            if (user == null)
            {
                await ReplyAsync(":x: Specify a user by mentioning them, their name or id.");
                return;
            }

            await ReplyAsync($@"> ## {user.Username}
> Mention: <@{user._id}>
> Id: `{user._id}`
> Online: {user.Online}
> [\[Default Avatar\]]({user.DefaultAvatarUrl}) [\[Avatar\]]({user.AvatarUrl})");
        }

        [Command("revolt")]
        [Summary("Information about revolt instance.")]
        public async Task RevoltInfo()
        {
            var info = await Message.Client.GetApiInfo();
            var voso = await Message.Client.GetVosoInfo();
            await ReplyAsync(@$"> ## Revolt info (for app.revolt.chat)
> ### Versions
> **Api:** {info.Version}
> **Voso:** {voso.Version}
> ### Features
> **Email:** {StringBooled(info.Features.Email)}
> **Invite-only:** {StringBooled(info.Features.InviteOnly)}
> **Captcha:** {StringBooled(info.Features.Captcha.Enabled)}
> **Voso.RTP:** {StringBooled(voso.Features.Rtp)}");
        }

        public string StringBooled(bool value)
            => StringBooled(value, value.ToString());

        public string StringBooled(bool value, string str)
        {
            return $@"$\color{{{(value ? "lime" : "red")}}}\text{{{str}}}$";
        }

        // [Command("discord")]
        // public async Task Discord()
        // {
        //     await Message.Channel.SendFileAsync("bruh", "discord.jpg", @"C:\Users\Jan\Downloads\image0-53 (1).jpg");
        // }

        [Command("fuck")]
        [Summary("Fuck someone.")]
        public async Task Fuck()
        {
            var mention = GetMention(Args);
            if (mention == null)
            {
                await ReplyAsync("mention someone h");
                return;
            }

            var web = new WebClient();
            var authorPfp =
                await Image.LoadAsync(new MemoryStream(await web.DownloadDataTaskAsync(Message.Author.AvatarUrl)));
            var mentionPfp =
                await Image.LoadAsync(new MemoryStream(await web.DownloadDataTaskAsync(mention.AvatarUrl)));
            authorPfp.Mutate(c => c.Resize(new Size(166, 166)));
            mentionPfp.Mutate(c => c.Resize(new Size(110, 110)));
            var image = await Image.LoadAsync(@"./Resources/Fuck.png");
            image.Mutate(c =>
            {
                c.DrawImage(authorPfp, new Point(117, 65), 1.0f);
                c.DrawImage(mentionPfp, new Point(140, 330), 1.0f);
            });
            await Message.Channel.SendPngAsync(image, "get fucked nerd");
        }

        // [Command("jan")]
        // public async Task Jan()
        // {
        //     var web = new WebClient();
        //     await Message.Channel.SendFileAsync("jan", "jan.png",
        //         await web.DownloadDataTaskAsync(
        //             "https://cdn.discordapp.com/attachments/803693023661522966/816394428176138250/motivate.png"));
        // }

        public User GetMention(string mention)
        {
            return Message.Client.UsersCache.FirstOrDefault(u => u._id == mention) ??
                   Message.Client.UsersCache.FirstOrDefault(u => u.Username.ToLower() == mention.ToLower()) ??
                   Message.Client.UsersCache.FirstOrDefault(u => u._id == mention.Replace("<@", "").Replace(">", "")) ??
                   Message.Client.Users.Get(mention);
        }

        [Command("retard")]
        public async Task Retard()
        {
            await Message.Channel.BeginTypingAsync();
            await Task.Delay(7000);
            await ReplyAsync("<@01EXAG0ZFX02W7PNQE7W5MT339> retard");
            await Message.Channel.EndTypingAsync();
        }

        [Command("druh")]
        [Summary("rape webcocket")]
        [RequireBotOwner]
        public async Task Druh()
        {
            var delay = 30;
            while (true)
            {
                await Message.Channel.BeginTypingAsync();
                await Task.Delay(delay);
                await Message.Channel.EndTypingAsync();
                await Task.Delay(delay);
            } 
        }
        //
        // [Command("unfriend")]
        // public async Task UnfriendMePls()
        // {
        //     if (Message.AuthorId != "01EX40TVKYNV114H8Q8VWEGBWQ")
        //     {
        //         await ReplyAsync("cock");
        //         return;
        //     }
        //
        //     Console.WriteLine("h1");
        //     await Message.Client.Users.RemoveFriendAsync(Message.AuthorId);
        //
        //     Console.WriteLine("h2");
        //     await ReplyAsync("get kek'd");
        // }

        [Command("leave")]
        [Summary("Leaves the group.")]
        [GroupOnly]
        [RequireGroupOwner]
        public async Task LeaveChannel()
        {
            await Message.Client.Channels.LeaveAsync(Message.ChannelId);
        }

        // [Command("hspace")]
        // public async Task HSpace()
        // {
        //     var text = "";
        //     for (int i = 0; i < 30; i++)
        //     {
        //         text += $"$\\hspace{{{i}cm}}$ >\n";
        //     }
        //
        //     await ReplyAsync(text);
        // }

        [Command("iplookup", "hack", "ip", "nslookup")]
        [Summary("Retrieve information about an IP or domain.")]
        public async Task Hack()
        {
            // todo: filter out http://(.+)/ ??? and hrafegtjyku
            var obj = JObject.Parse(
                (await new RestClient().ExecuteGetAsync(new RestRequest("http://ip-api.com/json/" + Args))).Content);
            // query, country, countryCode, regionName, city, zip, timezone, isp, org
            dynamic dyn = obj;
            // check for death response
            if (dyn.query == Args && dyn.country == null)
            {
                await ReplyAsync(":x: I can't find that IP or domain.");
                return;
            }
            await ReplyAsync(@$"> ## IP Lookup: {Args}
> **IP:** {dyn.query}
> **Country:** {dyn.country} [{dyn.countryCode}]
> **Region:** {dyn.regionName}
> **City:** {dyn.city}
> **Zip:** {dyn.zip}
> **Time zone:** {dyn.timezone}
> **ISP:** {dyn.isp}
> **Organization:** {dyn.org}");
        }

        [Command("ping")]
        [Summary("Ping!")]
        public Task Ping()
        {
            var web = new WebClient();
            var stopwatch = Stopwatch.StartNew();
            web.DownloadString(Message.Client.ApiUrl);
            var restPing = stopwatch.ElapsedMilliseconds;
            return ReplyAsync(@$"REST API Ping: {restPing}ms
Websocket Ping: doesnt exist");
        }

        [Command("edittest")]
        [RequireDeveloper]
        public async Task EditTest()
        {
            var msg = await ReplyAsync("hell");
            for (int i = 0; i < 300; i++)
            { 
                await msg.EditAsync(i.ToString());
                //await Task.Delay(100);
            }

            await msg.EditAsync("ok cool");
        }

        [Command("group", "groupinfo")]
        public Task GroupInfo()
        {
            var group = (GroupChannel) Message.Channel;
            return ReplyAsync($@"> ## {group.Name}
> {group.Description}
> **Owner:** <@{group.OwnerId}> [`{group.OwnerId}`]
> **ID:** `{group._id}`
> {group.RecipientIds.Length} Recipients");
        }

        [Command("flush", "flushed")]
        public Task Flushed()
            => ReplyAsync("# $\\huge\\text{😳}$");
    }
}