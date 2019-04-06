using Dungeon;

namespace Dungeon{
class DgFloorTile : DgTileBase{
    public override bool IsPass(){
        return true;
    }
    public override bool IsWall(){
        return false;
    }

}
}