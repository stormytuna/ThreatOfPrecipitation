using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    //[AutoloadEquip(EquipType.Waist)] // TODO: implement this
    public class Medkit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("After taking damage, if you don't take damage for 8 seconds, heal back 60% of that damage you took");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MedkitPlayer>().medkit = true;
        }
    }

    public class MedkitGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Nurse;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Medkit>()));
        }
    }

    public class MedkitPlayer : ModPlayer
    {
        public bool medkit;

        private int medkitHeal = -1;
        private int medkitCounter = 0;

        public override void ResetEffects()
        {
            medkit = false;
        }

        public override void PostUpdate()
        {
            if (medkitCounter <= 0 && medkitHeal > 0)
            {
                Player.Heal(medkitHeal);
                medkitHeal = -1;
            }

            medkitCounter--;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            TryCreateMedkitHeal(damage);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            TryCreateMedkitHeal(damage);
        }

        private void TryCreateMedkitHeal(int damage)
        {
            medkitCounter = 7 * 60;
            medkitHeal = (int)((float)damage * 0.6f);
        }
    }
}