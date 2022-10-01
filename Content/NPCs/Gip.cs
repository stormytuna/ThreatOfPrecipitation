using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using ThreatOfPrecipitation.Content.Items.CytokineticSlime;

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
        }

        public override void AI()
        {
            NPC.dontTakeDamage = false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Draws eyes
            Texture2D texture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/NPCs/Gip_Glow").Value;
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

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A small, sticky slime. It smells faintly of strawberries...")
            });
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
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CytokineticSlime>(), 8));
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