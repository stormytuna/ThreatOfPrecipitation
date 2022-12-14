using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using ThreatOfPrecipitation.Content.NPCs.GupFamily;
using Microsoft.Xna.Framework.Graphics;
using ThreatOfPrecipitation.Content.Items.Accessories;

namespace ThreatOfPrecipitation.Content.Items.Placeable.Banners
{
    public class ToPBanners : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override bool CreateDust(int i, int j, ref int type) => false;

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int itemType;

            switch (frameX / 18)
            {
				default:
					return;
				case 0:
					itemType = ModContent.ItemType<GipBanner>();
					break;
				case 1:
					itemType = ModContent.ItemType<GeepBanner>();
					break;
				case 2:
					itemType = ModContent.ItemType<GupBanner>();
					break;
				case 3:
					itemType = ModContent.ItemType<Warbanner>();
					break;
            }

			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, itemType);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
			if (closer)
            {
				Tile tile = Main.tile[i, j];
				int type;

				switch (tile.TileFrameX / 18)
                {
					default:
						return;
					case 0:
						type = ModContent.NPCType<Gip>();
						break;
					case 1:
						type = ModContent.NPCType<Geep>();
						break;
					case 2:
						type = ModContent.NPCType<Gup>();
						break;
				}

				Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<Gip>()] = true;
				Main.SceneMetrics.hasBanner = true;
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
			if (i % 2 == 1)
            {
				spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}