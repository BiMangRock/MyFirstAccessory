// Items/MightyRing.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyFirstAccessory.Items.MightyRing
{
    public class MightyRing : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true; // 이것도 액세서리입니다.
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;
        }

        // [효과] 장착 시 모든 공격력 10% 증가
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.10f;
        }

        // [제작법] 철 주괴 5개로 작업대에서 제작
        public override void AddRecipes()
        {
 
                CreateRecipe()
                .AddIngredient(ItemID.Wood, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}