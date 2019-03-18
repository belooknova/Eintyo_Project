using UnityEngine;

namespace Dungeon{
abstract class DgTileBase{

        abstract public bool IsPass();
        abstract public bool IsWall();

        virtual public void DrawTile(int x, int y){
            
        }
}
}