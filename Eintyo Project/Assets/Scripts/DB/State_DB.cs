using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "State", menuName = "DataBase/CreateState")]
public class State_DB : ScriptableObject
{
    [SerializeField]
    private string StateName; //状態の名前

    [SerializeField, Range(0, 10)]
    private int priority; //優先度

    [SerializeField]
    private int release_trun = 3;

    //--------------------------

    public string GetName()
    {
        return StateName;
    }

    public int GetPrio()
    {
        return priority;
    }

    public int Getrelease()
    {
        return release_trun;
    }

}
