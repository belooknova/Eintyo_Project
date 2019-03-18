using Dungeon;
class NatureDg : DgBase{

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sizeX">Ｘ方向のサイズ</param>
    /// <param name="sizeY">Ｙ方向のサイズ</param>
    public NatureDg(int sizeX, int sizeY){
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        map = new DgTileBase[sizeY, sizeX];

        DgCreatePass dgCreatePass = new DgCreatePass(map);
        dgCreatePass.CreateZigzagPass();

        DgCreateRooms dgCreateRooms = new DgCreateRooms(map);
        dgCreateRooms.CreateRooms();

        DrawMap();
    }

    public override void Use(){
        DrawMap();
    }

    
}