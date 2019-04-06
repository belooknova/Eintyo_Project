using UnityEngine;
using System.Collections.Generic;
using Dungeon;

namespace Dungeon{
    class DgCreatePass{

        private DgMap map;

        //通路作成マップから除外するサイズ
        private int excludedSize;

        //作成された通路の座標を持つリスト
        private List<DgCell> passCoordinates = new List<DgCell>();

        //部屋を作成する候補座標
        private List<DgCell> candidateRoomCoordinates = new List<DgCell>();

        //どれだけ一定方向に伸ばし続けるか
        private int extendLength;
        //伸ばし方(0:じぐざぐ　1:まっすぐ)
        private bool extendType;
        private const int UP = 0;
        private const int RIGHT = 1;
        private const int DOWN = 2;
        private const int LEFT = 3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="excludedSize">部屋の最大の大きさ</param>
        /// <param name="map"></param>
        /// <param name="extendLength">どれだけ一定方向に伸ばし続けるか</param>
        /// <param name="extendType">伸ばし方(0:じぐざぐ　1:まっすぐ)</param>
        public DgCreatePass(DgMap map, int roomMaxSize){
            this.map = map;
            this.excludedSize = (int)Mathf.Ceil(roomMaxSize/2);
        }

        /// <summary>
        /// ジグザグ通路を作る
        /// </summary>
        /// <returns>通路情報を格納した二次元配列</returns>
        public DgMap CreateZigzagPass(){

            Debug.Log("CreateZigZagPass:46");
            //境界線を描く
            DrawBoundaryLine();

            DgCell nowCoordinate;

            //通路を描き始める座標を決める
            int firstX, firstY;
            firstX = Random.Range(excludedSize, map.SizeX-excludedSize);
            firstY = Random.Range(excludedSize, map.SizeY-excludedSize);
            nowCoordinate = new DgCell(firstX, firstY);
            passCoordinates.Add(nowCoordinate);
            map.RegisterTile(nowCoordinate, new DgFloorTile());

            //伸ばす方向
            int direction_X;
            int direction_Y;
            

            //最初の位置から通路を伸ばしていく。
            while(candidateRoomCoordinates.Count < 6){

                //部屋を作成する候補座標を登録する
                candidateRoomCoordinates.Add(nowCoordinate);

                // //今まで伸ばした経路の中から次伸ばす元の座標を選ぶ
                // nowCoordinate = passCoordinates[Random.Range(0, passCoordinates.Count)];

                //部屋を作成する候補座標を基に次の通路を伸ばし始める座標を決める
                nowCoordinate = candidateRoomCoordinates[
                    Random.Range(0,candidateRoomCoordinates.Count)];

                direction_X = Random.Range(0, 2) * 2 + 1;
                direction_Y = Random.Range(0, 2) * 2;

                //同じ方向にrepeatValue*(horizenValue + verticalValue)回通路を伸ばす
                int repeatValue = Random.Range(6, 8);
                int horizonValue;
                int verticalValue;
                

                for (int i = 0; i < repeatValue; i++)
                {
                    horizonValue = Random.Range(0, 3);
                    verticalValue = Random.Range(0, 3);
                    nowCoordinate = ExtendPass(nowCoordinate, horizonValue, direction_X);
                    nowCoordinate = ExtendPass(nowCoordinate, verticalValue, direction_Y);
                }
            }
            
            candidateRoomCoordinates.Add(nowCoordinate);
            //境界線を取り除く
            //RemoveBoundaryLine();

            return map;
        }


        /// <summary>
        /// 指定座標から指定された長さだけ指定された方向に通路を出来る限り伸ばす。
        /// </summary>
        /// <param name="startCoordinates">伸ばし始める座標</param>
        /// <param name="extendLength">伸ばす長さ</param>
        /// <param name="direction">伸ばす方向</param>
        /// <returns>最後に通路とした座標</returns>
        private DgCell ExtendPass(DgCell startCoordinates, int extendLength, int direction){
            DgCell nowCoordinate = startCoordinates;

            for (int i = 0; i < extendLength; i++)
            {
                if(CanExtendPass(nowCoordinate, direction)){
                    DgCell nextCoordinate = 
                        GetCoordinateInAssignedDirection(nowCoordinate,direction);
                    passCoordinates.Add(nextCoordinate);
                    map.RegisterTile(nextCoordinate, new DgFloorTile());
                    nowCoordinate = nextCoordinate;
                }else{
                    break;
                }
            }
            return nowCoordinate;
        }


        /// <summary>
        /// 指定座標の指定方向の座標を取得する。
        /// </summary>
        /// <param name="coordinate">指定座標</param>
        /// <param name="direction">指定方向</param>
        /// <returns>取得した座標</returns>
        private DgCell GetCoordinateInAssignedDirection(DgCell coordinate, int direction){
            if(direction == UP){return coordinate.Up;}
            if(direction == RIGHT){return coordinate.Right;}
            if(direction == DOWN){return coordinate.Down;}
            return coordinate.Left;
        }

        /// <summary>
        /// 指定された座標を基にそこから伸ばせる通路の座標をListに格納して戻す
        /// </summary>
        /// <returns>指定された座標から伸ばせる通路の座標</returns>
        private List<DgCell> StoreCoodinateOptions(DgCell coordinates){
            List<DgCell> coordinateOptions = new List<DgCell>();

            DgCell[] dgCells = new DgCell[4];
            dgCells[0] = coordinates.Up;
            dgCells[1] = coordinates.Right;
            dgCells[2] = coordinates.Down;
            dgCells[3] = coordinates.Left;
            
            for(int i = 0; i < 4; i++){
                if(CanCreatePass(dgCells[i])){
                    coordinateOptions.Add(dgCells[i]);
                }
            }

            return coordinateOptions;
        }

