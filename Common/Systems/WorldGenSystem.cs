using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using ThreatOfPrecipitation.Content.Items.Placeable.Banners;
using ThreatOfPrecipitation.Content.Tiles;

namespace ThreatOfPrecipitation.Common.Systems
{
    public class WorldGenSystem : ModSystem
    {
        int[] dungeonBricks = new int[] { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick };

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int dungeonIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
            if (dungeonIndex != -1)
            {
                tasks.Insert(dungeonIndex + 1, new PassLegacy("Warbanners", Warbanners));
            }

            int heartCrystalsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Life Crystals"));
            if (heartCrystalsIndex != -1)
            {
                tasks.Insert(heartCrystalsIndex + 1, new PassLegacy("Shrines", Shrines));
            }
        }

        private void Warbanners(GenerationProgress progress, GameConfiguration _)
        {
            progress.Message = "Placing Warbanners in the Dungeon";

            int numBanners = 4;
            if (Main.maxTilesX > 4200)
                numBanners += 2;
            if (Main.maxTilesX > 6400)
                numBanners += 2;

            int numPlaced = 0;
            // Code adapted from vanilla placing banners in dungeon
            while (numPlaced < numBanners)
            {
                // Get our initial x, y coordinates
                int x = WorldGen.genRand.Next(WorldGen.dMinX, WorldGen.dMaxX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                
                // Reroll x, y coordinates until we find an empty tile in the dungeon
                while (!Main.wallDungeon[Main.tile[x, y].WallType] || Main.tile[x, y].HasTile)
                {
                    x = WorldGen.genRand.Next(WorldGen.dMinX, WorldGen.dMaxX);
                    y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                }

                // Move upwards until we hit a solid tile
                while (!WorldGen.SolidTile(x, y) && y > 10)
                    y--;

                // Check that we have room for our banner
                y++;
                if (!Main.wallDungeon[Main.tile[x, y].WallType] || Main.tile[x, y - 1].TileType == 48 || Main.tile[x, y].HasTile || Main.tile[x, y + 1].HasTile || Main.tile[x, y + 2].HasTile || Main.tile[x, y + 3].HasTile)
                    continue;

                // Check that the surrounding tiles are dungeon tiles
                bool flag = true;
                for (int j = x - 1; j <= x + 1; j++)
                {
                    for (int k = y; k <= y + 3; k++)
                    {
                        if (Main.tile[j, k].HasTile && (Main.tile[j, k].TileType == 10 || Main.tile[j, k].TileType == 11 || Main.tile[j, k].TileType == 91))
                            flag = false;
                    }
                }

                // Actually place our tile
                if (flag)
                {
                    WorldGen.PlaceTile(x, y, ModContent.TileType<ToPBanners>(), mute: true, forced: false, -1, 3);
                    numPlaced++;
                }
            }
        }

        private void Shrines(GenerationProgress progress, GameConfiguration _)
        {
            progress.Message = "Placing shrines";

            for (int i = 0; i < Main.maxTilesX * Main.maxTilesY * 2E-05; i++)
            {
                float completion = (float)((double)i / Main.maxTilesX * Main.maxTilesY * 1E-06);
                progress.Set(completion);

                bool finished = false;
                int numTries = 0;
                while (!finished)
                {
                    bool placed = false;

                    // Try placing a shrine
                    int x = WorldGen.genRand.Next(40, Main.maxTilesX - 40);
                    int y = WorldGen.genRand.Next((int)(Main.worldSurface * 2.0 + Main.rockLayer) / 3, Main.maxTilesY - 300);
                    for (int k = y; k < Main.maxTilesY; k++)
                    {
                        if (Main.tile[x, k].HasTile && Main.tileSolid[Main.tile[x, k].TileType])
                        {
                            int num = k - 1;

                            // Guard clauses
                            // If tile is wet, don't place here
                            if (Main.tile[x, num - 1].LiquidAmount > 0 || Main.tile[x - 1, num - 1].LiquidAmount > 0 || Main.tile[x + 1, num - 1].LiquidAmount > 0)
                                break;

                            // If tile doesn't have enough empty tiles, don't place here
                            if (!WorldGen.EmptyTileCheck(x - 1, x + 1, num - 3, num))
                                break;

                            // If tile is in the dungeon, don't place here
                            if (Main.wallDungeon[Main.tile[x, num].WallType])
                                break;

                            WorldGen.Place3x4(x, num, (ushort)ModContent.TileType<ChanceShrine>(), 0);
                            ShrineSystem.Instance.RegisterShrinePlacedByWorld(x, num);
                            placed = true;
                        }
                    }

                    if (placed)
                        finished = true;

                    else
                    {
                        numTries++;
                        if (numTries > 10000)
                            finished = true;
                    }
                }
            }
        }
    }
}