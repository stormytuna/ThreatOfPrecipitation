using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace ThreatOfPrecipitation.Content.Projectiles.CytokineticSlime
{
    public class CytokineticGlowstickProj : ModProjectile
    {
        private ref float ChildCount => ref Projectile.ai[1];
        bool firstFrame = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cytokinetic Glowstick");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 14;
            Projectile.netImportant = true;
            Projectile.alpha = 75;
            Projectile.timeLeft = 80;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0);
        }

        public override void AI()
        {
            if (ChildCount == 2)
            {
                // Should stay alive for 5 minutes
                if (firstFrame)
                {
                    Projectile.timeLeft = 5 * 60 * 60;
                    firstFrame = false;
                }

                // Lighting
                Lighting.AddLight(Projectile.Center, new Vector3(0.87f, 0.61f, 0f) * 2f);
            }
            else if (ChildCount == 1)
            {
                // Lighting
                Lighting.AddLight(Projectile.Center, new Vector3(0.91f, 0.43f, 0f) * 2f);
            }
            else
            {
                // Lighting
                Lighting.AddLight(Projectile.Center, new Vector3(0.76f, 0.28f, 0f) * 2f);
            }
        }

        public override void Kill(int timeLeft)
        {
            // If this isn't the third child, we should spawn a couple more glowsticks
            if (ChildCount != 2)
            {
                Vector2 vel = new Vector2(1.5f, -3f) + Projectile.velocity;
                vel *= Main.rand.NextFloat(0.8f, 1f);
                Projectile proj;

                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel, Type, 0, 0f, Projectile.owner);
                proj.ai[1] = ChildCount + 1;

                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel * new Vector2(-1f, 1f), Type, 0, 0f, Projectile.owner);
                proj.ai[1] = ChildCount + 1;

                #region Visuals
                // Sound :D
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
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
                #endregion

                return;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D smallTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticGlowstickProj_Small").Value;
            Texture2D middleTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticGlowstickProj_Middle").Value;
            Texture2D largeTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticGlowstickProj_Large").Value;
            Texture2D textureToUse = largeTexture;
            if (ChildCount == 1) { textureToUse = middleTexture; }
            if (ChildCount == 2) { textureToUse = smallTexture; }

            Rectangle sourceRect = new Rectangle(0, 0, textureToUse.Width, textureToUse.Height);

            Vector2 origin = sourceRect.Size() / 2f;

            SpriteEffects spriteEffects = SpriteEffects.None; 
            if (Projectile.direction == -1) { spriteEffects = SpriteEffects.FlipHorizontally; }

            Main.spriteBatch.Draw(textureToUse, Projectile.Center - Main.screenPosition, sourceRect, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }
    }
}