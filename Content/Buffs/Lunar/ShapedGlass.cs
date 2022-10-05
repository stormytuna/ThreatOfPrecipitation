using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class ShapedGlass : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Shaped Glass]");
            Description.SetDefault("Damage is doubled [c/E3242B:but health is halved]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().shapedGlass = true;
        }
    }

    public class ShapedGlass_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shaped Glass");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Shaped Glass] lunar buff\nDamage is doubled [c/E3242B:but health is halved]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }
}