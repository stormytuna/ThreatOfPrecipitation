using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Buffs.PlayerDebuffs
{
    public class LifeDebt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Life Debt");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            var player = Main.LocalPlayer;
            int buffIndex = player.FindBuffIndex(Type);
            int buffTime = player.buffTime[buffIndex];
            float percentLost = (float)buffTime / (20f * 60f * 60f);
            percentLost = MathHelper.Lerp(0f, 0.5f, percentLost);
            int percentLostInt = (int)(percentLost * 100f);

            tip = $"{percentLostInt}% of maximum health lost!";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int buffTime = player.buffTime[buffIndex];
            float percentLost = (float)buffTime / (20f * 60f * 60f);
            percentLost = MathHelper.Lerp(1f, 0.5f, percentLost);
            player.statLifeMax2 = (int)((float)player.statLifeMax2 * percentLost);
        }
    }
}