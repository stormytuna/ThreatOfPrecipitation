using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class Transcendence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Transcendence]");
            Description.SetDefault("Health is doubled [c/E3242B:but you cannot regenerate and healing is half as effective]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().transendence = true;
        }
    }

    public class Transcendence_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Transcendence");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Transcendence] lunar buff\nHealth is doubled [c/E3242B:but you cannot regenerate and healing is half as effective]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }
}