using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ThreatOfPrecipitation
{
    public static class stormytunaUtils
    {

        /// <summary>Gets a list of NPCs within the range of that position</summary>
        /// <param name="position">The position</param>
        /// <param name="range">The range measured in units, 1 tile is 16 units</param>
        /// <returns>A list of NPCs within range of the position</returns>
        public static List<NPC> GetNearbyNPCs(Vector2 position, float range)
        {
            List<NPC> npcs = new List<NPC>();
            float rangeSquared = range * range;

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                float distanceSquared = Vector2.DistanceSquared(position, npc.position);
                if (distanceSquared <= rangeSquared)
                {
                    npcs.Add(npc);
                }
            }

            return npcs;
        }
    }
}