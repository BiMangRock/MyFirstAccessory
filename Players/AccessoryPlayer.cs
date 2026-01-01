// Players/AccessoryPlayer.cs (데이터 저장/로드 기능 추가)

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO; // SaveData와 LoadData를 위해 필요합니다.
using MyFirstAccessory.Items.GrowthCharm;

namespace MyFirstAccessory.Players
{
    public class AccessoryPlayer : ModPlayer
    {
        // [추가] 성장으로 얻은 추가 체력을 기록할 변수
        public int growthBonus = 0;

        // 게임을 나갔다 들어와도 보너스를 유지하기 위해 데이터를 저장합니다.
        public override void SaveData(TagCompound tag) {
            tag["growthBonus"] = growthBonus;
        }

        // 저장된 데이터를 불러옵니다.
        public override void LoadData(TagCompound tag) {
            growthBonus = tag.GetInt("growthBonus");
        }
        
        // 플레이어의 최대 체력을 실제로 올려주는 부분입니다.
        public override void PostUpdate() {
            Player.statLifeMax2 += growthBonus;
        }

        // NPC를 죽였을 때 보너스를 올리는 부분입니다.
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool hasAccessory = false;
            for (int i = 3; i < 10; i++) {
                if (Player.armor[i].type == ModContent.ItemType<GrowthCharm>()) {
                    hasAccessory = true;
                    break;
                }
            }
            
            if (hasAccessory && target.life <= 0)
            {
                // [수정] 이제 변수에 보너스를 기록합니다.
                growthBonus += 1;
                CombatText.NewText(Player.getRect(), CombatText.HealLife, "+1 HP");
            }
        }
    }
}