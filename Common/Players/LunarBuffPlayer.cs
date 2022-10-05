using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Buffs.Lunar;

namespace ThreatOfPrecipitation.Common.Players
{
    public class LunarBuffPlayer : ModPlayer
    {
        // This checks our mouseItem to see if it's 
        public override void PreUpdate()
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseItem.type == ModContent.ItemType<ShapedGlass_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<ShapedGlass>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }

                if (Main.mouseItem.type == ModContent.ItemType<MercurialRachis_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<MercurialRachis>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }

                if (Main.mouseItem.type == ModContent.ItemType<StoneFluxPauldron_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<StoneFluxPauldron>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }
            }
        }

        public bool shapedGlass;
        public bool mercurialRachis;
        public bool mercurialRachisAura;
        public bool stoneFluxPauldron;

        public override void ResetEffects()
        {
            shapedGlass = false;
            mercurialRachis = false;
            mercurialRachisAura = false;
            stoneFluxPauldron = false;
        }

        public override void PostUpdateBuffs()
        {
            if (shapedGlass)
            {
                Player.statLifeMax2 /= 2;
            }

            if (mercurialRachis || mercurialRachisAura)
            {
                Player.GetDamage(DamageClass.Generic) += 0.2f;
            }

            if (stoneFluxPauldron)
            {
                Player.maxRunSpeed /= 2f;
                Player.moveSpeed /= 2f;
                Player.wingRunAccelerationMult /= 2f;
                Player.runAcceleration /= 2f;
                Player.runSoundDelay *= 2;
                Player.accRunSpeed /= 2f;
                Player.wingAccRunSpeed /= 2f;
            }
            Main.NewText(Player.velocity);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (shapedGlass)
            {
                damage *= 2;
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (shapedGlass)
            {
                damage *= 2;
            }
        }
    }
}