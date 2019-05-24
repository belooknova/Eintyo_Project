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


        Debug.Log("アイテムを踏んだ");
        if (status.GetAbilityData("mpck") <= status.GetAbilityData("pock"))
        {
            Debug.Log("アイテムがいっぱいだった");
            return;
        }
        if (itemData.Attempt_Obtain_Event(status)) //入手時のイベントを実行
        {
            //ログ
        }
        Destroy(gameObject); //このオブジェクトを消す

    }

    //アイテム生成
    public void CreateItemeData(int itemID)
    {
        ItemData data = new ItemData(itemID);
        itemData = data;
        this.itemID = itemID;
    }

    //アイテムデータのチェック
    private void ChackData()
    {
        if (itemData == null)
        {
            CreateItemeData(itemID);
        }
    }





}
