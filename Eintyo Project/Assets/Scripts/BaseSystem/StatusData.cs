using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusData : MonoBehaviour {

    public float priority = 0; //優先

    private int _hp;  //体力
    private int _hp_mx;
    private int _sp;  //魔力
    private int _sp_mx;

    private int _speed = 1; //移動力

    private int _atk; //攻撃力
    private int _def; //防御力
    private int _agi; //俊敏性

    private int _hun; //満腹度
    private int _hun_mx;

    //読み取りと書き込みを設定
    public int Hp
    {
        get { return _hp; }
    }

    public void SetHp(Object obj, int hp)
    {
        if(obj is AttackSource)
        {
            _hp = hp;
        }
    }

    public int HpMax
    {
        get { return _hp_mx; }
    }

    public int Sp
    {
        get { return _sp;}
    }

    public int SpMax
    {
        get { return _sp_mx; }
    }

    public int Atk
    {
        get { return _atk; }
    }

    public int Def
    {
        get { return _def; }
    }

    public int Agi
    {
        get { return _agi; }
    }

    public int Hun
    {
        get { return _hun; }
    }

    public int HunMax
    {
        get { return _hun; }
    }
    //書き込み
}
