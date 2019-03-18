using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;

namespace Dungeon
{
abstract class DgFactoryBase : MonoBehaviour {

    public DgBase create(int SizeX, int SizeY){
        DgBase dungeonBase = createDungeon(SizeX, SizeY);

        return dungeonBase;


    }

    abstract protected DgBase createDungeon(int SizeX, int SizeY); 

}
}