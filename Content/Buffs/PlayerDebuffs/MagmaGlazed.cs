using Terraria;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Buffs.PlayerDebuffs
{
    public class MagmaGlazed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Glazed");
            Description.SetDefault("Losing life");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MagmaGlazedDebuffPlayer>().magmaGlazed = true;
        }
    }

    public class MagmaGlazedDebuffPlayer : ModPlayer
    {
        public bool magmaGlazed;

        public override void ResetEffects()
        {
            magmaGlazed = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (magmaGlazed)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 10;
            }
        }
    }
}