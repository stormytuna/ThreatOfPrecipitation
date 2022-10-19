using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class GestureOfTheDrowned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Gesture of the Drowned]");
            Description.SetDefault("Healing potions are more effective [c/E3242B:but you automatically drink them whenever possible]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().gestureOfTheDrowned = true;
        }
    }

    public class GestureOfTheDrowned_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gesture of the Drowned");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Gesture of the Drowned] lunar buff\nHealing potions are more effective [c/E3242B:but you automatically drink them whenever possible]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }
}