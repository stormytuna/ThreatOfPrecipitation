using Terraria;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Items.Consumables.CytokineticSlime
{
    public class CytokineticSlime : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Aromatic and bitter!'");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.maxStack = 999;
            Item.alpha = 60;
            Item.value = Item.sellPrice(copper: 15);
        }
    }
}