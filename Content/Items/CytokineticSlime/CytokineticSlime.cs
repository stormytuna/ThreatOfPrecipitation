using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace ThreatOfPrecipitation.Content.Items.CytokineticSlime
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