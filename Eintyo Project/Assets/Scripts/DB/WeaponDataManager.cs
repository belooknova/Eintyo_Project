using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataManager", menuName = "DataBase/Create_WeaponDataBase")]
public class WeaponDataManager : ScriptableObject
{
    [SerializeField]
    private List<Weapon_DB> weaponLists = new List<Weapon_DB>();

    //　ステートリストを返す
    public List<Weapon_DB> GetItemLists()
    {
        return weaponLists;
    }

    //リストの長さを取得する
    public int GetListLenght()
    {
        return weaponLists.Count;
    }

}
