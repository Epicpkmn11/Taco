﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revolt;
using Revolt.Commands.Attributes;
using Revolt.Commands.Attributes.Preconditions;
using Taco.Attributes;
using Taco.CommandHandling;

namespace Taco.Modules
{
    [Name("Developer")]
    [Group("dev")]
    [Summary("Developer-only commands.")]
    [RequireDeveloper]
    [Hidden]
    public class DevCommands : TacoModuleBase
    {
        [RequireDeveloper]
        [Command("perm")]
        public async Task SetPermissions(string userId, string levelStr)
        {
            if (userId == Program.BotOwnerId)
            {
                await ReplyAsync("Sussus amogus.");
                return;
            }

            sbyte level;
            if (!sbyte.TryParse(levelStr, out level))
            {
                Enum.TryParse(levelStr, ignoreCase: true, out PermissionLevel lvl);
                level = (sbyte)lvl;
            }

            if ((sbyte)Context.UserData.PermissionLevel <= level && Context.User._id != Program.BotOwnerId)
            {
                await ReplyAsync("Can't set higher or same permission level.");
                return;
            }

            var userData = Mongo.GetOrCreateUserData(userId);
            userData.PermissionLevel = (PermissionLevel)level;
            await userData.UpdateAsync();
            await ReplyAsync($"<@{userId}> [`{userId}`] permission level changed to `{level}`");
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

        [Command("druh")]
        [RequireBotOwner]
        public async Task Druh(int delay = 30)
        {
            while (true)
            {
                await Message.Channel.BeginTypingAsync();
                await Task.Delay(delay);
                await Message.Channel.EndTypingAsync();
                await Task.Delay(delay);
            }
        }

        [Command("updatestatus")]
        [Summary("Force update status.")]
        public async Task UpdateStatus()
        {
            await Program.Client.Self.EditProfileAsync(new UserInfo()
            {
                Status = new()
                {
                    Text = Program.Config.Status,
                    Presence = Program.Config.Presence
                },
                Profile = new()
                {
                    Content = Program.Config.Profile
                }
            });
            await ReplyAsync("Updated status and soige,o.");
        }

        [Command("setstatus")]
        public async Task SetStatus([Remainder] string arg)
        {
            Program.Config.Status = _nullableArgs(arg);
            await Program.SaveConfig();
            await ReplyAsync("UPdated le staustm,.");
        }

        [Command("setpresence")]
        public async Task SetPresence([Remainder] string arg)
        {
            Program.Config.Presence = arg;
            await Program.SaveConfig();
            await ReplyAsync("updated le presenc");
        }

        [Command("setprofile")]
        public async Task SetProfile([Remainder] string arg)
        {
            Program.Config.Profile = _nullableArgs(arg);
            await Program.SaveConfig();
            await ReplyAsync("profiel set!!!");
        }

        // [Command("rateLimited")]
        // [Summary("List people that got rate limited.")]
        // public Task ListRateLimited()
        // {
        //     var str = new StringBuilder();
        //     foreach (var retard in Program.RateLimited)
        //     {
        //         str.AppendLine($"> <@{retard.Key}>");
        //     }
        //
        //     return ReplyAsync(str.ToString());
        // }
        //
        // [Command("unRateLimit")]
        // [Summary("Remove someone from the rate limit list.")]
        // public Task RemoveRateLimited()
        // {
        //     Program.RateLimited.Remove(Args);
        //     return ReplyAsync($"<@{Args}> has been removed from the rate limit list.");
        // }

        [Command("coc set")]
        public Task CocSet(int index, [Remainder] string str)
        {
            Program.Config.CodeOfConduct[index] = str;
            Program.SaveConfig();
            return ReplyAsync("set;");
        }

        [Command("coc add")]
        public Task CocAdd([Remainder] string str)
        {
            Program.Config.CodeOfConduct.Add(str);
            Program.SaveConfig();
            return ReplyAsync("add;");
        }

        [Command("coc rm")]
        public Task CocRemove(int index)
        {
            Program.Config.CodeOfConduct.RemoveAt(index);
            Program.SaveConfig();
            return ReplyAsync("rm;");
        }

        private string _nullableArgs(string str) => str == "null" ? null : str;

        [Command("ownertest")]
        [RequireBotOwner]
        public Task OwnerTest() => ReplyAsync("sus");
    }
}