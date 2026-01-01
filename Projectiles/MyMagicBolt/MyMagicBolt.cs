using Microsoft.Xna.Framework; // Vector2를 위해 필요
using Microsoft.Xna.Framework.Graphics; // Texture2D, SpriteEffects를 위해 필요
using Terraria; // Main, Projectile을 위해 필요
using Terraria.GameContent; // TextureAssets를 위해 필요
using Terraria.ID; // DustID 등을 위해 필요
using Terraria.ModLoader; // ModProjectile을 위해 필요

namespace MyFirstAccessory.Projectiles.MyMagicBolt
{
    public class MyMagicBolt : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.friendly = true;  // NPC들을 공격하기 위해 true 유지
    Projectile.hostile = true;   // 플레이어를 공격하기 위해 true 추가!!

        }

        public override void AI() {
            // PNG가 위를 보고 있다면 + MathHelper.PiOver2 유지
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2)) {
                // 파편은 항상 투사체 중심(Center)에서 생성
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleCrystalShard, Vector2.Zero);
                d.noGravity = true;
            }
        }
                    // MyMagicBolt.cs 파일 내부에 아래 메소드를 통째로 복사해서 넣으세요.
            // (SetDefaults나 AI 메소드 아래에 넣으면 됩니다.)

            public override bool? CanHitNPC(NPC target) {
                // Projectile.ai[0]에는 아까 우리가 저장한 상인의 번호(NPC.whoAmI)가 들어있습니다.
                // 만약 부딪힌 NPC(target)의 번호가 내 주인(ai[0])과 같다면?
                if (target.whoAmI == (int)Projectile.ai[0]) {
                    return false; // 공격하지 마라 (통과해라)
                }
                
                // 그 외의 NPC들은 정상적으로 맞음
                return null; 
            }

            // 플레이어는 주인일 리가 없으므로 항상 맞게 설정 (로그 상 0번 타겟 공격용)
            public override bool CanHitPlayer(Player target) {
                return true; 
            }


        public override bool PreDraw(ref Color lightColor) {
            // 1. TextureAssets는 Terraria.GameContent에 들어있습니다.
            // .Value를 붙여야 Asset에서 실제 Texture2D를 꺼냅니다.
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // 2. Width와 Height 뒤에 괄호()를 제거했습니다 (속성값 사용).
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            // 3. 그릴 위치 (화면 좌표 계산)
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            // 4. SpriteEffects는 Microsoft.Xna.Framework.Graphics에 들어있습니다.
            Main.EntitySpriteDraw(
                texture, 
                drawPos, 
                null, 
                lightColor, 
                Projectile.rotation, 
                drawOrigin, 
                Projectile.scale, 
                SpriteEffects.None, 
                0
            );

            return false; // 기본 그리기를 끄고 중심점이 보정된 코드로 직접 그림
        }
    }
}