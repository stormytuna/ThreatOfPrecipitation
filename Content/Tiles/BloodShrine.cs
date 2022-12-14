using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ThreatOfPrecipitation.Common.Systems;
using ThreatOfPrecipitation.Content.Buffs.PlayerDebuffs;
using ThreatOfPrecipitation.Content.Items.Accessories;
using ThreatOfPrecipitation.Content.Items.Placeable;

namespace ThreatOfPrecipitation.Content.Tiles
{
    public class BloodShrine : ModTile
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
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 64, ModContent.ItemType<BloodShrine_Item>());
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void MouseOver(int i, int j)
        {
            var player = Main.LocalPlayer;

            // Guard clause
            if (ShrineSystem.Instance.IsShrineUsedUp(i, j))
                return;

            string mouseText = "50% [i:29]"; // This item is the life crystal, meant to imply it costs 50% of your max hp

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

            // If player is already recovering from a sacrifice shrine, play insufficient funds noise
            if (player.HasBuff(ModContent.BuffType<LifeDebt>()))
            {
                SoundEngine.PlaySound(new SoundStyle("ThreatOfPrecipitation/Assets/Sounds/InsufficientFunds"), new Vector2(i * 16, j * 16));
                return false;
            }

            // Apply debuff
            player.AddBuff(ModContent.BuffType<LifeDebt>(), 20 * 60 * 60);

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

            Vector2 position = new Vector2((i * 16) + 9f, (j * 16) - 27f);

            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(position, DustID.Blood);
                d.velocity *= Main.rand.NextFloat(2.5f, 3.5f);
                d.scale = Main.rand.NextFloat(1f, 1.5f);
            }

            #endregion

            SoundEngine.PlaySound(new SoundStyle("ThreatOfPrecipitation/Assets/Sounds/ShrineActivate"), new Vector2(i * 16, j * 16));

            return true;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            // Create some funky blood dust if this shrine is active
            if (!ShrineSystem.Instance.IsShrineUsedUp(i, j)) 
            {
                // Gets the right i and j
                Tile tile = Main.tile[i, j];
                if (tile.TileFrameX == 36)
                    i--;
                if (tile.TileFrameX == 0)
                    i++;
                if (tile.TileFrameY == 0)
                    j += 1;
                if (tile.TileFrameY == 36)
                    j--;
                if (tile.TileFrameY == 54)
                    j -= 2;

                if (Main.rand.NextBool(1))
                {
                    Vector2 dustPosition = new Vector2(i * 16f, j * 16f);
                    dustPosition += new Vector2(9f, 9f);
                    Vector2 offset = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 3f;
                    dustPosition += offset;
                    Vector2 velocity = -offset * 0.5f;
                    Dust d = Dust.NewDustPerfect(dustPosition, DustID.Blood, velocity, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                    d.noGravity = true;
                }
            }
        }
    }
}