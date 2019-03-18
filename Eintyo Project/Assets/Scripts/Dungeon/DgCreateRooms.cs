namespace Dungeon{
    class DgCreateRooms{

        private DgTileBase[,] map;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="map"></param>
        public DgCreateRooms(DgTileBase[,] map){
            this.map = map;
        }
        
        /// <summary>
        /// 部屋を作る
        /// </summary>
        /// <returns>部屋情報を追加した二次元配列</returns>
        public DgTileBase[,] CreateRooms(){
            return map;
        }
    }
}