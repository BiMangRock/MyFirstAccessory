using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace MyFirstAccessory.NPCs.expMob1
{
    public class expMob1 : ModNPC
    {
        // 1. 원본 상인의 텍스처 경로 지정 (xnb 파일을 직접 읽음)
        public override string Texture => "Terraria/Images/TownNPCs/Merchant_Default";

        private int revengeTargetIndex = -1;
        private bool targetIsPlayer = false;
        private int attackTimer = 0;

        public override void SetStaticDefaults() {
            // 상인 원본 스프라이트의 프레임 수
            Main.npcFrameCount[NPC.type] = 25; 
            NPCID.Sets.AttackFrameCount[NPC.type] = 5; 
            NPCID.Sets.DangerDetectRange[NPC.type] = 700; 
            
            // 마을 NPC처럼 보이게 설정 (이름 표시 등을 위해)
            NPCID.Sets.ActsLikeTownNPC[NPC.type] = true;
        }

        public override void SetDefaults() {
            NPC.width = 18;  // 상인 히트박스 가로
            NPC.height = 40; // 상인 히트박스 세로
            NPC.damage = 20;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1; // 커스텀 AI
            NPC.value = 1000f;
        }

        public override void AI() {
            // 1. 타겟 찾기
            FindTarget();
            

            if (NPC.target >= 0) {
                Entity targetEntity = targetIsPlayer ? (Entity)Main.player[NPC.target] : (Entity)Main.npc[NPC.target];
                
                // 타겟 방향 바라보기
                NPC.direction = (targetEntity.Center.X < NPC.Center.X) ? -1 : 1;
                NPC.spriteDirection = NPC.direction;

                float distance = Vector2.Distance(NPC.Center, targetEntity.Center);
                
                // 2. 공격 로직 (지팡이 투사체 발사)
                if (distance < 500f) {
                    attackTimer++;
                    if (attackTimer >= 60) { // 1초마다 발사
                        Main.NewText($"[공격!] 내 번호: {NPC.whoAmI}, 현재 타겟 번호: {NPC.target}");
                        Vector2 shootVel = (targetEntity.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 5f;
                        
                        // 이전에 만든 지팡이 투사체를 소환
                        // 주의: 프로젝트 내 MyMagicBolt의 정확한 네임스페이스와 클래스명을 확인하세요.
                        int projType = ModContent.ProjectileType<Projectiles.MyMagicBolt.MyMagicBolt>();
                        //Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootVel, projType, 20, 3f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootVel, projType, 20, 3f, Main.myPlayer, NPC.whoAmI);
                        attackTimer = 0;
                    }
                }

                // 3. 이동 로직 (부드러운 추격)
                if (distance > 120f) {
                    NPC.velocity.X = (NPC.velocity.X * 20f + NPC.direction * 1.8f) / 21f;

                        // [점프 로직 추가]
    // NPC.collideX는 NPC가 가로 방향으로 벽에 부딪혔을 때 true가 됩니다.
    // NPC.velocity.Y == 0f는 현재 땅에 서 있다는 뜻입니다.
    if (NPC.collideX && NPC.velocity.Y == 0f) {
        NPC.velocity.Y = -15f; // 위쪽으로 힘을 주어 점프 (숫자가 클수록 높게 점프)
    }

                } 
                else {
                    NPC.velocity.X *= 0.9f; 
                }
            }
            
            // 중력 적용
            NPC.velocity.Y += 0.4f; 
        }
        private void FindTarget() {
    // 1. 복수 타겟이 있는지 먼저 확인 (최우선순위)
    if (revengeTargetIndex != -1) {
        Entity rev = targetIsPlayer ? (Entity)Main.player[revengeTargetIndex] : (Entity)Main.npc[revengeTargetIndex];
        if (rev.active && (targetIsPlayer ? !((Player)rev).dead : ((NPC)rev).life > 0)) {
            NPC.target = revengeTargetIndex;
            return;
        }
        revengeTargetIndex = -1; 
    }

    float closestDist = 700f;
    NPC.target = -1;

    // 2. [주민 NPC 최우선 스캔] 
    // 여기서는 오직 'friendly'가 true인 주민들만 찾습니다.
    for (int i = 0; i < Main.maxNPCs; i++) {
        NPC other = Main.npc[i];
        if (other.active && other.whoAmI != NPC.whoAmI && other.type != NPC.type && other.friendly) {
            float d = Vector2.Distance(NPC.Center, other.Center);
            if (d < closestDist) {
                closestDist = d;
                NPC.target = i;
                targetIsPlayer = false;
            }
        }
    }

    // [중요] 만약 주변에 주민을 한 명이라도 찾았다면, 여기서 바로 함수를 끝냅니다(return).
    // 이렇게 해야 아래에 있는 플레이어나 몬스터를 쳐다보지도 않습니다.
    if (NPC.target != -1) return; 

    // 3. [차선책: 주민이 없을 때만 플레이어와 몬스터를 스캔]
    
    // 플레이어 스캔
    for (int i = 0; i < Main.maxPlayers; i++) {
        Player p = Main.player[i];
        if (p.active && !p.dead) {
            float d = Vector2.Distance(NPC.Center, p.Center);
            if (d < closestDist) {
                closestDist = d;
                NPC.target = i;
                targetIsPlayer = true;
            }
        }
    }

    // 몬스터(적대적 NPC) 스캔 (플레이어보다 몬스터가 더 가까우면 몬스터를 잡음)
    for (int i = 0; i < Main.maxNPCs; i++) {
        NPC other = Main.npc[i];
        if (other.active && other.whoAmI != NPC.whoAmI && other.type != NPC.type && !other.friendly) {
            float d = Vector2.Distance(NPC.Center, other.Center);
            if (d < closestDist) {
                closestDist = d;
                NPC.target = i;
                targetIsPlayer = false;
            }
        }
    }
}


// public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) {
//     // 플레이어가 나를 때리는 순간, 기존에 누구를 쫓고 있었든 상관없이 
//     // revengeTargetIndex를 '방금 나를 때린 놈'의 번호로 덮어씌웁니다.
//     // revengeTargetIndex = player.whoAmI;
//     // targetIsPlayer = true;
// }

//         public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
//             //Main.NewText($"투사체에 맞음! 투사체 이름: {projectile.Name}, 데미지: {damageDone}");
//             // if (projectile.owner >= 0 && projectile.owner < 255) {
//             //     revengeTargetIndex = projectile.owner;
//             //     targetIsPlayer = true;
//             // }
//         }

        // 플레이어가 아이템(칼 등)으로 때릴 때 - 무시하도록 비워둠
public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) { }

// 플레이어가 쏜 투사체에 맞을 때 - 무시하도록 비워둠
public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) { }



        // 프레임 애니메이션 처리 (상인 원본 스프라이트 구조 기준)
        public override void FindFrame(int frameHeight) {
            if (NPC.velocity.Y != 0f) {
                NPC.frame.Y = 1 * frameHeight; // 점프/공중 프레임
            } else if (NPC.velocity.X == 0f) {
                NPC.frame.Y = 0 * frameHeight; // 대기 프레임
            } else {
                // 걷기 프레임 (2~15번 사용)
                NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.15f;
                NPC.frameCounter %= 14; 
                NPC.frame.Y = (int)(2 + NPC.frameCounter) * frameHeight;
            }

            // 공격 타이머가 끝에 도달했을 때 팔 뻗는 모션 (21번 프레임)
            if (attackTimer > 40) {
                NPC.frame.Y = 21 * frameHeight;
            }
        }

        // 머리 위 UI 그리기
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            // 1. 이름 표시
            string name = "expMob1 (수호자)";
            Vector2 nameSize = FontAssets.MouseText.Value.MeasureString(name);
            Vector2 namePos = NPC.Top - screenPos + new Vector2(0, -45);
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, name, 
                namePos - new Vector2(nameSize.X / 2, 0), Color.Yellow, 0f, Vector2.Zero, Vector2.One);

            // 2. 체력바 표시
            Vector2 barPos = NPC.Top - screenPos + new Vector2(-20, -20);
            float healthPercent = (float)NPC.life / NPC.lifeMax;
            
            // 배경
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barPos.X, (int)barPos.Y, 40, 4), Color.Black);
            // 체력 (초록색)
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barPos.X, (int)barPos.Y, (int)(40 * healthPercent), 4), Color.LimeGreen);
        }
    }
}