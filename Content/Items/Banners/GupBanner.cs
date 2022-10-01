using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;

namespace ThreatOfPrecipitation.Content.Items.Banners
{
    public class GupBanner : ModBanner
    {
        public override void SafeSetDefaults()
        {
            Item.createTile = ModContent.TileType<ToPBanners>();
            Item.placeStyle = 2;
        }
    }
}