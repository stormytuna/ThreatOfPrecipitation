using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class StoneFluxPauldron : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Stone Flux Pauldron]");
            Description.SetDefault("Health is doubled [c/E3242B:but movement speed is halved]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().stoneFluxPauldron = true;
        }
    }

    public class StoneFluxPauldron_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Flux Pauldron");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Stone Flux Pauldron] lunar buff\nHealth is doubled [c/E3242B:but movement speed is halved]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }
}