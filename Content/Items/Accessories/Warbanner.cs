using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ThreatOfPrecipitation.Content.Projectiles;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    // [AutoloadEquip(EquipType.Back)] // TODO: Implement this
    public class Warbanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allies in range (including you) gain 8% increased damage and 25% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 58;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WarbannerPlayer>().warbanner = true;
            player.GetModPlayer<WarbannerPlayer>().warbannerVisuals = !hideVisual;
        }
    }

    public class WarbannerPlayer : ModPlayer
    {
        public bool warbanner;
        public bool warbannerVisuals;

        public int warbannerCounter = 0;
        public int warbannerCounterMax = 80;

        private float warbannerRange = 60f * 16f;

        private Asset<Texture2D> warbannerAuraTexture;
        private Asset<Texture2D> warbannerAuraFadeTexture;

        public override void ResetEffects()
        {
            warbanner = false;
            warbannerVisuals = false;
        }

        public override void PostUpdateEquips()
        {
            if (warbanner)
            {
                List<Player> nearbyPlayers = stormytunaUtils.GetNearbyPlayers(Player.Center, warbannerRange, false, Player.team);

                foreach (Player player in nearbyPlayers)
                {
                    player.AddBuff(ModContent.BuffType<WarbannerBuff>(), 5 * 60); // Lasts for 5 seconds after you leave the aura
                }
            }
        }

        public override void PostUpdate()
        {
            // Counter stuff
            if (warbannerVisuals && Player.active && !Player.dead)
                warbannerCounter++;
            else
                warbannerCounter--;

            warbannerCounter = (int)MathHelper.Clamp((float)warbannerCounter, 0f, warbannerCounterMax);

            if (warbannerCounter > 0)
            {
                // Do dust
                float range = warbannerCounter >= warbannerCounterMax // Looks wacky, essentially gets the size of our current ring
                    ? warbannerRange
                    : warbannerRange * stormytunaUtils.EaseOut(0.5f, 1f, (float)warbannerCounter / (float)warbannerCounterMax, 5);
                for (int i = 0; i < 20; i++)
                {
                    Vector2 randomVector = new Vector2(range, 0f).RotatedByRandom(MathHelper.TwoPi);
                    Vector2 position = randomVector + Player.Center;
                    Vector2 velocity = -randomVector;
                    velocity.Normalize();
                    velocity *= Main.rand.NextFloat(10f, 20f);

                    Dust d = Dust.NewDustPerfect(position, 64, velocity, Main.rand.Next(120, 200), default, Main.rand.NextFloat(1f, 2f));
                    d.noGravity = true;
                    d.noLight = true;
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            var player = drawInfo.drawPlayer;
            var modPlayer = player.GetModPlayer<WarbannerPlayer>();

            if (modPlayer.warbannerCounter > 0)
            {
                TryGetTextures();

                // Draw circle
                Vector2 drawPosition = player.Center - Main.screenPosition;
                drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y); // Prevents sprite from wiggling
                Rectangle rect = new Rectangle(0, 0, warbannerAuraTexture.Width(), warbannerAuraTexture.Height());
                Vector2 origin = rect.Size() / 2f;
                float scale = stormytunaUtils.EaseOut(0.3f, 1f, (float)modPlayer.warbannerCounter / (float)modPlayer.warbannerCounterMax, 4);
                float easeValue = stormytunaUtils.EaseOut(0f, 1f, (float)modPlayer.warbannerCounter / (float)modPlayer.warbannerCounterMax, 4);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
                Main.spriteBatch.Draw(warbannerAuraTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue, Lighting.GetColor(player.Center.ToTileCoordinates())), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(warbannerAuraFadeTexture.Value, drawPosition, rect, GetAuraDrawColor(easeValue, Lighting.GetColor(player.Center.ToTileCoordinates())), 0f, origin, scale, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
            }
        }

        public void TryGetTextures()
        {
            if (warbannerAuraTexture == null)
                warbannerAuraTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/Warbanner_Aura");
            if (warbannerAuraFadeTexture == null)
                warbannerAuraFadeTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Accessories/Warbanner_AuraFade");
        }

        public Color GetAuraDrawColor(float easeValue, Color lightColor)
        {
            Color color = Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 69, 0, 20), easeValue);
            lightColor.G *= 69 / 255;
            lightColor.B = 0;
            return Color.Lerp(color, lightColor, 0.5f);
        }
    }

    public class WarbannerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inspired");
            Description.SetDefault("8% increased damage and 25% increased movement speed");
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.moveSpeed += 0.25f;
        }
    }
}