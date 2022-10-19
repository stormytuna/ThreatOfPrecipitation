using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Tiles.Furniture;

namespace ThreatOfPrecipitation.Content.Items.Placeable.Furniture
{
    public class MagmaWormRelic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Worm Relic");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BossRelic>(), 0);

            Item.width = 38;
            Item.width = 50;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(gold: 5);
        }
    }
}