using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Systems;
using ThreatOfPrecipitation.Content.Buffs.PlayerDebuffs;
using ThreatOfPrecipitation.Content.Items.BossBags;
using ThreatOfPrecipitation.Content.Items.BossMasks;
using ThreatOfPrecipitation.Content.Items.Pets;
using ThreatOfPrecipitation.Content.Items.Placeable.Furniture;
using ThreatOfPrecipitation.Content.Items.Weapons;

namespace ThreatOfPrecipitation.Content.NPCs.MagmaWorm
{
    [AutoloadBossHead]
    internal class MagmaWormHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<MagmaWormBody>();
        public override int TailType => ModContent.NPCType<MagmaWormTail>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Worm");

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "ThreatOfPrecipitation/Content/NPCs/MagmaWorm/MagmaWorm_Bestiary",
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 10f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifier);

            NPCID.Sets.DebuffImmunitySets[Type] = new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            };

            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.width = 38;
            NPC.height = 38;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.damage = 70;
            NPC.defense = 0;
            NPC.lifeMax = 80000;
            NPC.HitSound = SoundID.NPCHit15;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.value = 120000f;
            NPC.scale = 1.25f;
            NPC.boss = true;
            NPC.netAlways = true;
            Music = MusicID.Boss2;
        }

        private int ballCooldown = 0;
        private int ballTimer = 0;
        private int ballStaggerTime = 10;

        public override void AI()
        {
            // Check for target if we dont have one
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            // Flee if target is dead or outside hell
            if (Main.player[NPC.target].dead || !Main.player[NPC.target].ZoneUnderworldHeight)
            {
                NPC.EncourageDespawn(10);
                return; // Guard clause here so we dont cause errors if NPC.target is < 0
            }

            bool collision = CheckCollision(false);

            // Add some lighting
            if (!collision)
            {
                Lighting.AddLight(NPC.position, TorchID.White);
            }

            // If we're pointing at our target, we should increase our top speed
            Player target = Main.player[NPC.target];
            float rotToTarget = (target.Center - NPC.Center).ToRotation();
            float curRot = NPC.velocity.ToRotation();
            float absDif = MathF.Abs(rotToTarget - curRot);
            if (absDif < MathHelper.ToRadians(15f))
            {
                MoveSpeed = 20f;
                Acceleration = 0.12f;
            }
            else 
            {
                MoveSpeed = 18f;
                Acceleration = 0.2f;
            }

            // Fire a few balls when it's near the player, has line of sight and off cooldown
            // Using this method to stagger spawns
            float range = 16f * 60f; // 50 tiles
            if (!collision && Vector2.DistanceSquared(NPC.Center, target.Center) <= range * range && Collision.CanHit(NPC, target) && ballCooldown < 0 && ballTimer < 0)
            {
                int numBalls = Main.rand.Next(2, 6);
                ballTimer = numBalls * ballStaggerTime;
                ballCooldown = GetFireballCooldown() + ballTimer;
                NPC.netUpdate = true;
            }

            // Check collision and line of sight here again in case wormy boy goes back into a tile
            if (ballTimer > 0 && ballTimer % ballStaggerTime == 0 && !collision) 
            {
                // Fire ball
                Vector2 velocity = target.Center - NPC.Center;
                velocity.Normalize();
                velocity *= Main.rand.NextFloat(6f, 9f);
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(40f));
                Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, velocity, ProjectileID.BallofFire, 25, 3f);
                proj.friendly = false;
                proj.hostile = true;
                proj.scale *= Main.rand.NextFloat(1.2f, 1.5f);
                proj.damage = (int)(proj.damage * proj.scale);
                proj.ignoreWater = true;
                proj.timeLeft = 5 * 60;
                proj.penetrate = 1;
                proj.netUpdate = true;

                SoundEngine.PlaySound(SoundID.Item20, NPC.position);
            } 

            ballCooldown--;
            ballTimer--;
        }

        private int GetFireballCooldown() => (int)(12 * 60 * MathHelper.Lerp(0.5f, 1f, NPC.life / NPC.lifeMax));

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedMagmaWorm, -1);
        }

        public override bool SpecialOnKill()
        {
            // Change position so we drop loot in the right location
            Vector2 position = NPC.position;
            Vector2 playerCenter = Main.player[NPC.target].Center;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && (Main.npc[i].type == ModContent.NPCType<MagmaWormHead>() || Main.npc[i].type == ModContent.NPCType<MagmaWormBody>() || Main.npc[i].type == ModContent.NPCType<MagmaWormTail>()))
                {
                    float distance = Vector2.Distance(Main.npc[i].Center, playerCenter);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        position = Main.npc[i].Center;
                    }
                }
            }

            Vector2 temp = NPC.position;
            NPC.position = position;
            NPC.NPCLoot();
            NPC.position = temp;

            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // BossBag - BossBag rule checks for expert mode
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MagmaWormBossBag>()));
            // Trophy
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MagmaWormTrophy_Item>()));
            // Relic
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MagmaWormRelic>()));
            // Pet
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MagmaWormPet>()));
            // Rest of drops are not expert, so we need a leading conditional rule
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            // Boss mask
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MagmaWormMask>(), 7));

            int[] options = new int[]
            {
                ModContent.ItemType<LaserGlaive>(),
                ModContent.ItemType<VulcanShotgun>(),
                ModContent.ItemType<PiercingWind>(),
                ModContent.ItemType<TeslaBeacon>()
            };

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, options));

            npcLoot.Add(notExpertRule);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            // Guard clause since gores arent loaded on the server
            if (Main.netMode == NetmodeID.Server)
                return;

            // Spawn gores
            if (NPC.life <= 0)
            {
                int gore1 = Mod.Find<ModGore>("MagmaWormHead_Gore1").Type;
                int gore2 = Mod.Find<ModGore>("MagmaWormHead_Gore2").Type;
                int gore3 = Mod.Find<ModGore>("MagmaWormHead_Gore3").Type;
                int gore4 = Mod.Find<ModGore>("MagmaWormHead_Gore4").Type;

                var source = NPC.GetSource_Death();

                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore1);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore2);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore3);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore4);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<MagmaGlazed>(), 3 * 60);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("A blind serpentine creature, the Magma Worm is incredibly conductive and propels itself by expelling ignited gas from its body")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ballCooldown);
            writer.Write(ballTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ballCooldown = reader.ReadInt32();
            ballTimer = reader.ReadInt32();
        }

        public override void Init()
        {
            MinSegmentLength = 40;
            MaxSegmentLength = 40;

            CommonWormInit(this);
        }

        internal static void CommonWormInit(Worm worm)
        {
            worm.MoveSpeed = 25f;
            worm.Acceleration = 0.2f;
        }
    }

    internal class MagmaWormBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Worm");

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifier);

            NPCID.Sets.DebuffImmunitySets[Type] = new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            };
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.width = 38;
            NPC.height = 38;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.damage = 55;
            NPC.defense = 30;
            NPC.lifeMax = 80000;
            NPC.HitSound = SoundID.NPCHit15;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.scale = 1.25f;
            NPC.dontCountMe = true;
        }

        public override void AI()
        {
            bool collision = CheckCollision(false);
            // Add some lighting
            if (!collision)
            {
                Lighting.AddLight(NPC.position, TorchID.White);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            // Guard clause since gores arent loaded on the server
            if (Main.netMode == NetmodeID.Server)
                return;

            // Spawn gores
            if (NPC.life <= 0)
            {
                int gore1 = Mod.Find<ModGore>("MagmaWormBody_Gore1").Type;
                int gore2 = Mod.Find<ModGore>("MagmaWormBody_Gore2").Type;
                int gore3 = Mod.Find<ModGore>("MagmaWormBody_Gore3").Type;
                int gore4 = Mod.Find<ModGore>("MagmaWormBody_Gore4").Type;

                var source = NPC.GetSource_Death();

                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore1);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore2);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore3);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore4);
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<MagmaGlazed>(), 3 * 60);
            }
        }

        public override void Init()
        {
            MagmaWormHead.CommonWormInit(this);
        }
    }

    internal class MagmaWormTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Worm");

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifier);

            NPCID.Sets.DebuffImmunitySets[Type] = new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            };
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.width = 38;
            NPC.height = 38;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.damage = 55;
            NPC.defense = 30;
            NPC.lifeMax = 80000;
            NPC.HitSound = SoundID.NPCHit15;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.scale = 1.25f;
            NPC.dontCountMe = true;
        }

        public override void AI()
        {
            bool collision = CheckCollision(false);
            // Add some lighting
            if (!collision)
            {
                Lighting.AddLight(NPC.position, TorchID.White);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            // Guard clause since gores arent loaded on the server
            if (Main.netMode == NetmodeID.Server)
                return;

            // Spawn gores
            if (NPC.life <= 0)
            {
                int gore1 = Mod.Find<ModGore>("MagmaWormTail_Gore1").Type;
                int gore2 = Mod.Find<ModGore>("MagmaWormTail_Gore2").Type;
                int gore3 = Mod.Find<ModGore>("MagmaWormTail_Gore3").Type;

                var source = NPC.GetSource_Death();

                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore1);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore2);
                }
                if (Main.rand.NextBool(2))
                {
                    Gore.NewGore(source, NPC.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), gore3);
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<MagmaGlazed>(), 3 * 60);
            }
        }

        public override void Init()
        {
            MagmaWormHead.CommonWormInit(this);
        }
    }
}