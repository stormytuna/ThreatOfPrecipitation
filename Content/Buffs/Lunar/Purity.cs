using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class Purity : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Purity]");
            Description.SetDefault("20% increased attack speed [c/E3242B:but you feel very unlucky]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic) += 0.2f;
            player.GetModPlayer<LunarBuffPlayer>().purity = true; // Used for luck changes as they didnt work in here
        }
    }

    public class Purity_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purity");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Purity] lunar buff\n20% increased attack speed [c/E3242B:but you feel very unlucky]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }
}