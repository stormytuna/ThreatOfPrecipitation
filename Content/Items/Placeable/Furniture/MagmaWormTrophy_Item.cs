using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Tiles.Furniture;

namespace ThreatOfPrecipitation.Content.Items.Placeable.Furniture
{
    public class MagmaWormTrophy_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Worm Trophy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MagmaWormTrophy>());

            Item.width = 32;
            Item.width = 32;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
        }
    }
}