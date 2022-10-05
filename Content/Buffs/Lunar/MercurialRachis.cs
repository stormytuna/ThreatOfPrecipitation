using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.GlobalNPCs;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class MercurialRachis : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Mercurial Rachis]");
            Description.SetDefault("All enemies and allies in range have 20% increased damage");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().mercurialRachis = true;

            float range = 50 * 16; // 50 tiles
            float rangeSquared = range * range; // Using Vector2.DistanceSquared to save on performance

            foreach (Player pl in Main.player)
            {
                if (pl.GetModPlayer<LunarBuffPlayer>().mercurialRachis)
                    continue;
                
                if (Vector2.DistanceSquared(player.Center, pl.Center) < rangeSquared)
                    pl.AddBuff(ModContent.BuffType<MercurialRachis_Aura>(), 2);
            }

            foreach (NPC npc in Main.npc)
            {
                if (Vector2.DistanceSquared(player.Center, npc.Center) < rangeSquared)
                    npc.AddBuff(ModContent.BuffType<MercurialRachis_Aura>(), 2);
            }
        }
    }

    public class MercurialRachis_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercurial Rachis");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Mercurial Rachis] lunar buff\nAll enemies and allies in range have 20% increased damage");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }

    public class MercurialRachis_Aura : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Mercurial Rachis Aura]");
            Description.SetDefault("20% increased damage");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().mercurialRachisAura = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<LunarBuffGlobalNPC>().mercurialRachisAura = true;
        }
    }
}