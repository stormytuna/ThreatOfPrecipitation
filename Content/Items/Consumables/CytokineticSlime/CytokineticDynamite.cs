using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ThreatOfPrecipitation.Content.Projectiles.CytokineticSlime;

namespace ThreatOfPrecipitation.Content.Items.Consumables.CytokineticSlime
{
    public class CytokineticDynamite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A large explosion that will destroy most tiles");
            ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Type] = true;
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