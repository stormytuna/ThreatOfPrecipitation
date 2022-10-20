using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.ItemDropRules;
using System;
using Terraria.Audio;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Creative;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    public class Gasoline : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Enemies you kill explode");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GasolinePlayer>().gasoline = true;
        }
    }

    public class GasolineGlobalNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            // Wacky way of putting gasoline earlier in the demolitionists shop
            List<Item> inventory = shop.item.ToList();
            Item item = new Item();
            item.SetDefaults(ModContent.ItemType<Gasoline>());
            inventory.Insert(3, item); // Places our gasoline after dynamite and before everything else
            shop.item = inventory.ToArray();
            nextSlot++;
        }
    }

    public class GasolinePlayer : ModPlayer
    {
        public bool gasoline;
        public float gasolineExplosionRange = 5f * 16f;

        public override void ResetEffects()
        {
            gasoline = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            TryGasolineExplosion(target, damage);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            TryGasolineExplosion(target, damage);
        }

        public void TryGasolineExplosion(NPC target, int damage)
        {
            if (gasoline && target.life <= 0)
            {
                #region Effects
                // Sound :D
                SoundEngine.PlaySound(SoundID.Item14, target.Center);
                // Smoke dust
                for (int i = 0; i < 12; i++)
                {
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(2f, 2.5f), 0f).RotatedByRandom(MathHelper.TwoPi);
                    Dust dust = Dust.NewDustPerfect(target.Center, DustID.Smoke, velocity, 100, default, 1.5f);
                }
                // More dust
                for (int i = 0; i < 6; i++)
                {
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(2f, 2.5f), 0f).RotatedByRandom(MathHelper.TwoPi);
                    Dust dust = Dust.NewDustPerfect(target.Center, DustID.Torch, velocity, 100, default, 2.5f);
                    dust.velocity *= 1.4f;
                    dust.noGravity = true;
                    velocity = new Vector2(Main.rand.NextFloat(2f, 2.5f), 0f).RotatedByRandom(MathHelper.TwoPi);
                    dust = Dust.NewDustPerfect(target.Center, DustID.Torch, velocity, 100, default, 1.5f);
                    dust.velocity *= 1.4f;
                    dust.noGravity = true;
                }
                // Some "gore" 
                Gore gore = Gore.NewGoreDirect(Player.GetSource_FromThis(), target.Center, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X += 1f;
                gore.velocity.Y += 1f;
                gore.scale *= 0.8f;
                gore = Gore.NewGoreDirect(Player.GetSource_FromThis(), target.Center, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X += 1f;
                gore.velocity.Y -= 1f;
                gore.scale *= 0.8f;
                gore = Gore.NewGoreDirect(Player.GetSource_FromThis(), target.Center, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X -= 1f;
                gore.velocity.Y += 1f;
                gore.scale *= 0.8f;
                gore = Gore.NewGoreDirect(Player.GetSource_FromThis(), target.Center, Vector2.Zero, Main.rand.Next(61, 64));
                gore.velocity.X -= 1f;
                gore.velocity.Y -= 1f;
                gore.scale *= 0.8f;
                #endregion

                // Actual damage
                List<NPC> closeNPCs = stormytunaUtils.GetNearbyEnemies(target.Center, gasolineExplosionRange, true);
                foreach (NPC npc in closeNPCs)
                {
                    int direction = MathF.Sign((npc.Center - target.Center).X);
                    Player.ApplyDamageToNPC(npc, damage * 3, 8f, direction, false);
                    npc.AddBuff(BuffID.OnFire, 5 * 60);
                }
            }
        }
    }
}