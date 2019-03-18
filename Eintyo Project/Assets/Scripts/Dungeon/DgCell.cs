/// <summary>
/// 座標を保持するクラス
/// </summary>
namespace Dungeon{
    class DgCell{
        private int x, y;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public DgCell(int x, int y){
            this.x = x;
            this.y = y;
        }
        
        public int X{
            get{
                return x;
            }
        }
        public int Y{
            get{
                return y;
            }
        }

        public DgCell Up{
            get{
                return new DgCell(x, y-1);
            }
        }

        public DgCell Right{
            get{
                return new DgCell(x+1, y);
            }
        }

        public DgCell Down{
            get{
                return new DgCell(x, y+1);
            }
        }

        public DgCell Left{
            get{
                return new DgCell(x-1, y);
            }
        }
    }
}