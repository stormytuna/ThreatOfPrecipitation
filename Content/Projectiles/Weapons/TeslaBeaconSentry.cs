using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThreatOfPrecipitation.Content.Projectiles.Weapons
{
    public class TeslaBeaconSentry : ModProjectile
	{
		private ref float AI_AttackRegenCooldown => ref Projectile.ai[0];
		private ref float AI_StoredAttacks => ref Projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Beacon Sentry");
			Main.projFrames[Type] = 26;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 44;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.sentry = true;
			Projectile.netImportant = true;
			Projectile.timeLeft = 7200;
			Projectile.aiStyle = -1;
		}

		public override bool? CanHitNPC(NPC target) => false;

        private bool CanShootRight => canShootRightFrames.Contains(Projectile.frame);
		private bool CanShootLeft => canShootLeftFrames.Contains(Projectile.frame);
		private Vector2 ShootPoint => Projectile.Center + new Vector2(0f, -8f);

		private int[] canShootRightFrames = new int[] { 1, 2, 3, 4, 5};
		private int[] canShootLeftFrames = new int[] { 14, 15, 16, 17, 18};
		private int maxShootCooldown = 6;
		private float maxAttackRegenCooldown = 15f;
		private float maxStoredAttacks = 4f;
		private float range = 30 * 16f;

		private int shootCooldown = 0;

        public override void AI()
        {
			Projectile.velocity = Vector2.Zero;

			// Animation stuff
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4)
            {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Type])
					Projectile.frame = 0;
			}

			// Try add another attack
			if (AI_StoredAttacks < maxStoredAttacks && AI_AttackRegenCooldown < 0)
            {
				AI_AttackRegenCooldown = maxAttackRegenCooldown;
				AI_StoredAttacks++;
				Projectile.netUpdate = true;
            }

			// Set our stored attack cooldown to max if we have max attacks
			if (AI_StoredAttacks >= maxStoredAttacks)
				AI_AttackRegenCooldown = maxAttackRegenCooldown;

			// Kill if player dies
			Player owner = Main.player[Projectile.owner];
			if (owner.dead || !owner.active)
				Projectile.Kill();

			// Decrement cooldowns
			AI_AttackRegenCooldown--;
			shootCooldown--;

			// Guard clauses 
			if ((!CanShootLeft && !CanShootRight) || shootCooldown > 0 || AI_StoredAttacks < 1f)
			{
				return;
			}

			List<NPC> closeNPCs = stormytunaUtils.GetNearbyEnemies(Projectile.Center, range, true);
			while (closeNPCs.Count > 0)
            {
				NPC randomNPC = closeNPCs[Main.rand.Next(0, closeNPCs.Count)];
				closeNPCs.Remove(randomNPC);
				float angleToNPC = (randomNPC.Center - ShootPoint).ToRotation();

				// Check if its above us
				if (stormytunaUtils.RotationIsWithinRange(-MathHelper.PiOver2, angleToNPC, MathHelper.ToRadians(20f)))
                {
					ShootNPC(randomNPC);
					return;
                }

				// Check if its below us
				if (stormytunaUtils.RotationIsWithinRange(MathHelper.PiOver2, angleToNPC, MathHelper.ToRadians(20f)))
				{
					ShootNPC(randomNPC);
					return;
				}

				// Check if its to the left
				if (CanShootLeft && randomNPC.Center.X < ShootPoint.X)
				{
					ShootNPC(randomNPC);
					return;
				}

				// Check if its to the right
				if (CanShootRight && randomNPC.Center.X > ShootPoint.X)
				{
					ShootNPC(randomNPC);
					return;
				}
			}
		}

		public void ShootNPC(NPC target)
        {
			// Create projectile
			Vector2 velocity = target.Center - ShootPoint;
			velocity.Normalize();
			velocity *= 0.5f;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), ShootPoint, velocity, ModContent.ProjectileType<TeslaBeaconSentryProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

			// Set cooldowns
			shootCooldown = maxShootCooldown;
			AI_StoredAttacks--;
			Projectile.netUpdate = true;

			// Sound
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, ShootPoint);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(shootCooldown);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			shootCooldown = reader.ReadInt32();
        }
    }
}