using System.Collections;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ThreatOfPrecipitation.Common.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool downedMagmaWorm = false;

        public override void OnWorldUnload()
        {
            downedMagmaWorm = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedMagmaWorm)
            {
                tag["downedMagmaWorm"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedMagmaWorm = tag.ContainsKey("downedMagmaWorm");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte(); // This can only hold 8 values, but it should be enough. If not, use multiple BitsByte vars
            flags[0] = downedMagmaWorm;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedMagmaWorm = flags[0];
        }
    }
}