using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Creative;
using Terraria.ModLoader.Utilities;

namespace ThreatOfPrecipitation.Content.Items.Banners
{
    // Helper class for creating banners
    public abstract class ModBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 10);
            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults()
        {
            Item.createTile = ModContent.TileType<ToPBanners>();
            Item.placeStyle = 0;
        }
    }
}