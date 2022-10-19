using Terraria;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Items.Placeable.Banners
{
    public class GipBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = ModContent.TileType<ToPBanners>();
            Item.placeStyle = 0;
        }
    }
}