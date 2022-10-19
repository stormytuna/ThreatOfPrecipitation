using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class FocusCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% increased damage to enemies that are close to you");
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
        }
    }

    public class FocusCrystalPlayer : ModPlayer
    {
        public bool focusCrystal;
        public bool oldFocusCrystal = false;

        public int focusCrystalCounter = 0;
        public int focusCrystalCounterMax = 80;
        public int focusCrystalDecayCounter = 0;

        private Asset<Texture2D> focusCrystalAuraTexture;
        private Asset<Texture2D> focusCrystalAuraFadeTexture;

        public override void ResetEffects()
        {
            focusCrystal = false;
        }

        public override void PostUpdate()
        {
            // TODO: make this code better, simplyify to only using 1 counter
            if (focusCrystal)
            {
                // Keeping track of stuff
                if (!oldFocusCrystal)
                    focusCrystalCounter = 0;

                focusCrystalCounter++;
                if (focusCrystalCounter >= focusCrystalCounterMax)
                    focusCrystalCounter = focusCrystalCounterMax;

                // Dust - Move into its own function thats called after the draw to make sure its only made when drawing is allowed
                float range = focusCrystalCounter >= focusCrystalCounterMax
                    ? focusCrystalRange
                    : focusCrystalRange * stormytunaUtils.EaseOut(0.5f, 1f, (float)focusCrystalCounter / (float)focusCrystalCounterMax, 5); // Essentially gets the size of our current ring
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
            else
            {
                focusCrystalCounter = 0;
            }

            // Try set decay counter
            if (!focusCrystal && oldFocusCrystal)
            {
                focusCrystalDecayCounter = focusCrystalCounterMax;
            }

            focusCrystalDecayCounter--;
            oldFocusCrystal = focusCrystal;
        }

        float focusCrystalRange = 16 * 16f;

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            TryFocusCrystalDamageIncrease(target, ref damage);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            TryFocusCrystalDamageIncrease(target, ref damage);
        }

        public void TryFocusCrystalDamageIncrease(NPC target, ref int damage) => damage = Vector2.DistanceSquared(target.Center, Player.Center) < focusCrystalRange * focusCrystalRange ? (int)((float)damage * 1.2f) : damage;

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            // TODO: Make this draw code better, make it draw in the social slot, make it not draw if the eye is empty in regular slot
            if (focusCrystal)
            {
                var drawPlayer = drawInfo.drawPlayer;

                TryGetTextures();

                Vector2 drawPosition = drawPlayer.Center - Main.screenPosition;
                drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y);
                Rectangle rect = new Rectangle(0, 0, 513, 513);
                Vector2 origin = rect.Size() / 2f;
                float scale = stormytunaUtils.EaseOut(0.5f, 1.2f, (float)focusCrystalCounter / (float)focusCrystalCounterMax, 5);
                float easeValue = stormytunaUtils.EaseOut(0f, 1f, (float)focusCrystalCounter / (float)focusCrystalCounterMax, 5);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(focusCrystalAuraTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(focusCrystalAuraFadeTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
            }
            else if (!focusCrystal && focusCrystalDecayCounter > 0)
            {
                var drawPlayer = drawInfo.drawPlayer;

                TryGetTextures();

                Vector2 drawPosition = drawPlayer.Center - Main.screenPosition;
                drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y);
                Rectangle rect = new Rectangle(0, 0, 513, 513);
                Vector2 origin = rect.Size() / 2f;
                float scale = stormytunaUtils.EaseIn(1.2f, 0.5f, (float)focusCrystalDecayCounter / (float)focusCrystalCounterMax, 5);
                float easeValue = stormytunaUtils.EaseIn(1f, 0f, (float)focusCrystalDecayCounter / (float)focusCrystalCounterMax, 5);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(focusCrystalAuraTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(focusCrystalAuraFadeTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
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