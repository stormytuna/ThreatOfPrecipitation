using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Common.Players;
using ThreatOfPrecipitation.Content.Rarities;

namespace ThreatOfPrecipitation.Content.Buffs.Lunar
{
    public class BrittleCrown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/5FCDE4:Brittle Crown]");
            Description.SetDefault("Hitting enemies will sometimes drop extra coins [c/E3242B:but being hit will sometimes make you lose coins]");
            Main.persistentBuff[Type] = true;
            //Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LunarBuffPlayer>().brittleCrown = true;
        }
    }

    public class BrittleCrown_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brittle Crown");
            Tooltip.SetDefault("Applies the [c/5FCDE4:Brittle Crown] lunar buff\nHitting enemies will sometimes drop extra coins [c/E3242B:but being hit will sometimes make you lose coins]");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<LunarRarity>();
        }
    }

    public class BrittleCrown_CoinProjectile : ModProjectile
    {
        private ref float AI_CoinType => ref Projectile.ai[0];
        private ref float AI_NumCoins => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brittle Crown Coin");
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 16;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            AI_CoinType = 0;
            if (Main.rand.NextBool(10))
                AI_CoinType = 1;
            if (Main.rand.NextBool(100))
                AI_CoinType = 2;

            AI_NumCoins = Main.rand.Next(1, 11);

            Projectile.frame = Main.rand.Next(0, Main.projFrames[Type]);

            Projectile.netUpdate = true;
        }

        public override void AI()
        {
            // Frames
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame > Main.projFrames[Type])
                {
                    Projectile.frame = 0;
                }
            }

            // Movement towards owner
            // Rotate towards owner
            Player owner = Main.player[Main.myPlayer];
            float rotTarget = Utils.ToRotation(owner.Center - Projectile.Center);
            float rotCur = Utils.ToRotation(Projectile.velocity);
            float rotMax = MathHelper.ToRadians(3f);
            Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCur, rotTarget, rotMax)) - Utils.ToRotation(Projectile.velocity)));
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            // Speed up slowly
            if (Math.Abs(rotTarget - rotCur) < MathHelper.ToRadians(25f))
            {
                Projectile.velocity *= 1.08f;
                float magnitude = MathHelper.Min(Projectile.velocity.Length(), 15f);
                Projectile.velocity.Normalize();
                Projectile.velocity *= magnitude;
            }

            // If we're near owner, kill proj and spawn a coin
            if (Vector2.Distance(owner.Center, Projectile.Center) < 10f)
            {
                int coinType = (int)AI_CoinType + 71; // This drops the right coin since 71-74 are all coins
                Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), coinType, (int)AI_NumCoins);
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D copperTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Buffs/Lunar/Coin_0").Value;
            Texture2D silverTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Buffs/Lunar/Coin_1").Value;
            Texture2D goldTexture = ModContent.Request<Texture2D>("ThreatOfPrecipitation/Content/Buffs/Lunar/Coin_2").Value;
            Texture2D textureToUse = copperTexture;
            if (AI_CoinType == 1f)
                textureToUse = silverTexture;
            if (AI_CoinType == 2f)
                textureToUse = goldTexture;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            int frameHeight = textureToUse.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRect = new Rectangle(0, startY, textureToUse.Width, frameHeight);

            Vector2 origin = sourceRect.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.spriteBatch.Draw(
                textureToUse,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                spriteEffects,
                0);

            return false;
        }
    }
}