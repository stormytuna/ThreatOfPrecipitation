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
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 16, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion
                    return;
                }
            }
        }

        public bool shapedGlass;

        public override void ResetEffects()
        {
            shapedGlass = false;
        }

        public override void PostUpdateBuffs()
        {
            if (shapedGlass)
            {
                Player.statLifeMax2 /= 2;
            }
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