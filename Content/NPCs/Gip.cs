using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using System;

namespace ThreatOfPrecipitation.Content.NPCs
{
    public class Gip : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gip");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 30;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.damage = 15;
            NPC.defense = 5;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 40;
            NPC.value = 400f; // 4 silver

            AIType = NPCID.BlueSlime;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A small, sticky slime. It smells faintly of strawberries...")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && NPC.downedBoss1)
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0.2f;
            }

            return 0f;
        }
    }
}