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

    protected int sizeX;
    protected int sizeY;
    
    protected DgTileBase[,] map;
    abstract public void Use();

    /// <summary>
    /// mapに格納された情報を基に実際にダンジョンを描画する
    /// </summary>
    virtual protected void DrawMap(){
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                map[sizeY, sizeX].DrawTile(j, i);
                if (map[j, i] is DgFloorTile)
                {
                    Instantiate(floorTile, new Vector3(j, i, 0), Quaternion.identity);
                }else{
                    if (map[j, i] is DgFl)
                    {
                        Instantiate(floorTile, new Vector3(j, i, 0), Quaternion.identity);
                    }
                }
            }
        }
    }
}
}