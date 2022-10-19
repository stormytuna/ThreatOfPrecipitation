using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Projectiles.Weapons
{
    public class TeslaBeaconSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tesla Beacon Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 959;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 16 * 60;
        }

        public override void AI()
        {
            // Dust
            if (Projectile.timeLeft < 940) // ie dont make dust on first 10 frames of life
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 230, Vector2.Zero, 40, default, Main.rand.NextFloat(0.5f, 0.7f));
                d.noGravity = true;
            }
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<TeslaBeaconDebuff>(), 5 * 60);
        }
    }
    
    public class TeslaBeaconDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<TeslaBeaconGlobalNPC>().teslaBeaconDebuff = true;
            npc.defense -= 5;
        }
    }

    public class TeslaBeaconGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool teslaBeaconDebuff;

        public override void ResetEffects(NPC npc)
        {
            teslaBeaconDebuff = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (teslaBeaconDebuff)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 16;
                if (damage < 2)
                    damage = 2;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (teslaBeaconDebuff)
                drawColor = NPC.buffColor(drawColor, 0.5f, 1f, 0.8f, 1f);
        }
    }
}