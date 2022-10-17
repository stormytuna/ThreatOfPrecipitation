using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent;

namespace ThreatOfPrecipitation.Content.Projectiles.Weapons
{
    public class LaserGlaiveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Glaive");
			ProjectileID.Sets.TrailCacheLength[Type] = 9; // (Strength total * 3) / 2
			ProjectileID.Sets.TrailingMode[Type] = 0;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

		// Helper functions
		private float StrengthMultiplier => (float)StrengthValue / (float)StrengthTotal;
		private int StrengthValue => targetsHit.Count;
		private int StrengthTotal => 6;
		private void TryFindNewTarget(bool finalCheck)
        {
			NPC target = stormytunaUtils.GetClosestEnemy(Projectile.Center, searchRange, true, targetsHit);
			if (target == null)
			{
				currentTarget = -1;
				if (finalCheck)
                {
					Projectile.ai[0] = 2f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				return;
			}
			currentTarget = target.whoAmI;
			Projectile.ai[0] = 1f;
			Projectile.ai[1] = 0f;
			Projectile.netUpdate = true;
		}

		private int currentTarget = -1;
		private List<int> targetsHit = new List<int>();
		private float searchRange = 30f * 16f; // 30 tiles

		public override void AI()
		{
			Player owner = Main.player[Projectile.whoAmI];

			// Funky sound
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 8;
				SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
			}

			// Spinny :D
			Projectile.rotation += 0.25f * Projectile.direction; // Base
			Projectile.rotation += MathHelper.Lerp(0f, 0.15f, StrengthMultiplier); // Spin faster when stronger

			// Lighting
			Vector3 lightColor = new Vector3(0.5f, 1f, 1f);
			lightColor *= MathHelper.Lerp(0.4f, 1.2f, StrengthMultiplier);
			Lighting.AddLight(Projectile.Center, lightColor);

			// Dust
			int numDust = 2; // Base
			numDust += StrengthValue / 2; // More dust when stronger
			for (int i = 0; i < numDust; i++)
            {
				if (Main.rand.NextBool(2))
				{
					float scale = MathHelper.Lerp(0.8f, 1f, StrengthMultiplier);
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Cyan, 0f, 0f, 100, default, scale);
					dust.velocity *= 0.7f;
				}
            }

			// AI state 1 - travelling away from player 
			if (Projectile.ai[0] == 0f)
			{
				// Increase our frame counter
				Projectile.ai[1] += 1f;
				// Check if our frame counter is high enough without finding a target
				if (Projectile.ai[1] >= 30f)
				{
					Projectile.ai[0] = 2f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}

				// Try find a target if projectile has been active long enough
				if (Projectile.ai[1] >= 10f)
				{
					TryFindNewTarget(false);
				}

				return; // So this part can acts as a guard clause
			}

			// AI state 2 - travelling to next NPC to hit
			else if (Projectile.ai[0] == 1f) 
            {
				// Should travel through tiles
				Projectile.tileCollide = false;

				// Check target is still active, switch to a new one if possible
				NPC target = Main.npc[currentTarget];
				if (!target.active)
                {
					TryFindNewTarget(true);
                }

				// Curved velocity homing towards next NPC
				float rotTarget = Utils.ToRotation(target.Center - Projectile.Center);
				float rotCurrent = Utils.ToRotation(Projectile.velocity);
				float rotMax = MathHelper.ToRadians(25f);
				Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCurrent, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));

				return; // Again, guard clause
			}

			// AI state 3 - travelling back to player
			// Should travel through tiles
			Projectile.tileCollide = false;

			// Check if projectile is too far away and should just be killed
			float maxRange = 3000f;
			if (Vector2.DistanceSquared(owner.Center, Projectile.Center) > maxRange * maxRange)
            {
				Projectile.Kill();
            }

			// Add to our velocity 
			float maxVelocity = 20f;
			float homingStrength = 4f;
			Vector2 directionToPlayer = owner.Center - Projectile.Center;
			directionToPlayer.Normalize();
			directionToPlayer *= homingStrength;
			Projectile.velocity += directionToPlayer;
			if (Projectile.velocity.LengthSquared() > maxVelocity * maxVelocity)
            {
				Projectile.velocity.Normalize();
				Projectile.velocity *= maxVelocity;
            }

			// Catch our projectile
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.getRect().Intersects(Main.player[Projectile.owner].getRect()))
				{
					Projectile.Kill();
				}
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			// Check if our target is even an enemy 
			if (!target.active || target.CountsAsACritter || target.friendly)
			{
				// Bounce off NPCs we dont home in on 
				if (Projectile.ai[0] == 0f)
				{
					Projectile.velocity = -Projectile.velocity;
					Projectile.ai[0] = 2f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				return;
			}

			// Get our next target if we hit an NPC when not going back to the player
			if (Projectile.ai[0] != 2f)
            {
				// Increase our crit chance
				Projectile.CritChance += 5;
				// Check if we've hit all of our targets
				if (!targetsHit.Contains(target.whoAmI))
                {
					targetsHit.Add(target.whoAmI);
                }
				if (targetsHit.Count >= 6)
				{
					Projectile.ai[0] = 2f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;

					return;
				}
				// If not, get our next target
				TryFindNewTarget(true);
			}
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			// Makes dig sound when our projectile hits tiles
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			// Bounce off tiles 
			if (Projectile.ai[0] == 0f)
			{
				Projectile.velocity = -Projectile.velocity;
				Projectile.ai[0] =  2f;
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
			return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			// Ensures projectile doesnt collide with tiles immediately after its thrown
			width = 20;
			height = 20;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White; // I think this draws in fullbright?
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile in fullbright
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int i = 2; i < Projectile.oldPos.Length; i+=3) // Little funky, this draws every third trail
            {
				// So we only draw trails up to our strength
				if (i > StrengthValue * 1.5f)
                {
					break;
                }

				Vector2 drawPos = (Projectile.oldPos[i] - Main.screenPosition) + drawOrigin + new Vector2(0, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(currentTarget);
			writer.Write(targetsHit.Count);
			for (int i = 0; i < targetsHit.Count; i++)
            {
				writer.Write(targetsHit[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			currentTarget = reader.ReadInt32();
			int numTargets = reader.ReadInt32();
			targetsHit.Clear();
			for (int i = 0; i < numTargets; i++)
            {
				targetsHit.Add(reader.ReadInt32());
            }
        }
    }
}