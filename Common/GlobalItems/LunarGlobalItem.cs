using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Buffs.Lunar;

namespace ThreatOfPrecipitation.Common.GlobalItems
{
    public class LunarGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<LunarBuffPlayer>();

            if (modPlayer.shapedGlass)
            {
                TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "Damage");
                if (line != null)
                {
                    string reg = @"\d+";
                    var match = Regex.Match(line.Text, reg);
                    if (int.TryParse(match.Value, out int damage))
                    {
                        damage *= 2;
                        string newText = Regex.Replace(line.Text, reg, damage.ToString());
                        line.Text = newText;
                    }
                }
            }
        }
    }
}