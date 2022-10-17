using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using ThreatOfPrecipitation.Content.Projectiles.Weapons;

namespace ThreatOfPrecipitation.Content.Items.Weapons
{
    public class PiercingWind : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Call down 3 homing energy bolts");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 38;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 6);
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.damage = 40;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<PiercingWindProj>();
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item9;
            Item.crit = 14;
            Item.mana = 13;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = player.Center + new Vector2(5 * 16 * player.direction);
            position = player.Center - new Vector2(0f, 600f);
            Vector2 heading = target - position;
            heading.Normalize();
            heading *= velocity.Length();

            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = new Vector2(10 * 16 * player.direction, -30 * 16) * i;
                Projectile.NewProjectile(source, position + offset, heading, type, damage, knockback, player.whoAmI);
            }

            return false;
        }
    }
}