using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Items.BossSummons;

namespace ThreatOfPrecipitation.Common.GlobalItems
{
    public class MechanicalWormGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MechanicalWorm;
        }

        int lavaWetCounter = 0;

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.lavaWet)
            {
                lavaWetCounter++;

                // Visuals
                if (lavaWetCounter % 15 == 0)
                {
                    int numDust = 3;
                    if (lavaWetCounter > 40)
                        numDust += 2;
                    if (lavaWetCounter > 70)
                        numDust += 4;
                    for (int i = 0; i < numDust; i++)
                    {
                        Dust.NewDustDirect(item.position, item.width, item.height, DustID.Lava, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3), 0, default, Main.rand.NextFloat(0.8f, 1f));
                    }
                }
            }
            else
            {
                lavaWetCounter = 0;
            }

            if (lavaWetCounter >= 105)
            {
                int magmaWorm = Item.NewItem(item.GetSource_Misc("-1"), item.position, ModContent.ItemType<LavaDippedWorm>(), item.stack);
                Main.item[magmaWorm].noGrabDelay = 100;
                SoundEngine.PlaySound(SoundID.Item29, item.position);
                item.active = false;
            }
        }
    }
}