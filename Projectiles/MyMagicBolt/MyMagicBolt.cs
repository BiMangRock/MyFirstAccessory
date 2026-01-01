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