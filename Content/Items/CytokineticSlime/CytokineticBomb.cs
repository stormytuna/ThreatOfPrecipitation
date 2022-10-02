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
    public class CytokineticBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A small explosion that will destroy most tiles\nSplits on impact");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(silver: 3, copper: 60);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<CytokineticBombProj>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CytokineticSlime>())
                .AddIngredient(ItemID.Bomb)
                .Register();
        }
    }
}