/// <summary>
/// DungeonBase
/// </summary>
using UnityEngine;
using Dungeon;

namespace Dungeon
{
abstract class DgBase : MonoBehaviour
{
    [SerializeField] protected GameObject floorTile;
    [SerializeField] protected GameObject frontWallTile;
    
    protected DgMap map;

    //部屋の最大の大きさ
    protected int roomMaxSize;

    /// <summary>
    /// マップを作成する
    /// </summary>
    /// <param name="sizeX">Ｘ方向のサイズ</param>
    /// <param name="sizeY">Ｙ方向のサイズ</param>
    abstract public void Use(int sizeX, int sizeY);

    /// <summary>
    /// mapに格納された情報を基に実際にダンジョンを描画する
    /// </summary>
    virtual public void DrawMap(){
        for (int y = 0; y < map.SizeY; y++)
        {
            for (int x = 0; x < map.SizeX; x++)
            {
                //mapに何も登録されていないならば
                if (map.GetTileInAssignedCoordinates(x,y)==null)
                {
                    //何もしない

                }else if (map.GetTileInAssignedCoordinates(x, y) is DgFloorTile){
                    //床タイルが登録されている
                    GameObject obj = Instantiate(floorTile, new Vector3(x*1.28f, y*1.28f, 0), Quaternion.identity);
                    obj.transform.localScale = new Vector3(4,4,4);
                }else if (map.GetTileInAssignedCoordinates(x,y) is DgFrontWallTile){
                    //前側の壁が登録されている
                    GameObject obj = Instantiate(frontWallTile, new Vector3(x*1.28f, y*1.28f, 0), Quaternion.identity);
                    obj.transform.localScale = new Vector3(4,4,4);
                }
            }
        }
    }
}
}