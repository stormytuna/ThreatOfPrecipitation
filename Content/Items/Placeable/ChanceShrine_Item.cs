using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Tiles;

namespace ThreatOfPrecipitation.Content.Items.Placeable
{
    public class ChanceShrine_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chance Shrine");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<ChanceShrine>(), 0);

            Item.width = 48;
            Item.height = 60;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 1);
        }
    }
}