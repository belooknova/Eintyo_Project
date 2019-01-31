using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "State", menuName = "DataBase/CreateState")]
public class State_DB : ScriptableObject
{
    [SerializeField]
    private string _stateName; //状態の名前

    [SerializeField, Range(0, 10)]
    private int priority; //優先度

    [SerializeField]
    private int release_trun = 3;

    //--------------------------

    public string StateName
    {
        get { return _stateName; }
    }

    public int GetPrio
    {
        get { return priority; }
    }

    public int Getrelease
    {
        get { return release_trun; }
    }

}
