using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class LightFluxPauldron : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Light Flux Pauldron]");
            Description.SetDefault("Debuff time is halved [c/E3242B:but attack speed is halved]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().lightFluxPauldron = true;
        }
    }

    public class LightFluxPauldron_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Flux Pauldron");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Light Flux Pauldron] lunar buff\nDebuff time is halved [c/E3242B:but attack speed is halved]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }
}