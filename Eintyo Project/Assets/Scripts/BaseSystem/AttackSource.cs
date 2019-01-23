using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSource : MonoBehaviour{

    private StatusData _A;  //server
    private StatusData _B;  //receiver

    private int damageField;　//計算したダメージ

    




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
