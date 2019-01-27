using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateDataManager", menuName = "DataBase/CreateStateDataBase")]
public class StateDataManager : ScriptableObject
{
    [SerializeField]
    private List<State_DB> stateLists = new List<State_DB>();

    //　ステートリストを返す
    public List<State_DB> GetStateLists()
    {
        return stateLists;
    }

}
