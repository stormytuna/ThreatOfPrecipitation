using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ThreatOfPrecipitation.Content.NPCs
{
    public class AlphaConstruct : ModNPC
    {
        // Some stuff we use later on
        private enum State
        {
            Asleep, // No player is nearby - Create shield and close top
            Notice, // A player is nearby but out of range - Keep shield but open top
            Active // A player is nearby and in range - Drop shield and start shooting
        }

        private enum Frame
        {
            ClosedTop,
            OpenTop
        }

        public ref float AI_State => ref NPC.ai[0];
        public ref float AI_ShootTimer => ref NPC.ai[1];

        private float maxShootDelay = 60f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alpha Construct");
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 32;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.defense = 30;
            NPC.lifeMax = 300;
            NPC.value = Item.sellPrice(silver: 10);
            NPC.gfxOffY = -4f;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return AI_State == (float)State.Active;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return AI_State == (float)State.Active;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            knockback = 0f;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = 0f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSnow)
                return SpawnCondition.OverworldNightMonster.Chance * 0.5f;

            return 0f;
        }

        public override void AI()
        {
            switch (AI_State)
            {
                case (float)State.Asleep:
                    Sleep();
                    break;
                case (float)State.Notice:
                    Notice();
                    break;
                case (float)State.Active:
                    Active();
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (AI_State == (float)State.Asleep)
                NPC.frame.Y = 0;
            else
                NPC.frame.Y = frameHeight;
        }

        // Helpers
        private void CheckChangeAI(Player target)
        {
            if (NPC.HasValidTarget && target.Distance(NPC.Center) < 25f * 16f)
            {
                AI_State = (float)State.Active;
                AI_ShootTimer = maxShootDelay;
            }
            else if (NPC.HasValidTarget && target.Distance(NPC.Center) < 30f * 16f)
            {
                AI_State = (float)State.Notice;
            }
            else
            {
                AI_State = (float)State.Asleep;
            }
        }

        private void Sleep()
        {
            // TODO: Implement shield
            Main.NewText("Shielding!");

            // Try find a target
            NPC.TargetClosest(false);
            var target = Main.player[NPC.target];

            // Check if we should change AI
            CheckChangeAI(target);
        }

        private void Notice()
        {
            // Has the same behavious as sleep but our frame is different
            Sleep();
        }

        private void Active()
        {
            // TODO: Drop shield

            // Double check our target
            NPC.TargetClosest(false);
            var target = Main.player[NPC.target];

            // Decrement our shoot delay 
            AI_ShootTimer--;
            if (AI_ShootTimer <= 0f)
            {
                // TODO: Shoot
                Main.NewText("Shoot!");
            }

            // Check if we should change AI
            CheckChangeAI(target);
        }
    }
}