using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ThreatOfPrecipitation.Content.Projectiles.Weapons;
using Terraria.GameContent.Creative;

namespace ThreatOfPrecipitation.Content.Items.Weapons
{
    public class LaserGlaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A seeking glaive that bounces up to 6 times, each bounce increasing crit chance by 5%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 38;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 36;
            Item.useTime = 36;
            Item.rare = ItemRarityID.Pink;
            Item.noMelee = true;
            Item.value = Item.sellPrice(gold: 6);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noUseGraphic = true;
            Item.damage = 80;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 17f;
            Item.shoot = ModContent.ProjectileType<LaserGlaiveProj>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<LaserGlaiveProj>()] < 1;
        }
    }
}