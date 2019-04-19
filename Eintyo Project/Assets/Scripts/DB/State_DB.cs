using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "State", menuName = "DataBase/Create_State")]
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

    //この状態になったときに呼び出されるメソッド
    public void InitialOperate(StatusData target)
    {

    }

    //この状態が終了するときに呼び出されるメソッド
    public void EndOperate(StatusData target)
    {

    }

}
