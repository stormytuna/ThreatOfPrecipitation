using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using System.Collections.Generic;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class HeroTowerShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Grants immunity to knockback\n" +
                "Absorbs 25% of damage done to players on your team when above 25% life\n" +
                "Enemies are more likely to target you\n" +
                "Allies in range gain 8% increased damage and 20% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 44;
            Item.value = Item.sellPrice(gold: 11);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.defense = 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PaladinsShield)
                .AddIngredient(ItemID.FleshKnuckles)
                .AddIngredient(ModContent.ItemType<Warbanner>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.HeroShield)
                .AddIngredient(ModContent.ItemType<Warbanner>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TowerShield>())
                .AddIngredient(ItemID.FleshKnuckles)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Add our paladin buff here because funy
            if ((float)player.statLife > (float)player.statLifeMax2 * 0.25f)
            {
                player.hasPaladinShield = true;
                if (player.whoAmI != Main.myPlayer && player.miscCounter % 10 == 0)
                {
                    List<Player> nearbyPlayers = stormytunaUtils.GetNearbyPlayers(player.Center, 50f * 16f, false, player.team);
                    foreach (Player p in nearbyPlayers)
                        p.AddBuff(BuffID.PaladinsShield, 3 * 60);
                }
            }

            player.aggro += 400;
            player.noKnockback = true;
            player.GetModPlayer<WarbannerPlayer>().warbanner = true;
            player.GetModPlayer<WarbannerPlayer>().warbannerVisuals = !hideVisual;
        }
    }
}