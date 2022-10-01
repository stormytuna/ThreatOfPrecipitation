using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using ThreatOfPrecipitation.Content.Items.CytokineticSlime;

namespace ThreatOfPrecipitation.Content.NPCs
{
    public class Gup : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gup");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.RainbowSlime];
        }

        public override void SetDefaults()
        {
            NPC.damage = 90;
            NPC.defense = 30;
            NPC.lifeMax = 500;
            NPC.value = 3000f; // 3 silver
            NPC.width = 74;
            NPC.height = 56;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 40;
            AIType = NPCID.BlueSlime;
        }

        public override void OnKill()
        {
            int slime = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Geep>(), Target: NPC.target);
            Main.npc[slime].velocity = new Vector2(-3f, -3f);
            Main.npc[slime].dontTakeDamage = true;
            slime = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Geep>(), Target: NPC.target);
            Main.npc[slime].velocity = new Vector2(3f, -3f);
            Main.npc[slime].dontTakeDamage = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Draws eyes
            Texture2D texture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/NPCs/Gup_Glow").Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            int frameHeight = texture.Height / Main.npcFrameCount[Type];
            int startY = NPC.frame.Y;
            Rectangle sourceRect = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRect.Size() / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.direction == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(texture, position, sourceRect, drawColor, 0f, origin, 1f, spriteEffects, 0f);
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
                NPC.frameCounter += 1.0;
            }
            if (num2 == 4)
            {
                NPC.frameCounter += 1.0;
            }
            if (NPC.frameCounter >= 8.0)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A humungous slime capable of undergoing mitosis to evade death. It tastes oddly bitter despite smelling of strawberries.")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CytokineticSlime>(), 8));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && NPC.downedPlantBoss)
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0.2f;
            }

            return 0f;
        }
    }
}