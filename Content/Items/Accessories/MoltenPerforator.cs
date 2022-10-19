using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ThreatOfPrecipitation.Content.Projectiles;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    public class MoltenPerforator : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Chance on hit for 3 exploding fireballs to spew out of your target");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Expert;
            Item.accessory = true;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MoltenPerforatorPlayer>().moltenPerforator = true;
        }
    }

    public class MoltenPerforatorPlayer : ModPlayer
    {
        public bool moltenPerforator;

        public override void ResetEffects()
        {
            moltenPerforator = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (moltenPerforator && Main.rand.NextBool(10))
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 velocity = new Vector2(0f, -3f);
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(90f));
                    velocity *= Main.rand.NextFloat(0.7f, 1.5f);
                    Projectile ball = Projectile.NewProjectileDirect(Player.GetSource_Misc("-1"), target.Center, velocity, ModContent.ProjectileType<MoltenPerforatorProj>(), damage, knockback * 0.5f, Player.whoAmI);
                    target.immune[ball.whoAmI] = 15;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (moltenPerforator && Main.rand.NextBool(10) && proj.type != ModContent.ProjectileType<MoltenPerforatorProj>())
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 velocity = new Vector2(0f, -3f);
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(70f));
                    velocity *= Main.rand.NextFloat(0.8f, 1.2f);
                    Projectile ball = Projectile.NewProjectileDirect(Player.GetSource_Misc("-1"), target.Center, velocity, ModContent.ProjectileType<MoltenPerforatorProj>(), damage, knockback * 0.5f, Player.whoAmI);
                    target.immune[ball.whoAmI] = 15;
                }
            }
        }
    }
}