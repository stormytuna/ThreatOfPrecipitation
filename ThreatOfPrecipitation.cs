using Terraria.GameContent.UI;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Currencies;
using ThreatOfPrecipitation.Content.Items;
using ThreatOfPrecipitation.Content.NPCs;

namespace ThreatOfPrecipitation
{
	public class ThreatOfPrecipitation : Mod
	{
		public static int LunarCoinCurrencyID;
        public override void Load()
        {
            LunarCoinCurrencyID = CustomCurrencyManager.RegisterCurrency(new LunarCoinCurrency(ModContent.ItemType<LunarCoin>(), 999L, "Mods.ThreatOfPrecipitation.Currencies.LunarCoinCurrency"));
        }

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("census", out Mod census))
            {
                census.Call("TownNPCCondition", ModContent.NPCType<Newt>(), "Have 5 Lunar Coins in your inventory");
            }
        }
    }
}