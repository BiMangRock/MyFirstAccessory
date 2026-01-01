// Projectiles/GhostMinion.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MyFirstAccessory.Projectiles
{
    public class GhostMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // [최종 수정] SetDefault 코드를 삭제했습니다.
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {   
            Projectile.usesLocalNPCImmunity = true;
            Projectile.damage = 5;
            Projectile.width = 18;
            Projectile.height = 28;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
        }

        public override bool? CanCutTiles() => false;

        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.GhostBuff>());
                return;
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.GhostBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            float targettingDistance = 700f;
            Vector2 targetCenter = Projectile.position;
            bool hasTarget = false;

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < targettingDistance)
                    {
                        targetCenter = npc.Center;
                        hasTarget = true;
                    }
                }
            }
            if (!hasTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        if (between < targettingDistance)
                        {
                            targettingDistance = between;
                            targetCenter = npc.Center;
                            hasTarget = true;
                        }
                    }
                }
            }

            float speed = 4f;
            float inertia = 20f;

            if (hasTarget)
            {
                Vector2 direction = targetCenter - Projectile.Center;
                direction.Normalize();
                direction *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
            }
            else
            {
                Vector2 idlePosition = player.Center;
                idlePosition.Y -= 48f;
                Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                float distanceToIdlePosition = vectorToIdlePosition.Length();
                if (distanceToIdlePosition > 1000f)
                {
                    Projectile.position = idlePosition;
                    Projectile.velocity *= 0.1f;
                }
                else if (distanceToIdlePosition > 20f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);
        }




        // GhostMinion.cs 파일 안에 이 새로운 함수를 추가하세요.
        // AI() 함수가 끝나는 괄호 '}' 바로 뒤에 추가하면 됩니다.

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 이 함수는 소환수가 적을 성공적으로 때렸을 때 '단 한 번'만 실행됩니다.

            // 이 소환수에게, 방금 때린 그 적(target)을 60프레임(1초) 동안
            // '공격 불가능한 무적 상태'로 만들어라' 라고 명령합니다.
            Projectile.localNPCImmunity[target.whoAmI] = 60;
        }

    }
}