        /// <summary>
        /// 指定された座標から指定された方向に通路を伸ばすことができるか調べる。
        /// </summary>
        /// <returns>真偽値</returns>
        private bool CanExtendPass(DgCell coordinates, int direction){

            DgCell[] dgCells = new DgCell[4];
            dgCells[0] = coordinates.Up;
            dgCells[1] = coordinates.Right;
            dgCells[2] = coordinates.Down;
            dgCells[3] = coordinates.Left;
            
            if(CanCreatePass(dgCells[direction])){
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定座標に通路を作れるか調べる。
        /// </summary>
        /// <param name="assignedCoordinates">指定された座標</param>
        /// <returns>真偽値</returns>
        private bool CanCreatePass(DgCell assignedCoordinates){

            //assignedCoordinatesの上，右上，右，の順に回るように調べていく。
            
            //何回連続で他通路が見つかったか？
            //3回見つかった場合二重通路となってしまうためそこには通路を設置できない。
            int continuedValue = 0;

            //周りの座標の情報を入れ込む配列
            DgTileBase[] propertiesAroundGround = new DgTileBase[8];
            
            propertiesAroundGround[0] = map.GetTileInAssignedCoordinates(assignedCoordinates.X  ,assignedCoordinates.Y-1);
            propertiesAroundGround[1] = map.GetTileInAssignedCoordinates(assignedCoordinates.X+1,assignedCoordinates.Y-1);
            propertiesAroundGround[2] = map.GetTileInAssignedCoordinates(assignedCoordinates.X+1,assignedCoordinates.Y  );
            propertiesAroundGround[3] = map.GetTileInAssignedCoordinates(assignedCoordinates.X+1,assignedCoordinates.Y+1);
            propertiesAroundGround[4] = map.GetTileInAssignedCoordinates(assignedCoordinates.X  ,assignedCoordinates.Y+1);
            propertiesAroundGround[5] = map.GetTileInAssignedCoordinates(assignedCoordinates.X-1,assignedCoordinates.Y+1);
            propertiesAroundGround[6] = map.GetTileInAssignedCoordinates(assignedCoordinates.X-1,assignedCoordinates.Y  );
            propertiesAroundGround[7] = map.GetTileInAssignedCoordinates(assignedCoordinates.X-1,assignedCoordinates.Y-1);

            //余り
            int remainder;
            //左上、上、右上が通路だった場合等を考慮して余分に二回回している。
            for(int i = 0; i < 10; i++){
                remainder = i % 8;
                Debug.Log(remainder);
                //Tileが無い、または、通路ではない場合
                if(propertiesAroundGround[remainder] == null 
                    || !propertiesAroundGround[remainder].IsPass()){

                    continuedValue = 0;
                    continue;
                }
                //通路である場合
                if(propertiesAroundGround[remainder].IsPass()){
                    continuedValue++;
                    if(continuedValue == 3){ return false; }
                }
            }
            
            return true;
        }

        /// <summary>
        /// passCoordinatesに入れた座標をmapに反映させる。
        /// </summary>
        private void ReflectPassInMap(){
            foreach (DgCell coordinates in passCoordinates)
            {
                
            }
        }

        /// <summary>
        /// 通路を描画していい場所としてはダメな場所の境界に通路を敷く。
        /// 通路を敷くことでそこには通路が作成されないようになる。
        /// </summary>
        private void DrawBoundaryLine(){
            //通路の壁を描画する四角形の四点
            DgCell LeftUp = new DgCell(excludedSize-1, excludedSize-1);
            DgCell RightUp = new DgCell(map.SizeX-excludedSize, excludedSize-1);
            DgCell LeftDown = new DgCell(excludedSize-1, map.SizeY-excludedSize);
            
            //上側と下側
            for (int x = LeftUp.X; x < RightUp.X+1; x++)
            {
                map.RegisterTile(x, LeftUp.Y, new DgFloorTile());
                map.RegisterTile(x, LeftDown.Y, new DgFloorTile());
            }

            //左側と右側
            for (int y = LeftUp.Y; y < LeftDown.Y+1; y++)
            {
                map.RegisterTile(LeftUp.X, y, new DgFloorTile());
                map.RegisterTile(RightUp.X, y, new DgFloorTile());
            }

            
            
        }

        /// <summary>
        /// 通路を描画していい場所としてはダメな場所の境界に敷いた境界線を
        /// 除去する。
        /// </summary>
        private void RemoveBoundaryLine(){
            //通路の壁を描画する四角形の四点
            DgCell LeftUp = new DgCell(excludedSize-1, excludedSize-1);
            DgCell RightUp = new DgCell(map.SizeX-excludedSize, excludedSize-1);
            DgCell LeftDown = new DgCell(excludedSize-1, map.SizeY-excludedSize);
            
            //上側と下側
            for (int x = LeftUp.X; x < RightUp.X+1; x++)
            {
                map.RemoveTile(x, LeftUp.Y);
                map.RemoveTile(x, LeftDown.Y);
            }

            //左側と右側
            for (int y = LeftUp.Y; y < LeftDown.Y+1; y++)
            {
                map.RemoveTile(LeftUp.X, y);
                map.RemoveTile(RightUp.X, y);
            }

        }

        
    }
}