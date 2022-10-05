using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.GlobalNPCs;
using ThreatOfPrecipitation.Content.Buffs.Lunar;

namespace ThreatOfPrecipitation.Common.GlobalProjectiles
{
    public class LunarBuffGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is NPC npc && npc.GetGlobalNPC<LunarBuffGlobalNPC>().mercurialRachisAura)
            {
                projectile.damage = (int)(projectile.damage * 1.2f);
            }
        }
    }
}