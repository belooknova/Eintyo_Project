using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EintyoSystem;

public class ItemObject : UnderStepObject {

    [SerializeField]
    private int itemID = 0; //アイテムのID
    private ItemData itemData; //アイテムの情報

    //踏んだ時に実行するイベント
    public override void StepEvent(BaseObject baseObject)
    {
        ChackData(); //アイテムのデータが入っているのかチェックする。入っていない場合はブランクアイテムのデータを入れる

        StatusData status = baseObject.MyStatus;

        //アイテム所持数の確認
        int pocket = status.ItemsList.Count;
        //最大アイテム所有数の確認
        int maxPocket = status.GetAvilityData("pock");

        //アイテムがいっぱいの場合はスルーする。
        if (pocket >= maxPocket)
        {
            //アイテム踏んだログ

            return;
        }

        status.ItemsList.Add(itemData); //アイテム保持リストにアイテムデータを挿入
        itemData.ParentStatus = status; //収納されるステータスデータを記録しておく
        itemData.Obtain_Event(); //入手時のイベントを実行
        Destroy(gameObject); //このオブジェクトを消す

    }

    //アイテム生成
    public void CreateItemeData(int itemID)
    {
        ItemData data = new ItemData(itemID);
        itemData = data;
    }

    //アイテムデータのチェック
    private void ChackData()
    {
        if (itemData == null)
        {
            ItemData data = new ItemData(0);
            itemData = data;
        }
    }





}
