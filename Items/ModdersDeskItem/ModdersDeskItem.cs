// Items/ModdersDeskItem.cs

using Terraria.ID;
using Terraria.ModLoader;
using MyFirstAccessory.Tiles; // 우리가 만들 타일 파일의 위치를 알려줌

namespace MyFirstAccessory.Items.ModdersDeskItem
{
    public class ModdersDeskItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 이 줄은 아이템이 자동으로 '가구' 카테고리로 분류되도록 도와줍니다.
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
        }

        public override void SetDefaults()
        {
            // 아이템 자체의 크기 (인벤토리나 월드에 떨어져 있을 때)
            Item.width = 28;
            Item.height = 18;

            // [가장 중요!] 이 아이템을 사용하면 어떤 '타일'이 설치될지 지정합니다.
            // ModContent.TileType<ModdersDesk>()는 Tiles 폴더의 ModdersDesk 클래스를 찾아 연결해줍니다.
            Item.createTile = ModContent.TileType<ModdersDesk>();

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing; // 설치 모션
            Item.consumable = true; // 설치하면 아이템이 소모됨
            Item.maxStack = 99;
            Item.value = 150;
        }

        // 아주 간단한 제작법 (나무 10개로 작업대에서)
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}