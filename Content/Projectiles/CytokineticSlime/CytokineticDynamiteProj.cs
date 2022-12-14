using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace ThreatOfPrecipitation.Content.Projectiles.CytokineticSlime
{
    public class CytokineticDynamiteProj : ModProjectile
    {
        private int childCount = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cytokinetic Dynamite");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 16;
            Projectile.timeLeft = 160;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 160)
            {
                childCount = Projectile.timeLeft - 160;
                Projectile.timeLeft = 160;
            }

            // This makes sure our projectile does damage when it explodes
            if (Projectile.timeLeft <= 3)
            {
                Projectile.Resize(180, 180);
                Projectile.damage = childCount == 2 ? 250 : 100; // ie 100 damage if last child, otherwise 40 damage
                Projectile.knockBack = 10f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Gore gore;
            int range;
            int minX;
            int maxX;
            int minY;
            int maxY;
            bool shouldExplodeWalls;
            Projectile.Resize(10, 10); // Makes sure our projectile is set to the right size before using its position

            // If this isn't the third child, we should spawn a couple more bombs
            // Jank way to check for third child but ai[0] and ai[1] are fucked with by vanilla AI
            if (childCount != 2)
            {
                Vector2 vel = new Vector2(1.5f, -3f) + Projectile.velocity;
                vel *= Main.rand.NextFloat(0.8f, 1f);
                Projectile proj;


                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel, Type, 0, 0f, Projectile.owner, childCount + 1);
                proj.timeLeft += childCount + 1;

                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel * new Vector2(-1f, 1f), Type, 0, 0f, Projectile.owner, childCount + 1);
                proj.timeLeft += childCount + 1;

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
                // Some "gore" 
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X += 1f;
                gore.velocity.Y += 1f;
                gore.scale *= 0.8f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X += 1f;
                gore.velocity.Y -= 1f;
                gore.scale *= 0.8f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X -= 1f;
                gore.velocity.Y += 1f;
                gore.scale *= 0.8f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X -= 1f;
                gore.velocity.Y -= 1f;
                gore.scale *= 0.8f;
                #endregion

                // Small explosion
                range = 4;
                minX = (int)(Projectile.Center.X / 16f - range);
                maxX = (int)(Projectile.Center.X / 16f + range);
                minY = (int)(Projectile.Center.Y / 16f - range);
                maxY = (int)(Projectile.Center.Y / 16f + range);
                if (minX < 0) { minX = 0; }
                if (maxX > Main.maxTilesX) { maxX = Main.maxTilesX; }
                if (minY < 0) { minY = 0; }
                if (maxY > Main.maxTilesY) { maxY = Main.maxTilesY; }

                shouldExplodeWalls = Projectile.ShouldWallExplode(Projectile.Center, range, minX, maxX, minY, maxY);
                Projectile.ExplodeTiles(Projectile.Center, range, minX, maxX, minY, maxY, shouldExplodeWalls);

                return;
            }

            // If this is the third child, we should explode some stuff
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            #region Visuals
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
            // Some "gore" 
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X += 1f;
            gore.velocity.Y += 1f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X += 1f;
            gore.velocity.Y -= 1f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X -= 1f;
            gore.velocity.Y += 1f;
            gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
            gore.velocity.X -= 1f;
            gore.velocity.Y -= 1f;
            #endregion

            range = 6;
            minX = (int)(Projectile.Center.X / 16f - range);
            maxX = (int)(Projectile.Center.X / 16f + range);
            minY = (int)(Projectile.Center.Y / 16f - range);
            maxY = (int)(Projectile.Center.Y / 16f + range);
            if (minX < 0) { minX = 0; }
            if (maxX > Main.maxTilesX) { maxX = Main.maxTilesX; }
            if (minY < 0) { minY = 0; }
            if (maxY > Main.maxTilesY) { maxY = Main.maxTilesY; }

            shouldExplodeWalls = Projectile.ShouldWallExplode(Projectile.Center, range, minX, maxX, minY, maxY);
            Projectile.ExplodeTiles(Projectile.Center, range, minX, maxX, minY, maxY, shouldExplodeWalls);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Manually draw so we can change sprite and offset so it rolls properly
            Texture2D smallTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticDynamiteProj_Small").Value;
            Texture2D middleTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticDynamiteProj_Middle").Value;
            Texture2D largeTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticDynamiteProj_Large").Value;
            Texture2D textureToUse = largeTexture;
            if (childCount == 1) { textureToUse = middleTexture; }
            if (childCount == 2) { textureToUse = smallTexture; }

            Rectangle sourceRect = new Rectangle(0, 0, textureToUse.Width, textureToUse.Height);

            Vector2 origin = sourceRect.Size() / 2f;

            SpriteEffects spriteEffects = SpriteEffects.None; 
            if (Projectile.direction == -1) { spriteEffects = SpriteEffects.FlipHorizontally; }

            Main.spriteBatch.Draw(textureToUse, Projectile.Center - Main.screenPosition, sourceRect, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }
    }
}