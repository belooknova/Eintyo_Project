using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataManager", menuName = "DataBase/Create_ItemDataBase")]
public class ItemDataManager : ScriptableObject
{
    [SerializeField]
    private List<Item_DB> itemLists = new List<Item_DB>();

    //　ステートリストを返す
    public List<Item_DB> GetItemLists()
    {
        return itemLists;
    }

    //リストの長さを取得する
    public int GetListLenght()
    {
        return itemLists.Count;
    }

}
