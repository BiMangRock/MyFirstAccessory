// Projectiles/YellowPixel.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MyFirstAccessory.Projectiles
{
    // 이것은 GhostMinion과 아무 상관이 없는, 완전히 새로운 발사체입니다.
    public class YellowPixel : ModProjectile
    {
        public override void SetDefaults()
        {
            // 발사체의 기본 정보 설정
            Projectile.width = 8;        // 발사체 이미지의 가로 크기 (px)
            Projectile.height = 8;       // 발사체 이미지의 세로 크기 (px)
            Projectile.friendly = true;  // 플레이어 편 (적을 공격)
            Projectile.hostile = false;  // 적 편이 아님
            Projectile.DamageType = DamageClass.Magic; // 데미지 타입은 '마법'
            Projectile.penetrate = 1;    // 1개의 적만 맞추고 사라짐
            Projectile.timeLeft = 600;   // 10초(600틱) 동안 날아감
            Projectile.alpha = 255;      // 처음엔 투명하게 생성 (선택 사항)
            Projectile.light = 0.5f;     // 주변을 밝히는 빛
            Projectile.ignoreWater = true; // 물의 영향을 받지 않음
            Projectile.tileCollide = true; // 타일에 부딪히면 사라짐
        }

        // 발사체가 날아가는 동안 실행될 AI 코드
        public override void AI()
        {
            // 서서히 나타나는 효과 (선택 사항)
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }

            // 발사체가 진행 방향을 바라보도록 회전 (선택 사항)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        // 발사체가 사라질 때 (적이나 타일에 맞았을 때)
        public override void OnKill(int timeLeft)
        {
            // 노란색 먼지 파티클 생성
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch);
            }
        }
    }
}