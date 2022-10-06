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

            if (modPlayer.lightFluxPauldron)
            {
                TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "Speed");
                if (line != null)
                {
					// Code from vanilla to make attack speed display properly
					if (item.useAnimation * 2 <= 8)
					{
						line.Text = Lang.tip[6].Value;
					}
					else if (item.useAnimation * 2 <= 20)
					{
						line.Text = Lang.tip[7].Value;
					}
					else if (item.useAnimation * 2 <= 25)
					{
						line.Text = Lang.tip[8].Value;
					}
					else if (item.useAnimation * 2 <= 30)
					{
						line.Text = Lang.tip[9].Value;
					}
					else if (item.useAnimation * 2 <= 35)
					{
						line.Text = Lang.tip[10].Value;
					}
					else if (item.useAnimation * 2 <= 45)
					{
						line.Text = Lang.tip[11].Value;
					}
					else if (item.useAnimation * 2 <= 55)
					{
						line.Text = Lang.tip[12].Value;
					}
					else
					{
						line.Text = Lang.tip[13].Value;
					}
				}
            }
        }
    }
}