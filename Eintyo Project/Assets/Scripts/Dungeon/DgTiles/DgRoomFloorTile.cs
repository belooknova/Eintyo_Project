using Dungeon;

namespace Dungeon{
class DgRoomFloorTile : DgTileBase{
    public override bool IsPass(){
        return true;
    }
    public override bool IsWall(){
        return false;
    }

}
}