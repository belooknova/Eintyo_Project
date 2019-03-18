using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataManager", menuName = "DataBase/CreateSkillDataBase")]
public class SkillDataManager : ScriptableObject
{
    [SerializeField]
    private List<Skill_DB> skillLists = new List<Skill_DB>();

    //　ステートリストを返す
    public List<Skill_DB> GetSkillLists()
    {
        return skillLists;
    }

    //リストの長さを取得する
    public int GetListLenght()
    {
        return skillLists.Count;
    }

}
