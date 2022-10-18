using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace ThreatOfPrecipitation.Content.Projectiles.Weapons
{
	public class TeslaBeaconDropPodProj : ModProjectile
	{
		private ref float AI_WorldX => ref Projectile.ai[0];
		private ref float AI_WorldY => ref Projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Beacon Drop Pod");
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 68;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}

		private float oldCenterX;

        public override void OnSpawn(IEntitySource source)
        {
			oldCenterX = Projectile.Center.X;
        }

        public override void AI()
        {
			// Spawns our sentry
			bool crossedWorldXThreshhold = (Projectile.Top.X < AI_WorldX && oldCenterX > AI_WorldX) || (Projectile.Top.X > AI_WorldX && oldCenterX < AI_WorldX);
			if (crossedWorldXThreshhold)
            {
				Projectile sentry = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), new Vector2(AI_WorldX, AI_WorldY), Vector2.Zero, ModContent.ProjectileType<TeslaBeaconSentry>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai1: 1f);
				sentry.originalDamage = Projectile.damage;
				Projectile.Kill();
				Main.player[Projectile.owner].UpdateMaxTurrets();
            }

			// Visuals
			// Point where facing
			Projectile.rotation = Projectile.velocity.ToRotation() + 3 * MathHelper.PiOver2;
			// Dust
			for (int i = 0; i < 5; i++)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 dustVelocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, dustVelocity.X, dustVelocity.Y, 100, default, Main.rand.NextFloat(1.2f, 1.5f));
                    d.noLight = true;
                }
            }
			for (int i = 0; i < 5; i++)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 dustVelocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, dustVelocity.X, dustVelocity.Y);
                    d.noLight = true;
                }
			}
            // Sound 
            SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
		}

        public override void Kill(int timeLeft)
        {
            Gore gore = new Gore();
            // Smoke dust
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.velocity *= 1.4f;
            }
            // More dust
            for (int i = 0; i < 6; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
            // Even more dust
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 3f);
                dust.velocity *= 1.4f;
            }
            // Smoke gore
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X += 1f;
            gore.velocity.Y += 1f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X += 1f;
            gore.velocity.Y -= 1f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X -= 1f;
            gore.velocity.Y += 1f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X -= 1f;
            gore.velocity.Y -= 1f;
            //Parts gore
            //Vector2 position = new Vector2(Projectile.Center.X + Projectile.Center.Y);
            int gore1 = Mod.Find<ModGore>("TeslaBeaconDropPodGore1").Type;
            int gore2 = Mod.Find<ModGore>("TeslaBeaconDropPodGore2").Type;
            int gore3 = Mod.Find<ModGore>("TeslaBeaconDropPodGore3").Type;
            int gore4 = Mod.Find<ModGore>("TeslaBeaconDropPodGore4").Type;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore1);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore2);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore2);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore2);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore3);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore3);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore4);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Top, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), gore4);
            gore.timeLeft = (int)(gore.timeLeft * 0.4);
            // And finally, sound :D
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }
    }
}