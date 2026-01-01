// Tiles/ModdersDesk.cs (수정 완료)

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MyFirstAccessory.Items;

namespace MyFirstAccessory.Tiles
{
    public class ModdersDesk : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = true;

            // --- 가구 배치 및 크기 설정 ---
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2); // 바닐라의 3x2 크기 가구 스타일을 복사
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 }; // 각 칸의 세로 픽셀 높이
            
            // [수정!] 가구의 기준점을 바닥 중앙으로 설정합니다.
            // 1은 가로축 중앙(가로 3칸 중 가운데), 2는 세로축 맨 아래(세로 2칸 중 아래)를 의미합니다.
            TileObjectData.newTile.Origin = new Point16(1, 1); 

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            IEntitySource source = new EntitySource_TileBreak(i, j);
            Item.NewItem(source, i * 16, j * 16, 48, 32, ModContent.ItemType<ModdersDeskItem>());
        }
    }
}