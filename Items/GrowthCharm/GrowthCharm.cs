// Items/GrowthCharm.cs (동적 툴팁 기능 추가)

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic; // List를 사용하기 위해 필요합니다.
using Terraria.Localization; // TooltipLine을 위해 필요합니다.
using Microsoft.Xna.Framework; // Color를 사용하기 위해 필요합니다.
using MyFirstAccessory.Players; // [수정] AccessoryPlayer를 찾기 위해 반드시 필요합니다!

namespace MyFirstAccessory.Items
{
    public class GrowthCharm : ModItem
    {
        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        // [추가] 툴팁을 동적으로 수정하는 함수입니다.
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 현재 플레이어의 AccessoryPlayer 정보를 가져옵니다.
            var accessoryPlayer = Main.LocalPlayer.GetModPlayer<AccessoryPlayer>();
            
            // 만약 보너스가 0보다 크다면
            if (accessoryPlayer.growthBonus > 0)
            {
                // 새로운 툴팁 줄을 만듭니다.
                var line = new TooltipLine(Mod, "GrowthBonus", $"최대 체력 +{accessoryPlayer.growthBonus}")
                {
                    OverrideColor = Color.LawnGreen // 글자 색을 보기 좋게 초록색으로 설정
                };
                
                // 툴팁 목록에 새로운 줄을 추가합니다.
                tooltips.Add(line);
            }
        }
    }
}