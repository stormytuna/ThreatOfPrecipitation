using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Projectiles.Weapons
{
    public class PiercingWindProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Piercing Wind Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 400;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            // Check if we should tile collide
            if (Projectile.ai[0] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) && Projectile.Center.Y < Main.player[Projectile.owner].Center.Y + Main.screenHeight / 2)
            {
                Projectile.ai[0] = 1f;
                Projectile.tileCollide = true;
                Projectile.netUpdate = true;
            }

            // Dust 
            for (int i = 0; i < 3; i++)
            {
                Vector2 velocity = Vector2.One.RotateRandom(MathHelper.TwoPi);
                velocity *= Main.rand.NextFloat(0.8f, 1.2f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Clentaminator_Cyan, velocity, 50, default, Main.rand.NextFloat(0.8f, 1.2f));
                dust.noGravity = true;
            }

            // Light homing
            float range = 30 * 16f;
            NPC closestNPC = stormytunaUtils.GetClosestEnemy(Projectile.Center, range, true);
            // Guard clause
            if (closestNPC == null)
                return;

            Projectile.velocity = stormytunaUtils.RotateVelocityHoming(Projectile.velocity, Projectile.Center, closestNPC.Center, 5f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Makes dig sound when our projectile hits tiles
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            // Dust 
            for (int i = 0; i < 16; i++)
            {
                Vector2 velocity = Vector2.One.RotateRandom(MathHelper.TwoPi);
                velocity *= Main.rand.NextFloat(2f, 4f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Clentaminator_Cyan, velocity, 50, default, Main.rand.NextFloat(0.8f, 1.2f));
                dust.noGravity = true;
            }
        }
    }
}