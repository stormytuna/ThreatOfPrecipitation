using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace ThreatOfPrecipitation.Content.Projectiles.CytokineticSlime
{
    public class CytokineticGrenadeProj : ModProjectile
    {
        private int childCount = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cytokinetic Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.penetrate = 5;
            Projectile.aiStyle = 16;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 80;
            Projectile.damage = 12;
            Projectile.knockBack = 3f;
            Projectile.usesIDStaticNPCImmunity = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 80)
            {
                childCount = Projectile.timeLeft - 80;
                Projectile.timeLeft = 80;
            }

            // This makes sure our projectile does damage when it explodes
            if (Projectile.timeLeft <= 3)
            {
                Projectile.Resize(100, 100);
                Projectile.damage = 12;
                Projectile.knockBack = 3f;
            }

            // This makes sure there's a delay from the grenade being thrown/created to it being able to deal damage
            if (Projectile.timeLeft < 70)
            {
                Projectile.friendly = true;
                Projectile.damage = 12;
                Projectile.knockBack = 3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.timeLeft > 2)
                Projectile.timeLeft = 2;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.active && !target.friendly && target.damage > 0 && !target.dontTakeDamage && Main.LocalPlayer.CanNPCBeHitByPlayerOrPlayerProjectile(target);
        }

        public override void Kill(int timeLeft)
        {
            Gore gore;
            Projectile.Resize(14, 14);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // If this isn't the third child, we should spawn a couple more bombs
            // Jank way to check for third child but ai[0] and ai[1] are fucked with by vanilla AI
            if (childCount != 2)
            {
                Vector2 vel = new Vector2(3f, -3f);
                vel *= Main.rand.NextFloat(0.6f, 1f);
                Projectile proj;

                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel, Type, 0, 0f, Projectile.owner);
                proj.timeLeft += childCount + 1;

                proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel * new Vector2(-1f, 1f), Type, 0, 0f, Projectile.owner);
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

                return;
            }

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
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Manually draw so we can change sprite and offset so it rolls properly
            Texture2D smallTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticGrenadeProj_Small").Value;
            Texture2D middleTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticGrenadeProj_Middle").Value;
            Texture2D largeTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Projectiles/CytokineticSlime/CytokineticGrenadeProj_Large").Value;
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