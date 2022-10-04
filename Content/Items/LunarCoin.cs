using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Items
{
    public class LunarCoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Currency for trading with the Newt");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 1);
        }
    }

    public class LunarCoinGlobalNPC : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunarCoin>(), 1000));
        }
    }
}