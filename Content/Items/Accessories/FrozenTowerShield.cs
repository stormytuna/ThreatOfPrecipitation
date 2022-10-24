using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using System.Collections.Generic;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class FrozenTowerShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Grants immunity to knockback\n" +
                "Puts a shell around the owner when below 50% life that reduces damage by 25%\n" +
                "Absorbs 25% of damage done to players on your team when above 25% life\n" +
                "Allies in range gain 8% increased damage and 20% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 40;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.defense = 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PaladinsShield)
                .AddIngredient(ItemID.FrozenTurtleShell)
                .AddIngredient(ModContent.ItemType<Warbanner>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.FrozenShield)
                .AddIngredient(ModContent.ItemType<Warbanner>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TowerShield>())
                .AddIngredient(ItemID.FrozenTurtleShell)
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
            // Add our frozen shield buff here because funy
            if ((float)player.statLife <= (float)player.statLifeMax2 * 0.5)
                player.AddBuff(BuffID.IceBarrier, 5);

            player.noKnockback = true;
            player.GetModPlayer<WarbannerPlayer>().warbanner = true;
            player.GetModPlayer<WarbannerPlayer>().warbannerVisuals = !hideVisual;
        }
    }
}