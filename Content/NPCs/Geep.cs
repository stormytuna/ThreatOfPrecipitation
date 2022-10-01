using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;

namespace ThreatOfPrecipitation.Content.NPCs
{
    public class Geep : ModNPC
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geep");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            NPC.damage = 55;
            NPC.defense = 20;
            NPC.lifeMax = 160;
            NPC.value = 400f; // 4 silver
            NPC.width = 44;
            NPC.height = 38;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 40;
            AIType = NPCID.BlueSlime;
        }

        public override void OnKill()
        {
            int slime = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Gip>(), Target: NPC.target);
            Main.npc[slime].velocity = new Vector2(-3f, -3f);
            Main.npc[slime].dontTakeDamage = true;
            slime = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Gip>(), Target: NPC.target);
            Main.npc[slime].velocity = new Vector2(3f, -3f);
            Main.npc[slime].dontTakeDamage = true;
        }

        public override void AI()
        {
            NPC.dontTakeDamage = false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int num2 = 0;
            if (NPC.aiAction == 0)
            {
                num2 = (NPC.velocity.Y < 0f) ? 2 : ((NPC.velocity.Y > 0f) ? 3 : ((NPC.velocity.X != 0f) ? 1 : 0));
            }
            else if (NPC.aiAction == 1)
            {
                num2 = 4;
            }
            if (num2 > 0)
            {
                NPC.frameCounter++;
            }
            if (num2 > 4)
            {
                NPC.frameCounter++;
            }
            if (NPC.frameCounter >= 8)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y > frameHeight)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A large slime made of a unique type of gel that can undergo mitosis to save its life")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && Main.hardMode)
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0.2f;
            }

            return 0f;
        }
    }
}