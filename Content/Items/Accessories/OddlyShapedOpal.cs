using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using System;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    public class OddlyShapedOpal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The first hit you take every 5 seconds deals half damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(silver: 40);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<OddlyShapedOpalPlayer>().oddlyShapedOpal = true;
            player.GetModPlayer<OddlyShapedOpalPlayer>().oddlyShapedOpalVisuals = !hideVisual;
        }
    }

    public class OddlyShapedOpalGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Tim;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OddlyShapedOpal>()));
        }
    }

    public class OddlyShapedOpalPlayer : ModPlayer
    {
        public bool oddlyShapedOpal;
        public bool oddlyShapedOpalVisuals;

        public int oddlyShapedOpalCounter = 0;
        public int oddlyShapedOpalCounterMax = 5 * 60;

        public override void ResetEffects()
        {
            oddlyShapedOpal = false;
            oddlyShapedOpalVisuals = false;
        }

        private Vector2 DustLocation()
        {
            Vector2 position = Player.Center;
            if (Player.direction == -1)
                position += new Vector2(-8f, 0f);
            if (Player.gravDir == -1)
                position += new Vector2(0f, -13f);
            return position;
        }

        public override void PostUpdate()
        {
            oddlyShapedOpalCounter--;

            // Dust
            if (oddlyShapedOpalCounter < 0)
            {
                if (Main.rand.NextBool(5) && Player.mount.Type == MountID.None)
                {
                    Dust d = Dust.NewDustDirect(DustLocation(), 6, 8, DustID.Clentaminator_Cyan, 0, 0, 200, default, Main.rand.NextFloat(0.2f, 0.4f));
                    d.velocity *= 0.2f;
                    d.noLight = true;
                }
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            TryOpalDamageReduction(ref damage);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            TryOpalDamageReduction(ref damage);
        }

        private void TryOpalDamageReduction(ref int damage)
        {
            // Try reduce damage 
            if (oddlyShapedOpalCounter < 0)
            {
                damage /= 2;

                // TODO: fancy dust effect, dust being absorbed by the necklace?
                if (Player.mount.Type == MountID.None)
                {
                    for (int i = 0; i < 12f; i++)
                    {
                        Vector2 position = DustLocation();
                        Vector2 offset = new Vector2(0f, 5f);
                        offset = offset.RotatedBy(i / 12f * MathHelper.TwoPi);
                        position += offset;
                        Vector2 velocity = -offset;
                        velocity.Normalize();

                        Dust.NewDustPerfect(position, DustID.Clentaminator_Cyan, velocity, 200, default, 0.3f);
                    }
                }
            }

            // Reset our counter
            oddlyShapedOpalCounter = oddlyShapedOpalCounterMax;
        }
    }

    public class OddlyShapedOpalDrawLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> opalNecklaceTexture;
        private Asset<Texture2D> opalShineTexture;
        private Asset<Texture2D> opalAloneTexture;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<OddlyShapedOpalPlayer>().oddlyShapedOpalVisuals;

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (opalNecklaceTexture == null)
                opalNecklaceTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/OddlyShapedOpal_Neck");
            if (opalShineTexture == null)
                opalShineTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/OddlyShapedOpal_Shine");
            if (opalAloneTexture == null)
                opalAloneTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/OddlyShapedOpal_Opal");

            // Set some stuff used in drawing
            var drawPlayer = drawInfo.drawPlayer;
            var modPlayer = drawPlayer.GetModPlayer<OddlyShapedOpalPlayer>();
            const float TwoPi = MathHelper.TwoPi;
            Vector2 drawPosition = drawInfo.Center - Main.screenPosition;
            drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y);
            Rectangle rect = drawPlayer.bodyFrame;
            Color drawColor = drawInfo.colorArmorBody;
            float rotation = drawPlayer.bodyRotation;
            Vector2 origin = drawInfo.bodyVect;
            float scale = 1f;
            SpriteEffects effects = drawInfo.playerEffect;

            // Draw our necklace
            drawInfo.DrawDataCache.Add(new(opalNecklaceTexture.Value, drawPosition, rect, drawColor, rotation, origin, scale, effects, 0));

            // Funky stuff for when we have full charge
            if (modPlayer.oddlyShapedOpalCounter < 0)
            {
                // Draw our shine
                float shineScale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 1.1f;
                Color effectColor = drawColor;
                effectColor.A = 0;
                effectColor = effectColor * 0.1f * shineScale;
                for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
                {
                    drawInfo.DrawDataCache.Add(new(opalShineTexture.Value, drawPosition + (TwoPi * num5).ToRotationVector2() * 2f, rect, effectColor, 0f, origin, 1f, effects, 0));
                }
            }
        }
    }
}