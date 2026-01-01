using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MyFirstAccessory.Projectiles
{
    public class MyMagicBolt : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = 16;       // 투사체 크기
            Projectile.height = 16;
            Projectile.friendly = true;  // 플레이어 편
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;   // 5초 후 소멸
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true; // 벽에 닿으면 터짐
        }

        public override void AI() {
            // 1. 투사체가 날아가는 방향으로 자동 회전
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // 2. 파편(Dust) 효과 넣기 (이게 그래픽의 핵심!)
            // 매 프레임마다 70% 확률로 파편 생성
            if (Main.rand.NextFloat() < 0.7f) {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,           // 파편 생성 위치
                    DustID.PurpleCrystalShard,   // 파편 종류 (보라색 보석 파편)
                    Projectile.velocity * -0.5f, // 날아가는 반대 방향으로 퍼짐
                    150,                         // 투명도
                    Color.Purple,                // 색상
                    1.2f                         // 크기
                );
                dust.noGravity = true;           // 중력 무시
                dust.velocity *= 0.5f;           // 파편이 너무 멀리 안 퍼지게
            }
        }

        // 벽에 부딪혀 터질 때 효과
        public override void OnKill(int timeLeft) {
            for (int i = 0; i < 10; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard);
            }
        }
    }
}