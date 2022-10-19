using Terraria;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Common.GlobalNPCs
{
    public class LunarBuffGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool mercurialRachisAura;

        public override void ResetEffects(NPC npc)
        {
            mercurialRachisAura = false;
        }

        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (mercurialRachisAura)
                damage = (int)(damage * 1.2f);
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (mercurialRachisAura)
                damage = (int)(damage * 1.2f);
        }
    }
}