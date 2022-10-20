using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Creative;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class FocusCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% increased damage to enemies that are close to you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FocusCrystalPlayer>().focusCrystal = true;
            player.GetModPlayer<FocusCrystalPlayer>().focusCrystalVisuals = !hideVisual;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<FocusCrystalPlayer>().focusCrystalVisuals = true;
        }
    }

    public class FocusCrystalGlobalProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Geode;

        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (Main.rand.NextBool(10))
            {
                int i = Item.NewItem(projectile.GetSource_Loot(), projectile.getRect(), ModContent.ItemType<FocusCrystal>());
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i, 1f);
                }
            }
        }
    }

    public class FocusCrystalPlayer : ModPlayer
    {
        public bool focusCrystal;
        public bool focusCrystalVisuals;

        public int focusCrystalCounter = 0;
        public int focusCrystalCounterMax = 80;

        private Asset<Texture2D> focusCrystalAuraTexture;
        private Asset<Texture2D> focusCrystalAuraFadeTexture;

        float focusCrystalRange = 16 * 16f;

        public override void ResetEffects()
        {
            focusCrystal = false;
            focusCrystalVisuals = false;
        }

        public override void PostUpdate()
        {
            if (focusCrystalVisuals)
                focusCrystalCounter++;
            else
                focusCrystalCounter--;

            focusCrystalCounter = (int)MathHelper.Clamp((float)focusCrystalCounter, 0f, focusCrystalCounterMax);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (focusCrystal)
                TryFocusCrystalDamageIncrease(target, ref damage);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (focusCrystal)
                TryFocusCrystalDamageIncrease(target, ref damage);
        }

        public void TryFocusCrystalDamageIncrease(NPC target, ref int damage) => damage = Vector2.DistanceSquared(target.Center, Player.Center) < focusCrystalRange * focusCrystalRange ? (int)((float)damage * 1.2f) : damage;

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (focusCrystalCounter > 0)
            {
                TryGetTextures();

                // Draw circle
                Vector2 drawPosition = Player.Center - Main.screenPosition;
                drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y); // Prevents sprite from wiggling
                Rectangle rect = new Rectangle(0, 0, 513, 513);
                Vector2 origin = rect.Size() / 2f;
                float scale = stormytunaUtils.EaseOut(0.5f, 1.2f, (float)focusCrystalCounter / (float)focusCrystalCounterMax, 4);
                float easeValue = stormytunaUtils.EaseOut(0f, 1f, (float)focusCrystalCounter / (float)focusCrystalCounterMax, 4);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(focusCrystalAuraTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(focusCrystalAuraFadeTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);

                // Do dust
                float range = focusCrystalCounter >= focusCrystalCounterMax // Looks wacky, essentially gets the size of our current ring
                    ? focusCrystalRange
                    : focusCrystalRange * stormytunaUtils.EaseOut(0.5f, 1f, (float)focusCrystalCounter / (float)focusCrystalCounterMax, 5); 
                for (int i = 0; i < 20; i++)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Vector2 randomVector = new Vector2(range * 0.9f, 0f).RotatedByRandom(MathHelper.TwoPi);
                        Vector2 position = randomVector + Player.Center;
                        Vector2 velocity = -randomVector;
                        velocity.Normalize();
                        velocity *= Main.rand.NextFloat(5f, 10f);

                        Dust d = Dust.NewDustPerfect(position, 60, velocity, Main.rand.Next(120, 200), default, Main.rand.NextFloat(1f, 2f));
                        d.noGravity = true;
                        d.noLight = true;
                    }
                }
            }
        }

        public void TryGetTextures()
        {
            if (focusCrystalAuraTexture == null)
                focusCrystalAuraTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/FocusCrystal_Aura");
            if (focusCrystalAuraFadeTexture == null)
                focusCrystalAuraFadeTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/FocusCrystal_AuraFade");
        }

        public Color GetAuraDrawColor(float easeValue) => Color.Lerp(new Color(255, 0, 0, 0), new Color(255, 0, 0, 255), easeValue);
    }
}