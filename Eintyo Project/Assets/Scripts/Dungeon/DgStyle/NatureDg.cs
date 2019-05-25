using Dungeon;
/// <summary>
/// 自然系Productクラス
/// </summary>
class NatureDg : DgBase{

    void Start(){
        roomMaxSize = 6;
    }

    public override void Use(int sizeX, int sizeY){
        map = new DgMap(sizeX, sizeY);

        DgCreatePass dgCreatePass = new DgCreatePass(map, roomMaxSize);
        map = dgCreatePass.CreateZigzagPass();

        DgCreateRooms dgCreateRooms = new DgCreateRooms(map, roomMaxSize);
        map = dgCreateRooms.CreateRooms();

        DrawMap();
    }

    
}