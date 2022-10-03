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
    public class CytokineticGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A small explosion that will not destroy tiles\nSplits in two when it goes off");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(copper: 20);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 5.5f;
            Item.shoot = ModContent.ProjectileType<CytokineticGrenadeProj>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 12;
            Item.knockBack = 3f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<CytokineticSlime>())
                .AddIngredient(ItemID.Grenade, 2)
                .Register();
        }
    }
}