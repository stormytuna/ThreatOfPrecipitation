using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.GlobalNPCs;

namespace ThreatOfPrecipitation.Common.GlobalProjectiles
{
    public class LunarBuffGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool isNPCProjectile = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is NPC npc)
            {
                isNPCProjectile = true;

                if (npc.GetGlobalNPC<LunarBuffGlobalNPC>().mercurialRachisAura)
                    projectile.damage = (int)(projectile.damage * 1.2f);
            }
        }
    }
}