using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThreatOfPrecipitation.Content.Items.Pets
{
    public class MagmaWormPet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Scale");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 18;
            Item.height = 20;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Master;
            Item.noMelee = true;
            Item.value = Item.buyPrice(gold: 25);
            Item.buffType = ModContent.BuffType<MagmaWormPetBuff>();
            Item.shoot = ModContent.ProjectileType<MagmaWormPetProj>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
    }

    public class MagmaWormPetProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
            Main.projPet[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Type] = 71;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 174;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            //AIType = ProjectileID.DestroyerPet;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // Keep the projetile from disappearing as long as the player isnt dead and has the pet buff
            if (!owner.dead && owner.HasBuff(ModContent.BuffType<MagmaWormPetBuff>()))
                Projectile.timeLeft = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = TextureAssets.Projectile[Type].Value;

            int num = 8;
            int num2 = 12;

            SpriteEffects effects = (Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Microsoft.Xna.Framework.Rectangle rectangle = value.Frame(1, Main.projFrames[Type]);
            Vector2 origin = rectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Microsoft.Xna.Framework.Color alpha = Projectile.GetAlpha(Lighting.GetColor(Projectile.Center.ToTileCoordinates()));
            Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.White * ((float)(int)Main.mouseTextColor / 255f);
            Vector2 value2 = Projectile.Center;
            int num3 = 1;
            int num4 = Main.projFrames[Type] - 1;
            for (int i = 1; i < num; i++)
            {
                int frameY = num3;
                if (i == num - 1)
                    frameY = num4;

                Microsoft.Xna.Framework.Rectangle value3 = value.Frame(1, Main.projFrames[Type], 0, frameY);
                Vector2 value4 = Projectile.oldPos[i * 10] + Projectile.Size / 2f;
                float num5 = (value2 - value4).ToRotation();
                value4 = value2 - new Vector2(num2, 0f).RotatedBy(num5, Vector2.Zero);
                num5 = (value2 - value4).ToRotation() + (float)Math.PI / 2f;
                Vector2 position2 = value4 - Main.screenPosition;
                SpriteEffects effects2 = (!(value4.X < value2.X)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                value2 = value4;
                Main.EntitySpriteDraw(value, position2, value3, Projectile.GetAlpha(Lighting.GetColor(value4.ToTileCoordinates())), num5, origin, Projectile.scale, effects2, 0);
            }

            Main.EntitySpriteDraw(value, position, rectangle, alpha, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }
    }

    public class MagmaWormPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Magma Worm");
            Description.SetDefault("Hot to the touch");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<MagmaWormPetProj>();

            // If the player is local, and there hasn't been a pet projectile spawned yet - spawn it.
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
            {
                var entitySource = player.GetSource_Buff(buffIndex);

                Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}