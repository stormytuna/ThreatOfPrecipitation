using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Buffs.Lunar;
using Microsoft.Xna.Framework;
using ThreatOfPrecipitation.Common.GlobalProjectiles;
using Terraria.Localization;

namespace ThreatOfPrecipitation.Common.Players
{
    public class LunarBuffPlayer : ModPlayer
    {
        // This checks our mouseItem to see if it's a lunar buff
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

                if (Main.mouseItem.type == ModContent.ItemType<LightFluxPauldron_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<LightFluxPauldron>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }

                if (Main.mouseItem.type == ModContent.ItemType<BrittleCrown_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<BrittleCrown>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }

                if (Main.mouseItem.type == ModContent.ItemType<Purity_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<Purity>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }

                if (Main.mouseItem.type == ModContent.ItemType<Transcendence_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<Transcendence>(), 10 * 60 * 60);

                    #region Visuals

                    for (int i = 0; i < 40; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0f, 0f, 40, default, 1.2f);
                        dust.velocity *= 2f;
                    }

                    #endregion

                    return;
                }

                if (Main.mouseItem.type == ModContent.ItemType<GestureOfTheDrowned_Item>())
                {
                    Main.mouseItem.TurnToAir();
                    Player.AddBuff(ModContent.BuffType<GestureOfTheDrowned>(), 10 * 60 * 60);

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
        public bool lightFluxPauldron;
        public bool brittleCrown;
        public bool purity;
        public bool transendence;
        public bool transendenceHeal = true; // Heals to full health if youve just got the buff
        public bool gestureOfTheDrowned;

        public bool[] doLightFluxReduceDebuffTime = new bool[Player.MaxBuffs];
        public bool[] doGestureReduceDebuffTime = new bool[Player.MaxBuffs];

        public override void ResetEffects()
        {
            shapedGlass = false;
            mercurialRachis = false;
            mercurialRachisAura = false;
            stoneFluxPauldron = false;
            lightFluxPauldron = false;
            brittleCrown = false;
            purity = false;
            transendence = false;
            gestureOfTheDrowned = false;
        }

        public override void PreUpdateBuffs()
        {
            if (Player.HasBuff(ModContent.BuffType<LightFluxPauldron>()))
            {
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Player.buffTime[i] <= 0)
                    {
                        doLightFluxReduceDebuffTime[i] = true;
                    }

                    if (doLightFluxReduceDebuffTime[i] && Main.debuff[Player.buffType[i]])
                    {
                        doLightFluxReduceDebuffTime[i] = false;
                        Player.buffTime[i] /= 2;
                    }
                }
            }

            if (Player.HasBuff(ModContent.BuffType<GestureOfTheDrowned>()))
            {
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Player.buffTime[i] <= 0)
                    {
                        doGestureReduceDebuffTime[i] = true;
                    }

                    if (doGestureReduceDebuffTime[i] && Player.buffType[i] == BuffID.PotionSickness)
                    {
                        doGestureReduceDebuffTime[i] = false;
                        Player.buffTime[i] = (int)(Player.buffTime[i] / 1.5f);
                    }
                }
            }

            if (!Player.HasBuff(ModContent.BuffType<Transcendence>()))
            {
                transendenceHeal = true;
            }
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

            if (lightFluxPauldron)
            {
                Player.GetAttackSpeed(DamageClass.Generic) /= 2;
            }

            if (transendence)
            {
                Player.statLifeMax2 += Player.statLifeMax;
                if (transendenceHeal)
                {
                    Player.statLife = Player.statLifeMax2;
                    transendence = false;
                }
            }
        }

        public override void PostUpdateRunSpeeds()
        {
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
        }

        public override void PostUpdate()
        {
            if (purity)
            {
                Player.luck = Player.luckMinimumCap;
                Player.luckNeedsSync = true;
            }

            if (gestureOfTheDrowned && Player.potionDelay <= 0 && Player.statLife < Player.statLifeMax2)
            {
                Player.QuickHeal();
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (transendence)
            {
                Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
            }
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (transendence)
            {
                healValue /= 2;
            }

            if (gestureOfTheDrowned)
            {
                healValue = (int)(healValue * 1.5f);
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (brittleCrown && Main.rand.NextBool(1))
            {
                TryStealBrittleCrownCoin(damage / Player.statLifeMax2);
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (proj.TryGetGlobalProjectile(out LunarBuffGlobalProjectile modProj))
            {
                if (proj.npcProj && brittleCrown && Main.rand.NextBool(1))
                {
                    TryStealBrittleCrownCoin(damage / Player.statLifeMax2);
                }
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

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (brittleCrown)
                TryMakeBrittleCrownCoin(target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (brittleCrown)
                TryMakeBrittleCrownCoin(target);
        }

        private void TryMakeBrittleCrownCoin(NPC target)
        {
            if (target.value > 0f && Main.rand.NextBool(15))
            {
                int numCoins = Main.rand.Next(2, 6);
                for (int i = 0; i < numCoins; i++)
                {
                    Vector2 vel = new Vector2(1f, 1f).RotatedByRandom(MathHelper.TwoPi) * 3f;
                    Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center, vel, ModContent.ProjectileType<BrittleCrown_CoinProjectile>(), 0, 0f, Player.whoAmI);
                }
            }
        }

        private void TryStealBrittleCrownCoin(float percentHealthLost)
        {
            // Loop through each of the players inventories, if we find a coin have a % chance based on the coin to lose a % based on the percentage of health lost
            List<Item[]> inventories = new List<Item[]>();
            inventories.Add(Player.inventory);
            inventories.Add(Player.bank.item);
            inventories.Add(Player.bank2.item);
            inventories.Add(Player.bank3.item);
            inventories.Add(Player.bank4.item);
            
            int coinValueLost = 0;

            foreach (Item[] inventory in inventories)
            {
                foreach (Item item in inventory)
                {
                    if (item.IsACoin)
                    {
                        bool loseCoin = false;
                        int coinValue = 0;
                        switch (item.type)
                        {
                            case ItemID.CopperCoin:
                                loseCoin = Main.rand.NextBool(2);
                                coinValue = Item.buyPrice(copper: 1);
                                break;
                            case ItemID.SilverCoin:
                                loseCoin = Main.rand.NextBool(4);
                                coinValue = Item.buyPrice(silver: 1);
                                break;
                            case ItemID.GoldCoin:
                                loseCoin = Main.rand.NextBool(8);
                                coinValue = Item.buyPrice(gold: 1);
                                break;
                            case ItemID.PlatinumCoin:
                                loseCoin = Main.rand.NextBool(16);
                                coinValue = Item.buyPrice(platinum: 1);
                                break;
                        }

                        if (loseCoin)
                        {
                            int numToLose = (int)MathHelper.Lerp(1f, 10f, percentHealthLost);
                            if (numToLose > item.stack)
                                numToLose = item.stack;

                            item.stack -= numToLose;
                            if (item.stack <= 0)
                                item.TurnToAir();

                            coinValueLost += numToLose * coinValue;
                        }
                    }
                }
            }

            #region Visuals
            if (coinValueLost > 0)
            {
                // Code copied from vanilla - PopupText.ValueToName()
                int platinumCoins = 0;
                int goldCoins = 0;
                int silverCoins = 0;
                int copperCoins = 0;
                while (coinValueLost > 0)
                {
                    if (coinValueLost >= 1000000)
                    {
                        coinValueLost -= 1000000;
                        platinumCoins++;
                    }
                    else if (coinValueLost >= 10000)
                    {
                        coinValueLost -= 10000;
                        goldCoins++;
                    }
                    else if (coinValueLost >= 100)
                    {
                        coinValueLost -= 100;
                        silverCoins++;
                    }
                    else if (coinValueLost >= 1)
                    {
                        coinValueLost--;
                        copperCoins++;
                    }
                }

                string combatText = "Lost ";
                if (platinumCoins > 0)
                    combatText = combatText + platinumCoins + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));

                if (goldCoins > 0)
                    combatText = combatText + goldCoins + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));

                if (silverCoins > 0)
                    combatText = combatText + silverCoins + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));

                if (copperCoins > 0)
                    combatText = combatText + copperCoins + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));

                if (combatText.Length > 1)
                    combatText = combatText.Substring(0, combatText.Length - 1);

                int ct = CombatText.NewText(Player.getRect(), Color.Red, combatText);
                Main.combatText[ct].velocity *= 0.6f;
                Main.combatText[ct].scale *= 1.2f;
            }
            #endregion
        }
    }
}