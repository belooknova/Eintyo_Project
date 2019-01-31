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

    //リストの長さを取得する
    public int GetListLenght()
    {
        return stateLists.Count;
    }

    //リストの要素数とステート名の対応関係を出力する
    public Dictionary<string, int> GetAddressTable()
    {
        Dictionary<string, int> dect = new Dictionary<string, int>();
        for (int i=0; i < GetListLenght(); i++)
        {
            string name = stateLists[i].name;
            dect.Add(name, i);
            Debug.Log(name);
        }
        return dect;
    }


}
