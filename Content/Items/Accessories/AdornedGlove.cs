using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace ThreatOfPrecipitation.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn)] // TODO: Sprite this
    public class AdornedGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases melee knockback\n" +
                "12% increased melee speed\n" +
                "Enables autoswing for melee weapons\n" +
                "Increases the size of melee weapons\n" +
                "20% increased damage to enemies that are close to you");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;
            Item.value = Item.sellPrice(gold: 4, silver: 50);
            Item.rare = ItemRarityID.LightPurple; // 1 above pink used by Power Glove
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FocusCrystalPlayer>().focusCrystal = true;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PowerGlove)
                .AddIngredient(ModContent.ItemType<FocusCrystal>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}