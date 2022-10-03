using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ThreatOfPrecipitation.Content.Projectiles.CytokineticSlime;

namespace ThreatOfPrecipitation.Content.Items.CytokineticSlime
{
    public class CytokineticGlowstick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Works when wet");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 4);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.holdStyle = 1;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<CytokineticGlowstickProj>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CytokineticSlime>())
                .AddIngredient(ItemID.Glowstick, 5)
                .Register();
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.X -= 2f * player.direction;
        }

        public override void HoldItem(Player player)
        {
            Lighting.AddLight(player.Center, new Vector3(0.76f, 0.28f, 0f) * 2f);
        }
    }
}