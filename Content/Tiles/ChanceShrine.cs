using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ThreatOfPrecipitation.Common.Systems;
using ThreatOfPrecipitation.Content.Items.Accessories;
using ThreatOfPrecipitation.Content.Items.Placeable;

namespace ThreatOfPrecipitation.Content.Tiles
{
    public class ChanceShrine : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1000;
            Main.tileFrameImportant[Type] = true;
            Main.tileOreFinderPriority[Type] = 500;
            Main.tileSpelunker[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            DustType = DustID.Stone;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Shrine");
            AddMapEntry(new Color(144, 148, 144), name);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) => ShrineSystem.Instance.IsShrineUsedUp(i, j);

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 64, ModContent.ItemType<ChanceShrine_Item>());
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void MouseOver(int i, int j)
        {
            var player = Main.LocalPlayer;

            // Guard clause
            if (ShrineSystem.Instance.IsShrineUsedUp(i, j))
                return;

            int shrineCost = ShrineSystem.Instance.GetShrineCost(i, j);
            string mouseText = stormytunaUtils.CoinValueToString(shrineCost, true, true);

            player.cursorItemIconID = -1;
            player.cursorItemIconText = mouseText;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override bool RightClick(int i, int j)
        {
            var player = Main.LocalPlayer;

            // Guard clause
            int shrineCost = ShrineSystem.Instance.GetShrineCost(i, j);
            if (shrineCost == -1 || ShrineSystem.Instance.IsShrineUsedUp(i, j))
                return false;

            // If player doesnt have enough money to buy it
            if (!player.CanBuyItem(shrineCost))
            {
                SoundEngine.PlaySound(new SoundStyle("ThreatOfPrecipitation/Assets/Sounds/InsufficientFunds"), new Vector2(i * 16, j * 16));
                return false;
            }

            if (!ShrineSystem.Instance.IsShrineUsedUp(i, j))
            {
                if (Main.rand.NextBool(2))
                {
                    // Success 
                    int[] itemTypes = new int[]
                    {
                        ModContent.ItemType<FocusCrystal>(),
                        ModContent.ItemType<Gasoline>(),
                        ModContent.ItemType<Medkit>(),
                        ModContent.ItemType<OddlyShapedOpal>(),
                        ModContent.ItemType<Warbanner>()
                    };

                    int item = Item.NewItem(new EntitySource_TileInteraction(player, i, j), i * 16, j * 16, 16, 16, itemTypes[Main.rand.Next(0, 5)]);
                    Main.item[item].noGrabDelay = 100;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                    }

                    ShrineSystem.Instance.SetShrineAsUsedUp(i, j);

                    #region Visuals

                    Vector2 position = new Vector2((i * 16) + 8, (j * 16) + 8);

                    for (int k = 0; k < 30; k++)
                    {
                        Dust d = Dust.NewDustPerfect(position, DustID.GreenTorch);
                        d.velocity *= Main.rand.NextFloat(2.5f, 3.5f);
                        d.scale = Main.rand.NextFloat(1f, 1.5f);
                    }

                    #endregion
                }
                else
                {
                    ShrineSystem.Instance.IncreaseShrineTries(i, j);

                    #region Visuals

                    Vector2 position = new Vector2((i * 16) + 8, (j * 16) + 8);

                    for (int k = 0; k < 30; k++)
                    {
                        Dust d = Dust.NewDustPerfect(position, DustID.RedTorch);
                        d.velocity *= Main.rand.NextFloat(2.5f, 3.5f);
                        d.scale = Main.rand.NextFloat(1f, 1.5f);
                    }

                    #endregion
                }

                SoundEngine.PlaySound(new SoundStyle("ThreatOfPrecipitation/Assets/Sounds/ShrineActivate"), new Vector2(i * 16, j * 16));
                player.BuyItem(shrineCost);

                return true;
            }

            return false;
        }
    }
}