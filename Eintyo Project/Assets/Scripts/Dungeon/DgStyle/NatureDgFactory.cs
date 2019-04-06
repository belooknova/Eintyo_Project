using Dungeon;

namespace Dungeon{

class NatureDgFactory : DgFactoryBase{
    
    protected override DgBase createDungeon(int sizeX, int sizeY){
        dgBase.Use(sizeX, sizeY);
        return dgBase;
    }
}
}