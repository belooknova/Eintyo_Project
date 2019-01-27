using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FormulePurser;

public class AttackSource : MonoBehaviour{

    private StatusData _A;  //server
    private StatusData _B;  //receiver

    private int damageField;　//計算したダメージ

    private void Start()
    {
        StatusData A = GetComponent<StatusData>();

        Purser p = new Purser(A, A, "((1 + a.def)* 2) * (b.atk + 12)");
        //Purser p = new Purser(A, A, "34+67*78");

        Debug.Log(p.Eval());
        //string s = "a.atk+56*34+(2 * a.hun)";
        //Debug.Log(s.IndexOf("a.atk"));
        //Debug.Log(p.Conversion_Status(s));

    }




    /// 攻撃値を登録する
    void Setdamage(int　num)
    {
        damageField = num;
    }

    ///サーバーを設定する
    void SetServer(StatusData A)
    {
        _A = A;
    }

    ///レシーバーを設定する
    void SetReceiver(StatusData B)
    {
        _B = B;
    }

    void Addbuff(string state, float percent)
    {

    }



}
