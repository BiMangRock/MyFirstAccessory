using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MyFirstAccessory.Projectiles.MyMagicBolt; // 이 주소를 미리 알고 있어라!
namespace MyFirstAccessory.Items.MyMagicStaff

{
    public class MyMagicStaff : ModItem
    {
        public override void SetDefaults() {
            Item.damage = 25;           // 공격력
            Item.DamageType = DamageClass.Magic; // 마법 데미지
            Item.mana = 0;              // 소모 마나
            Item.width = 40;            // 이미지 가로
            Item.height = 40;           // 이미지 세로
            Item.useTime = 25;          // 공격 속도
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot; // 지팡이처럼 쏘는 동작
            Item.noMelee = true;        // 지팡이 자체에는 공격 판정 없음
            Item.knockBack = 5;         // 밀쳐내기
            Item.value = Item.buyPrice(silver: 10); // 가격
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item20; // 발사 소리
            Item.autoReuse = true;      // 자동 발사 여부

            // 어떤 투사체를 쏠 것인가?
            Item.shoot = ModContent.ProjectileType<MyMagicBolt>();
            Item.shootSpeed = 10f;      // 투사체 속도
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 1) // 나무 1개 제작법
                .Register();
        }
    }
}