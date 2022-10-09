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
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameContent;
using ReLogic.Content;
using ThreatOfPrecipitation.Content.Items;
using ThreatOfPrecipitation.Content.Buffs.Lunar;

namespace ThreatOfPrecipitation.Content.NPCs
{
    [AutoloadHead]
    public class Newt : ModNPC
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Newt");
            Main.npcFrameCount[Type] = 25; ;
            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = 1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Truffle, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Hate);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 600;
            NPC.width = 20;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A Time Newt with an atrophied arm, the Newt owns a bazaar that exists between time. You may be able to purchase some of these wares with Lunar Coins")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            foreach (Player player in Main.player)
            {
                foreach (Item item in player.inventory)
                {
                    if (item.type == ModContent.ItemType<LunarCoin>() && item.stack > 5)
                        return true;
                }
            }

            return false;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return new NewtProfile();
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Newt"
            };
        }

        public override string GetChat()
        {
            return "amogus"; // TODO: Add chats
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            shop = true;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            // TODO: Add conditions for these to be sold

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<ShapedGlass_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<MercurialRachis_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<StoneFluxPauldron_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LightFluxPauldron_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<BrittleCrown_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Purity_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Transcendence_Item>());
            shop.item[nextSlot].shopCustomPrice = 2;
            shop.item[nextSlot].shopSpecialCurrency = ThreatOfPrecipitation.LunarCoinCurrencyID;
            nextSlot++;
        }

        public override bool CanGoToStatue(bool toKingStatue) => true; // Can go to either statue, Newt uses it/its in ror2

        // TODO: implement Newt attacks
    }

    public class NewtProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
                return ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/NPCs/Newt");

            if (npc.altTexture == 1)
                return ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/NPCs/Newt"); // TODO: Implement party texture

            return ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/NPCs/Newt");
        }

        public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("ThreatOfPrecipitation/Content/NPCs/Newt_Head");
    }
}