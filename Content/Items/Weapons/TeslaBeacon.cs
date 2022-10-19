using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Projectiles.Weapons;

namespace ThreatOfPrecipitation.Content.Items.Weapons
{
    public class TeslaBeacon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a sentry\nA beacon that fires lightning at nearby enemies, [c/7DF9FF:Shocking] them\n[c/7DF9FF:Shocked:] Lose 8 life per second and decreased defense by 5");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 14;
            Item.UseSound = SoundID.Item129;
            Item.useAnimation = 36;
            Item.useTime = 36;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 6);
            Item.DamageType = DamageClass.Summon;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.damage = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<TeslaBeaconDropPodProj>();
            Item.shootSpeed = 1f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(ModContent.ProjectileType<TeslaBeaconSentry>(), out int worldX, out int worldY, out int pushYUp);
            // Create our drop pod with ai[0] == worldX and ai[1] == worldY + pushYUp, we'll calculate velocity such that it crosses (worldX, worldY), then kills itself, then summons our sentry with the pushYUp offset
            Vector2 target = new Vector2(worldX, worldY);
            Vector2 spawnPosition = target + new Vector2(0f, -2000);
            spawnPosition.X += Main.rand.NextBool(2) ? -15f * 16f : 15f * 16f;
            spawnPosition.X += Main.rand.NextFloat(-3f * 16f, 3f * 16f);
            Vector2 heading = target - spawnPosition;
            heading.Normalize();
            heading *= 10f;
            Projectile dropPod = Projectile.NewProjectileDirect(source, spawnPosition, heading, type, damage, knockback, player.whoAmI, worldX, worldY - pushYUp + 7f);
            dropPod.originalDamage = damage; // Then we do the same in our drop pods code to our turret

            return false;
        }
    }
}