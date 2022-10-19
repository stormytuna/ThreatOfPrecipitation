using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;

namespace ThreatOfPrecipitation.Content.Projectiles
{
    public class MoltenPerforatorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exploding Fireball");
        }

        public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
            Projectile.aiStyle = 1; // Arrow AI (i think?)
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Default;
			Projectile.timeLeft = 10 * 60;
        }

        public override void AI()
        {
            // Adapted from vanilla code - Terraria.Projectile.cs at line 44885
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                    dust.velocity.Y += Math.Sign(dust.velocity.Y) * 1.2f;
                    dust.fadeIn += 0.5f;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathF.PI / 2f;
        }

		// Tucked away because of massive uncompressed vanilla code, woo
		#region KillHook
		public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item100, Projectile.position);

			// Code "adapted" from vanilla Terraria.Projectile.cs at line 46911
			// I cba condensing it :)
			int num77 = 4;
			int num78 = 20;
			int num79 = 10;
			int num80 = 20;
			int num81 = 20;
			int num82 = 4;
			float num83 = 1.5f;
			int num84 = 6;
			int num85 = 6;

			Projectile.position = Projectile.Center;
			Projectile.width = (Projectile.height = 16 * num84);
			Projectile.Center = Projectile.position;
			for (int num86 = 0; num86 < num77; num86++)
			{
				int num87 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num87].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
			}

			for (int num88 = 0; num88 < num78; num88++)
			{
				Dust dust19 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 200, default(Color), 2.5f);
				dust19.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 10f;
				Dust dust = dust19;
				dust.velocity *= 16f;
				if (dust19.velocity.Y > -2f)
					dust19.velocity.Y *= -0.4f;

				dust19.noLight = true;
				dust19.noGravity = true;
			}

			for (int num89 = 0; num89 < num80; num89++)
			{
				Dust dust20 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num85, 0f, 0f, 100, default(Color), 1.5f);
				dust20.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
				Dust dust = dust20;
				dust.velocity *= 2f;
				dust20.noGravity = true;
				dust20.fadeIn = num83;
			}

			for (int num90 = 0; num90 < num79; num90++)
			{
				int num91 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default(Color), 2.7f);
				Main.dust[num91].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
				Main.dust[num91].noGravity = true;
				Dust dust = Main.dust[num91];
				dust.velocity *= 3f;
			}

			for (int num92 = 0; num92 < num81; num92++)
			{
				int num93 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num93].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
				Main.dust[num93].noGravity = true;
				Dust dust = Main.dust[num93];
				dust.velocity *= 3f;
			}

			for (int num94 = 0; num94 < num82; num94++)
			{
				int num95 = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2((float)(Projectile.width * Main.rand.Next(100)) / 100f, (float)(Projectile.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64));
				Main.gore[num95].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
				Gore gore = Main.gore[num95];
				gore.position -= Vector2.One * 16f;
				if (Main.rand.NextBool(2))
					Main.gore[num95].position.Y -= 30f;

				gore = Main.gore[num95];
				gore.velocity *= 0.3f;
				Main.gore[num95].velocity.X += (float)Main.rand.Next(-10, 11) * 0.05f;
				Main.gore[num95].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.05f;
			}
		}
        #endregion

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.Kill();
            return base.OnTileCollide(oldVelocity);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Lerp(lightColor, Color.White, 0.5f);
            return new Color(color.R, color.G, color.B, 200) * Projectile.Opacity;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.timeLeft < 10 * 60 - 12;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(4, 9));
            }
        }
    }
}