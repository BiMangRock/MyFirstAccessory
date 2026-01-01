// Buffs/GhostBuff.cs
using Terraria;
using Terraria.ModLoader;
using MyFirstAccessory.Projectiles.GhostMinion;

namespace MyFirstAccessory.Buffs
{
    public class GhostBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // [최종 수정] SetDefault 코드를 모두 삭제했습니다.
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GhostMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}