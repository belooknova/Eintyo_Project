using Dungeon;

namespace Dungeon{

class NatureDgFactory : DgFactoryBase{
    protected override DgBase createDungeon(int sizeX, int sizeY){
        return new NatureDg(sizeX, sizeY);
    }
}
}