using Dungeon;

namespace Dungeon{
class DgFrontWallTile : DgTileBase{
    public override bool IsPass(){
        return false;
    }
    public override bool IsWall(){
        return true;
    }

}
}