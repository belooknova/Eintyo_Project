
using Dungeon;

namespace Dungeon{
class DgMap{

    //マップを格納する配列変数
    private DgTileBase[,] map;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sizeX">マップサイズ</param>
    /// <param name="sizeY">マップサイズ</param>
    public DgMap(int sizeX, int sizeY){
        map = new DgTileBase[sizeY, sizeX];
    }

    /// <summary>
    /// 指定座標にTileを登録する
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="tile"></param>
    public void RegisterTile(DgCell coordinates, DgTileBase tile){
        map[coordinates.Y, coordinates.X] = tile;
    }

    /// <summary>
    /// 指定座標にTileを登録する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="tile"></param>
    public void RegisterTile(int x, int y, DgTileBase tile){
        map[y, x] = tile;
    }

    /// <summary>
    /// 指定座標のTileを除去する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RemoveTile(int x, int y){
        map[y, x] = null;
    }

    /// <summary>
    /// 指定座標のTileを返す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public DgTileBase GetTileInAssignedCoordinates(int x, int y){
        return map[y, x];
    }

    /// <summary>
    /// マップのサイズXを返す
    /// </summary>
    /// <returns></returns>
    public int SizeX{
        get{
            return map.GetLength(1);
        }
    }

    /// <summary>
    /// マップのサイズYを返す
    /// </summary>
    /// <returns></returns>
    public int SizeY{
        get{
            return map.GetLength(0);
        }
    }

}


}