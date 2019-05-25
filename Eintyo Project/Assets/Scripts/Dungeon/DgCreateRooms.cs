using UnityEngine;

namespace Dungeon{
class DgCreateRooms{

    private DgMap map;

    /// <summary>
    /// 部屋の最大の大きさ。
    /// </summary>
    private int roomMaxSize;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="map"></param>
    public DgCreateRooms(DgMap map, int roomMaxSize){
        this.map = map;
        this.roomMaxSize = roomMaxSize;
    }
    
    /// <summary>
    /// 部屋を作る
    /// </summary>
    /// <returns>部屋情報を追加した二次元配列</returns>
    public DgMap CreateRooms(){
        DgCell roomCoordinate;

        for(int i = 0; i < 6; i++){
            roomCoordinate = map.FetchRoomCoordinate();
            if(roomCoordinate == null){
                Debug.Log("部屋をこれ以上描画できません");
                return map;
            }
            Debug.Log("部屋を描画する！");
            DrawRoom(roomCoordinate);
        }
        return map;
    }

    /// <summary>
    /// 指定座標を中心とした部屋を一つ作成する
    /// </summary>
    /// <param name="roomCoordinate">部屋の中心となる座標</param>
    private void DrawRoom(DgCell roomCoordinate){
        int roomSizeX = (int)Random.Range(roomMaxSize-2, roomMaxSize+1);
        int roomSizeY = (int)Random.Range(roomMaxSize-2, roomMaxSize+1);
        
        //部屋のTileを置き始める左上座標を求める。
        DgCell roomLeftUpCoordinate = 
            new DgCell(
                roomCoordinate.X - (int)Mathf.Ceil(roomSizeX/2), roomCoordinate.Y - (int)Mathf.Ceil(roomSizeY/2)
            );

        int nowDrawingX = roomLeftUpCoordinate.X;
        int nowDrawingY = roomLeftUpCoordinate.Y;

        for(int y = 0; y < roomSizeY; y++){
            for(int x = 0; x < roomSizeX; x++){
                map.RegisterTile(nowDrawingX, nowDrawingY, new DgRoomFloorTile());
                nowDrawingX++;
            }
            nowDrawingX = roomLeftUpCoordinate.X;
            nowDrawingY++;
        }
    }
}
}