using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ThreatOfPrecipitation.Common.Systems
{
    public class ShrineSystem : ModSystem
    {
        public static ShrineSystem Instance; // tMod sets up singletons automatically

        private Dictionary<Tuple<int, int>, int> shrineDict;

        public void RegisterShrinePlacedByWorld(int i, int j)
        {
            if (!shrineDict.ContainsKey(new(i, j)))
                shrineDict.Add(new(i, j), 0);
        }

        private void GetShrineCenter(ref int i, ref int j)
        {
            // Gets the right i and j so it lines up with our dict
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 36)
                i--;
            if (tile.TileFrameX == 0)
                i++;
            if (tile.TileFrameY == 0)
                j += 3;
            if (tile.TileFrameY == 18)
                j += 2;
            if (tile.TileFrameY == 36)
                j++;
        }

        public int GetShrineCost(int i, int j)
        {
            GetShrineCenter(ref i, ref j);
            if (shrineDict.ContainsKey(new(i, j)))
                return (int)(10000f * 10f * MathF.Pow(1.15f, shrineDict[new(i, j)]));

            return -1;
        }

        public bool IsShrineUsedUp(int i, int j)
        {
            GetShrineCenter(ref i, ref j);
            if (shrineDict.ContainsKey(new(i, j)))
                return shrineDict[new(i, j)] == -1;
            else
                return true;
        }

        public void IncreaseShrineTries(int i, int j)
        {
            GetShrineCenter(ref i, ref j);
            if (shrineDict.ContainsKey(new(i, j)))
                shrineDict[new(i, j)]++;
        }

        public void SetShrineAsUsedUp(int i, int j)
        {
            GetShrineCenter(ref i, ref j);
            if (shrineDict.ContainsKey(new(i, j)))
                shrineDict[new(i, j)] = -1;
        }

        public override void Load()
        {
            Instance = this;
            shrineDict = new();
        }

        public override void Unload()
        {
            Instance = null;
            shrineDict = null;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var list = tag.GetList<TagCompound>("shrineDict");

            shrineDict = new();
            foreach (var item in list)
            {
                shrineDict.Add(new(item.GetInt("key"), item.GetInt("key1")), item.GetInt("value"));
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (shrineDict == null)
                shrineDict = new();

            var list = new List<TagCompound>();
            foreach (var kvp in shrineDict)
            {
                list.Add(new TagCompound()
                {
                    { "key", kvp.Key.Item1 },
                    { "key1", kvp.Key.Item2 },
                    { "value", kvp.Value }
                });
            }

            tag["shrineDict"] = list;
        }
    }
}