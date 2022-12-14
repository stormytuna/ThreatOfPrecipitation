using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;

namespace ThreatOfPrecipitation.Content.Currencies
{
    public class LunarCoinCurrency : CustomCurrencySingleCoin
    {
        public LunarCoinCurrency(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
        {
            this.CurrencyTextKey = CurrencyTextKey;
            CurrencyTextColor = new Color(110, 116, 242);
        }
    }
}