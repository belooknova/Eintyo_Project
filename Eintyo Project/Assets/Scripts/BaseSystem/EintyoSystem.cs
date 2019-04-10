using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EintyoSystem
{

    //バフを与えるための情報
    [Serializable]
    public class AddBuffSource
    {
        [SerializeField]
        //0: 通常statusの増加/減少
        //1: stateの付与
        //2: stateの解除
        //3: スキルの実行
        private int type = 0;

        [SerializeField]
        //対象の番号
        private int index = 0;

        [SerializeField]
        //倍率
        private int addPercent = 0;

        [SerializeField]
        //定数
        private int addConstant = 0;

        [SerializeField]
        //stateに関する成功率
        private int percent = 100;




    }

    //アイテムのメタデータ
    public class ItemData
    {
        //アイテムの本当の名前
        private string mataName;

        //アイテムの表示名
        public string name;

        //アイテムの収納コスト
        public int possessionCost = 1;
    }
}
