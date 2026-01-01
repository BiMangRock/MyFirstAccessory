// Items/PredictionStaff.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MyFirstAccessory.Projectiles.YellowPixel; // YellowPixel을 사용하기 위해 추가

namespace MyFirstAccessory.Items.PredictionStaff
{
    // 이것은 GhostStaff와 아무 상관이 없는, 완전히 새로운 아이템입니다.
    public class PredictionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("예측의 지팡이");
            // Tooltip.SetDefault("적의 1초 전 위치를 향해 발사합니다.");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;                     // 데미지
            Item.DamageType = DamageClass.Magic;  // 데미지 타입은 '마법'
            Item.mana = 0;                        // 마나 소모량
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;                    // 사용 속도 (작을수록 빠름)
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot; // 발사하는 모션
            Item.noMelee = true;                  // 휘두를 때 데미지 없음
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;       // 발사 소리 (마법 구체 소리)
            Item.autoReuse = true;                // 자동 발사 기능

            // 이 지팡이가 발사할 발사체를 YellowPixel로 지정
            Item.shoot = ModContent.ProjectileType<YellowPixel>();
            Item.shootSpeed = 16f;                // 발사체 속도
        }
        // PredictionStaff.cs 파일 안에 있는 기존 Shoot 함수를 지우고 이걸로 덮어쓰세요.

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 1. 마우스 위치에서 가장 가까운 적을 찾습니다.
            NPC target = null;
            float maxDetectRadius = 800f; // 이 거리 안에 있는 적만 탐색
            float minDistance = maxDetectRadius;

            // 현재 월드에 있는 모든 NPC를 검사
            foreach (NPC npc in Main.npc)
            {
                // 공격할 수 있는 적인지 확인
                if (npc.CanBeChasedBy())
                {
                    // 마우스 커서와 적 사이의 거리를 계산
                    float distance = Vector2.Distance(Main.MouseWorld, npc.Center);

                    // 현재까지 찾은 가장 가까운 적보다 더 가깝다면, 이 적을 새로운 목표로 설정
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = npc;
                    }
                }
            }

            // 2. 만약 위 과정에서 적을 찾았다면, 발사 방향을 그 적의 '현재' 위치로 수정합니다.
            if (target != null)
            {
                // 예측 로직을 완전히 제거하고, 적의 현재 중앙 위치를 목표로 삼습니다.
                Vector2 direction = Vector2.Normalize(target.Center - player.Center);

                // 계산된 방향으로 발사체의 속도를 새로 설정합니다.
                velocity = direction * Item.shootSpeed;
            }
            // (만약 주변에 적을 찾지 못했다면, velocity는 원래대로 마우스 방향을 유지합니다)

            // 3. 최종적으로 계산된 속도로 발사체를 생성합니다.
            Projectile.NewProjectile(source, player.Center, velocity, type, damage, knockback, player.whoAmI);

            // Terraria의 기본 발사 로직을 실행하지 않도록 false를 반환합니다.
            return false;
        }


        // 제작법 추가
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}