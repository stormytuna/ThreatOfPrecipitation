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
        /// <param name="position">The position, should be the center of the search and usually the center of another entity</param>
        /// <param name="range">The range measured in units, 1 tile is 16 units</param>
        /// <param name="excludedNPCs">The whoAmI fields of any NPCs that are excluded from the search</param>
        /// <returns>A list of NPCs within range of the position</returns>
        public static List<NPC> GetNearbyEnemies(Vector2 position, float range, List<int> excludedNPCs = null)
        {
            List<NPC> npcs = new List<NPC>();
            float rangeSquared = range * range;
            if (excludedNPCs == null)
                excludedNPCs = new List<int>();

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.CountsAsACritter || npc.friendly || excludedNPCs.Contains(npc.whoAmI))
                {
                    continue;
                }

                float distanceSquared = Vector2.DistanceSquared(position, npc.Center);
                if (distanceSquared <= rangeSquared)
                {
                    npcs.Add(npc);
                }
            }

            return npcs;
        }

        /// <summary>Gets the closest hostile NPC within the range of that position</summary>
        /// <param name="position">The position, should be the center of the search and usually the center of another entity</param>
        /// <param name="range">The range measured in units, 1 tile is 16 units</param>
        /// <param name="careAboutLineOfSight">Whether the function should check Collision.CanHit</param>
        /// <param name="excludedNPCs">The whoAmI fields of any NPCs that are excluded from the search</param>
        /// <returns>Returns the closest NPC. Returns null if no NPC is found</returns>
        public static NPC GetClosestEnemy(Vector2 position, float range, bool careAboutLineOfSight, List<int> excludedNPCs = null)
        {
            NPC closestNPC = null;
            float rangeSquared = range * range;
            if (excludedNPCs == null)
                excludedNPCs = new List<int>();

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.CountsAsACritter || npc.friendly || excludedNPCs.Contains(npc.whoAmI))
                { 
                    continue;
                }

                float distanceSquared = Vector2.DistanceSquared(position, npc.Center);
                bool canSee = careAboutLineOfSight ? Collision.CanHit(position, 1, 1, npc.position, npc.width, npc.height) : true;
                if (distanceSquared < rangeSquared && canSee)
                {
                    closestNPC = npc;
                    rangeSquared = distanceSquared;
                }
            }

            return closestNPC;
        }

        /// <summary>Homing via rotating a projectiles velocity towards its target.\nThis overload searches for the closest enemy</summary>
        /// <param name="currentVelocity">The projectiles current velocity</param>
        /// <param name="startPosition">The position, should be the center of the projectile</param>
        /// <param name="range">The range measured in units, 1 tile is 16 units</param>
        /// <param name="careAboutLineOfSight">Whether the function should check Collision.CanHit</param>
        /// <param name="rotationMax">The max amount of degrees the velocity can be rotated, make sure to use degrees as this function coverts it to radians</param>
        /// <param name="excludeNPCs">The whoAmI fields of any NPCs that are excluded from the search</param>
        /// <returns>Returns velocity rotated towards the found NPC. Returns the original velocity if no NPC is found</returns>
        public static Vector2 RotateVelocityHoming(Vector2 currentVelocity, Vector2 startPosition, float range, bool careAboutLineOfSight, float rotationMax, List<int> excludeNPCs = null)
        {
            if (excludeNPCs == null)
                excludeNPCs = new List<int>();

            NPC closestNPC = GetClosestEnemy(startPosition, range, careAboutLineOfSight, excludeNPCs);

            if (closestNPC == null)
                return currentVelocity;

            return RotateVelocityHoming(currentVelocity, startPosition, closestNPC.Center, rotationMax);
        }

        /// <summary>Homing via rotating a projectiles velocity towards its target.</summary>
        /// <param name="currentVelocity">The projectiles current velocity</param>
        /// <param name="startPosition">The start position, should be the center of the projectile</param>
        /// <param name="targetPosition">The target position, should be the center of the target</param>
        /// <param name="rotationMax">The max amount of degrees the velocity can be rotated, make sure to use degrees as this function coverts it to radians</param>
        /// <returns>Returns velocity rotated towards the given position</returns>
        public static Vector2 RotateVelocityHoming(Vector2 currentVelocity, Vector2 startPosition, Vector2 targetPosition, float rotationMax)
        {
            rotationMax = MathHelper.ToRadians(rotationMax);
            float rotTarget = Utils.ToRotation(targetPosition - startPosition);
            float rotCurrent = Utils.ToRotation(currentVelocity);
            return Utils.RotatedBy(currentVelocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCurrent, rotTarget, rotationMax)) - Utils.ToRotation(currentVelocity)));
        }

        /// <summary>Checks within a rotation is within the range of another rotation</summary>
        /// <param name="rotation">The start rotation, measured in radians</param>
        /// <param name="rotationTestAgainst">The rotation to test against, measured in radians</param>
        /// <param name="range">The range around rotation to test if rotationTestAgainst is within, measured in radians</param>
        /// <returns>Returns true if rotationTestAgainst is within range of rotation. Returns false otherwise</returns>
        public static bool RotationIsWithinRange(float rotation, float rotationTestAgainst, float range)
        {
            float absDif = MathF.Abs(rotation - rotationTestAgainst);
            return absDif < range;
        }
    }
}