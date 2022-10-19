using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.NPCs.MagmaWorm;
using Microsoft.Xna.Framework;

namespace ThreatOfPrecipitation.Content.Items.BossSummons
{
    public class LavaLarva : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons the Magma Worm\nBe careful not to use this outside it's regular habitat...");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 14;
            Item.maxStack = 20;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<MagmaWormHead>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = ModContent.NPCType<MagmaWormHead>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
                }
            }

            return true;
        }
    }
}