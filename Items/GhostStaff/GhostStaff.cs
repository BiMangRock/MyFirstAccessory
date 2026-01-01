// Items/GhostStaff.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MyFirstAccessory.Buffs;
using MyFirstAccessory.Projectiles.GhostMinion;
using Microsoft.Xna.Framework;

namespace MyFirstAccessory.Items.GhostStaff
{
    public class GhostStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<GhostBuff>();
            Item.shoot = ModContent.ProjectileType<GhostMinion>();
        }

        // [최종 수정] 원본 Shoot 함수와 매개변수를 완벽하게 일치시켰습니다.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            projectile.originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            // CreateRecipe()
            //     .AddIngredient(ItemID.ShadowScale, 10)
            //     .AddTile(TileID.DemonAltar)
            //     .Register();
                        CreateRecipe()
                .AddIngredient(ItemID.Wood, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}