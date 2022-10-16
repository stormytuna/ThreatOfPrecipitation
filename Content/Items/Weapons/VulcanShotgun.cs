using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Items.Weapons
{
    public class VulcanShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Charge up for more damage and accuracy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 16;
            Item.rare = ItemRarityID.Pink;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 19;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<VulcanShotgunProj>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "Speed");
            line.Text = "Slow speed";
        }

        // Stops us from using the item if we dont have ammo
        public override bool CanUseItem(Player player)
        {
            return player.HasAmmo(Item) && !player.noItems && !player.CCed && cooldown <= 0;
        }

        // Stops the player consuming ammo while channelling
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !Main.mouseLeft;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Creates our VulcanShotgunProj if needed
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VulcanShotgunProj>()] < 1)
            {
                Projectile.NewProjectile(Item.GetSource_Misc("-1"), player.Center, Vector2.Zero, ModContent.ProjectileType<VulcanShotgunProj>(), Item.damage, Item.knockBack, player.whoAmI);
            }

            // Return false so we dont shoot a bullet
            return false;
        }

        private int cooldown = 0;
        private bool oldMouseLeft = false;

        public override void UpdateInventory(Player player)
        {
            // Probably extremely overengineered, this should give the player a 12 frame cooldown after they stop firing with this weapon
            // Lets us have a really short useAnimation and useTime while preserving a cooldown
            if (Item == player.inventory[player.selectedItem] && !Main.mouseLeft && oldMouseLeft && cooldown < 0)
            {
                cooldown = 35;
            }

            oldMouseLeft = Main.mouseLeft;
            cooldown--;
        }
    }

    public class VulcanShotgunProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vulcan Shotgun Projectile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }

        // This is a held projectile that just fires our shotgun since i couldnt find a way to do it in the item
        public override bool? CanHitNPC(NPC target) => false;

        // Helpers
        private ref float AI_FrameCount => ref Projectile.ai[0];
        private ref float AI_DoneFullChargeVisual => ref Projectile.ai[1];
        private int MaxChargeTime => 60;
        private float ChargeMultiplier => AI_FrameCount > MaxChargeTime ? 1f : AI_FrameCount / 60f; // Makes sure we only ever get 0-60 for frame count
        private List<Dust> dustList = new List<Dust>();

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // Check if the player has stopped channelling and if we should shoot
            if (!owner.ItemAnimationActive)
            {
                // Actually shooting
                // Add to our damage here so multiplier stays additive
                float damageBonus = MathHelper.Lerp(0f, 2f, ChargeMultiplier);
                owner.GetDamage(DamageClass.Ranged) += damageBonus;
                // Set some more stuff
                owner.PickAmmo(owner.HeldItem, out int projToShoot, out float speed, out int damage, out float knockback, out int usedAmmoItemID);
                IEntitySource source = Projectile.GetSource_ItemUse_WithPotentialAmmo(owner.HeldItem, usedAmmoItemID);
                float spread = MathHelper.ToRadians(MathHelper.Lerp(70f, 5f, ChargeMultiplier));
                // Shoot projectiles
                for (int i = 0; i < 8; i++)
                {
                    Vector2 velocity = Main.MouseWorld - Projectile.Center;
                    velocity.Normalize();
                    velocity *= speed;
                    velocity = velocity.RotatedByRandom(spread);
                    Projectile.NewProjectile(source, owner.Center, velocity, projToShoot, damage, knockback, Projectile.owner);
                }
                // Sound
                SoundEngine.PlaySound(SoundID.Item36, Projectile.position);
                // Kill projectile so this code isnt called again
                Projectile.Kill();
                return;
            }

            // Set location in the center of the players hand
            Vector2 location = Main.OffsetsPlayerOnhand[owner.bodyFrame.Y / 56] * 2f;
            if (owner.direction != 1)
                location.X = owner.bodyFrame.Width - location.X;
            if (owner.gravDir != 1f)
                location.Y = owner.bodyFrame.Height - location.Y;
            location -= new Vector2(owner.bodyFrame.Width - owner.width, owner.bodyFrame.Height - 42) / 2f;
            Projectile.Center = owner.RotatedRelativePoint(owner.MountedCenter - new Vector2(20f, 42f) / 2f + location, reverseRotation: false, addGfxOffY: false);

            // Visuals
            // Clear old dusts
            for (int i = 0; i < dustList.Count; i++)
            {
                dustList[i].active = false;
            }
            // Get some variables we use later on
            float visualArcLength = MathHelper.ToRadians(MathHelper.Lerp(70f, 5f, ChargeMultiplier));
            float halfArcLength = visualArcLength / 2f;
            float visualArcRadius = 5f * 16f;
            Vector2 vectorToMouse = Main.MouseWorld - Projectile.Center;
            vectorToMouse.Normalize();
            vectorToMouse *= visualArcRadius;
            int numDust = (int)(visualArcLength / MathHelper.ToRadians(0.4f));
            // Make actual dust 
            for (int i = 0; i < numDust; i++)
            {
                // Gets the ith % of our total arc
                float lerpAmount = (float)i / (float)(numDust - 1);
                float lerpedRot = MathHelper.Lerp(0f, visualArcLength, lerpAmount);
                lerpedRot -= halfArcLength;
                Vector2 position = owner.Center + vectorToMouse.RotatedBy(lerpedRot);
                Dust dust = Dust.NewDustPerfect(position, 60);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                int halfPoint = numDust / 2;
                float scaleLerp =  (float)(i - halfPoint) / (float)halfPoint;
                float absScaleLerp = Math.Abs(scaleLerp);
                dust.scale = MathHelper.Lerp(0.6f, 1f, absScaleLerp);
                dust.alpha = (int)MathHelper.Lerp(245, 200, absScaleLerp);
                dustList.Add(dust);
            }

            AI_FrameCount++;
        }
    }

    public class VulcanShotgunPlayerLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> vulcanShotgunTexture;

        public override bool IsHeadLayer => false;

        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HandOnAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<VulcanShotgun>();

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            if (vulcanShotgunTexture == null)
            {
                vulcanShotgunTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Items/Weapons/VulcanShotgun_OnPlayer");
            }

            Vector2 position = drawInfo.Center - Main.screenPosition + new Vector2(-5f * drawPlayer.direction, -3f);
            position = new Vector2((int)position.X, (int)position.Y);

            drawInfo.DrawDataCache.Add(new DrawData(
                vulcanShotgunTexture.Value,
                position,
                drawInfo.compFrontArmFrame,
                drawInfo.colorArmorBody,
                drawPlayer.bodyRotation + drawInfo.compositeFrontArmRotation,
                drawInfo.bodyVect + new Vector2(-5 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f),
                1f,
                drawInfo.playerEffect,
                0
            ));
        }
    }
}