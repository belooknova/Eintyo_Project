using UnityEngine;
using System.Collections.Generic;
using Dungeon;

namespace Dungeon{
    class DgCreatePass{

        private DgTileBase[,] map;

        //作成された通路の座標を持つリスト
        private List<DgCell> passCoordinates;

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
        /// <param name="map"></param>
        /// <param name="extendLength">どれだけ一定方向に伸ばし続けるか</param>
        /// <param name="extendType">伸ばし方(0:じぐざぐ　1:まっすぐ)</param>
        public DgCreatePass(DgTileBase[,] map){
            this.map = map;
        }

        /// <summary>
        /// ジグザグ通路を作る
        /// </summary>
        /// <returns>通路情報を格納した二次元配列</returns>
        public DgTileBase[,] CreateZigzagPass(){

            DgCell nowCoordinate;

            //decide first coordinates(端っこは使用しない。
            //通路作成時に面倒くせえなため)
            int firstX, firstY;
            firstX = Random.Range(1, map.GetLength(1)-1);
            firstY = Random.Range(1, map.GetLength(0)-1);
            passCoordinates.Add(new DgCell(firstX, firstY));

            //伸ばす方向
            int direction_X;
            int direction_Y;
            

            //最初の位置から通路を伸ばしていく。
            while(passCoordinates.Count < 200){

                nowCoordinate = passCoordinates[Random.Range(0, passCoordinates.Count)];

                direction_X = Random.Range(0, 1) * 2 + 1;
                direction_Y = Random.Range(0, 1) * 2;

                for (int i = 0; i < 20; i++)
                {
                    nowCoordinate = ExtendPass(nowCoordinate, 2, direction_X);
                    nowCoordinate = ExtendPass(nowCoordinate, 2, direction_Y);
                }
            }

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
            DgCell nowCoordinates = startCoordinates;

            for (int i = 0; i < extendLength; i++)
            {
                if(CanExtendPass(nowCoordinates, direction)){
                    DgCell nextCoordinate = GetCoordinateInAssignedDirection(nowCoordinates,
                                            direction);
                    passCoordinates.Add(nextCoordinate);
                    nowCoordinates = nextCoordinate;
                }else{
                    break;
                }
            }

            return nowCoordinates;
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
            
            propertiesAroundGround[0] = map[assignedCoordinates.Y-1,assignedCoordinates.X  ];
            propertiesAroundGround[1] = map[assignedCoordinates.Y-1,assignedCoordinates.X+1];
            propertiesAroundGround[2] = map[assignedCoordinates.Y  ,assignedCoordinates.X+1];
            propertiesAroundGround[3] = map[assignedCoordinates.Y+1,assignedCoordinates.X+1];
            propertiesAroundGround[4] = map[assignedCoordinates.Y+1,assignedCoordinates.X  ];
            propertiesAroundGround[5] = map[assignedCoordinates.Y+1,assignedCoordinates.X-1];
            propertiesAroundGround[6] = map[assignedCoordinates.Y  ,assignedCoordinates.X-1];
            propertiesAroundGround[7] = map[assignedCoordinates.Y-1,assignedCoordinates.X-1];

            for(int i = 0; i < 8; i++){
                if(propertiesAroundGround[i] == null){ continue; }
                if(propertiesAroundGround[i].IsPass()){
                    continuedValue++;
                    if(continuedValue == 3){ return false; }
                }
                if(!propertiesAroundGround[i].IsPass()){
                    continuedValue = 0;
                }
            }
            
            return true;
        }

        
    }
}