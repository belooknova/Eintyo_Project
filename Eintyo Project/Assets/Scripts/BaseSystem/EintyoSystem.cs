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
        //0: 通常statusの増加
        //1: 通常statusの減少
        //2: stateの付与
        //3: stateの解除
        //4: スキルの実行
        public int type = 0;

        [SerializeField]
        //対象の番号
        public int index = 0;

        [SerializeField]
        //倍率
        public int addPercent = 0;

        [SerializeField]
        //定数
        public int addConstant = 0;

        [SerializeField]
        //stateに関する成功率
        public int percent = 100;




    }

    //アイテムのメタデータ
    public class ItemData
    {
        public ItemData(int itemId)
        {
            Item_DB item_db = GameManager.instance.ItemsList[itemId];
            ItemId = itemId;
            PossessionCost = item_db.Cost;
            Name = item_db.MataName;

        }

        //アイテムの表示名
        public string Name;

        //アイテムの収納コスト
        public int PossessionCost = 1;

        //アイテムID
        private int ItemId;

        //収納されるStatusData
        public StatusData ParentStatus;

        //説明
        public string Description;

        //バフに使用するリストの要素番号
        private List<int> listAddress = new List<int>();
        public List<int> ListAddress { get { return listAddress; } }

        //リストの要素番号を設定する
        public void Set_Address(int index, int number)
        {
            try { listAddress[index] = number; }
            catch { Debug.Log("<color=red>[Set_Address]ERRER</color>"); }
        }

        //手に入れたときのイベント
        public void Obtain_Event()
        {
            Item_DB item_db = GameManager.instance.ItemsList[ItemId];
            int Len = item_db.Passive.Count;
            //パッシブリストが空の場合は終了
            if (Len == 0) return;
            //listAddressに何か入っている場合はメソッド終了(想定上はあり得ないけど一応)
            if (listAddress.Count != 0)
            {
                Debug.Log("<color=red>[Obtain_Event]ERRER : 二回以上呼び出されています。</cloor>");
                return;
            }

            foreach (AddBuffSource i in item_db.Passive)
            {
                //能力増減編
                if (i.type == 0 && i.type == 1)
                {
                    //リストのぶち込む値
                    int value = ParentStatus.GetAbilityData(i.index) * (int)(0.01f * i.addPercent) + i.addConstant;
                    int sign = 1;

                    if (i.type == 1) sign = -1; //減少の場合はマイナスに
                    int buffer = ParentStatus.AddAbyValueList(i.index, sign * value); //能力値リストに追加
                    
                    //一応エラーのための処理
                    if (buffer == -1)
                    {
                        Debug.Log("<color=red>[Obtain_Event]ERRER : bufferの値が-1のため強制的にメソッドを終了します。</color>");
                        return;
                    }

                    //アイテム内にどこにバフをかけたか記憶しておく
                    listAddress.Add(buffer);

                }


            }


        }

        //手放した時のイベント
        public void Unobtain_Event()
        {
            if (listAddress.Count == 0)
            {
                Debug.Log("<color=red>[Obtain_Event]ERRER : 入手時のイベントが呼び出されていません。</cloor>");
                return;
            }




        }

        //二つの変数をビットに変換する
        private int ValueToBit_buff(int abyIndex, int listIndex)
        {
            //abyIndex : 0~7  listIndex : 8~15
            int outbit = listIndex & abyIndex << 8;
            Debug.Log("[test]outbit : " + outbit);
            return outbit;
        }

        //ビットを二つの変数に変換する
        private void BitToValue_buff(int bit, out int abyIndex, int listIndex)
        {
            int bitMask = 0xF0; //1111 0000 

            listIndex = (bit &  bitMask) >> 4;
            abyIndex  = (bit & ~bitMask);

        }

    }
}
