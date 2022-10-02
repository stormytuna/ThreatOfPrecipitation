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
using ThreatOfPrecipitation.Content.Items.CytokineticSlime;

namespace ThreatOfPrecipitation.Content.Items.CytokineticSlime
{
    public class CytokineticDynamite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A large explosion that will destroy most tiles\nSplits on impact");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 28;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(silver: 4);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<CytokineticDynamiteProj>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 0;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CytokineticSlime>())
                .AddIngredient(ItemID.Dynamite)
                .Register();
        }
    }
}