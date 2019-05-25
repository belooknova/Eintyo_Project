/// <summary>
/// DungeonBase
/// </summary>
using UnityEngine;
using System.Collections.Generic;
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

    //Tile達。消すために保管しておく。
    private List<GameObject> tilesList = new List<GameObject>();

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
                    putTile(floorTile, x, y);
                }else if (map.GetTileInAssignedCoordinates(x,y) is DgFrontWallTile){
                    //前側の壁が登録されている
                    putTile(frontWallTile, x, y);
                }
            }
        }
    }

    /// <summary>
    /// prefabからGameObjectをインスタンス化
    /// </summary>
    /// <param name="tileObj">tileのprefab</param>
    /// <param name="x">設置する座標</param>
    /// <param name="y">設置する座標</param>
    private void putTile(GameObject tileObj, int x, int y){
        GameObject obj = Instantiate(tileObj, new Vector3(x*1.28f, y*1.28f, 0), Quaternion.identity);
        obj.transform.localScale = new Vector3(4,4,4);
        tilesList.Add(obj);
    }

    /// <summary>
    /// Sceneからマップを削除したいときに呼び出してください。
    /// </summary>
    public void RemoveTileGameObject(){
        foreach(GameObject tileObj in tilesList){
            Destroy(tileObj);
        }
    }
}
}