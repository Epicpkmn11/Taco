﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jan0660.AzurAPINet.Ships;
using Revolt;
using Revolt.Channels;
using Revolt.Commands.Info;
using SixLabors.ImageSharp;
using Taco.Attributes;
using Taco.CommandHandling;

namespace Taco
{
    public static class ExtensionMethods
    {
        public static async Task<Message> SendPngAsync(this Channel channel, Image image, string content,
            string filename = "h.png")
        {
            var memory = new MemoryStream();
            await image.SaveAsPngAsync(memory);
            return await channel.SendFileAsync(content, filename, memory.GetBuffer());
        }

        /*
 @$"**Health:** {stats.Health}
**Armor:** {stats.Armor}
**Reload:** {stats.Reload}
**Luck:** {stats.Luck}
**Firepower:** {stats.Firepower}
**Torpedo:** {stats.Torpedo}
**Evasion:** {stats.Evasion}
**Speed:** {stats.Speed}
**AntiAir:** {stats.AntiAir}
**Aviation:** {stats.Aviation}
**Oil consumption:** {stats.OilConsumption}
**Accuracy:** {stats.Accuracy}
**AntiSubmarineWarfare:** {stats.AntiSubmarineWarfare}",
 */
        public static Dictionary<string, string> ToDict(this ShipStats stats)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Oil consumption", stats.OilConsumption.ToString());
            dict.Add("Health", stats.Health.ToString());
            dict.Add("Armor", stats.Armor);
            dict.Add("Reload", stats.Reload.ToString());
            dict.Add("Luck", stats.Luck.ToString());
            dict.Add("Firepower", stats.Firepower.ToString());
            dict.Add("Torpedo", stats.Torpedo.ToString());
            dict.Add("Evasion", stats.Evasion.ToString());
            dict.Add("Speed", stats.Speed.ToString());
            // AA
            dict.Add("AntiAir", stats.AntiAir.ToString());
            dict.Add("Aviation", stats.Aviation.ToString());
            dict.Add("Accuracy", stats.Accuracy.ToString());
            // ASW
            dict.Add("AntiSub", stats.AntiSubmarineWarfare.ToString());
            return dict;
        }

        public static bool IsHidden(this ModuleInfo module)
        {
            return module.Attributes.Any(att => att is HiddenAttribute);
        }

        public static void LimitedAdd<T>(this List<T> list, T item, int maxCount)
        {
            list.Add(item);
            if (list.Count == maxCount)
                list.Remove(list.FirstOrDefault());
        }

        [Pure]
        public static string Shorten(this string str, int length)
            => str.Length > length ? str[..(length - 5)] + "(...)" : str;

        public static T GetAttribute<T>(this ModuleInfo module) where T : Attribute
        {
            foreach (var att in module.Attributes)
            {
                if (att is T)
                {
                    return (T)att;
                }
            }

            return null;
        }
    }
}