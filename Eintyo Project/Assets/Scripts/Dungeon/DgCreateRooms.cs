namespace Dungeon{
    class DgCreateRooms{

        private DgMap map;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="map"></param>
        public DgCreateRooms(DgMap map){
            this.map = map;
        }
        
        /// <summary>
        /// 部屋を作る
        /// </summary>
        /// <returns>部屋情報を追加した二次元配列</returns>
        public DgMap CreateRooms(){
            return map;
        }
    }
}