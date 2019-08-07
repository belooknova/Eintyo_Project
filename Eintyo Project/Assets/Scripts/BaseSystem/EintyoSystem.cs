using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        public int addPercent = 100;

        [SerializeField]
        //定数
        public int addConstant = 0;

        [SerializeField]
        //stateに関する成功率
        public int percent = 100;

    }

    /*-----inspector拡張コード-----*/

#if UNITY_EDITOR

    //リスト内表示のコード
    [CustomPropertyDrawer(typeof(AddBuffSource))]
    public class BuffParamDrawer : PropertyDrawer
    {

        void OnEnable()
        {

        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                AbilityValue_DB AVD = Resources.Load("DataBase/AbilityValue", typeof(AbilityValue_DB)) as AbilityValue_DB;


                EditorGUIUtility.labelWidth = 30;
                position.height = EditorGUIUtility.singleLineHeight;

                var typeRect = new Rect(position) { y = position.y + 4 };
                var indexpRect = new Rect(position)
                {
                    y = typeRect.y + EditorGUIUtility.singleLineHeight + 16,
                    width = position.width * 0.5f
                };
                var labelRect = new Rect(indexpRect)
                {
                    y = indexpRect.y + EditorGUIUtility.singleLineHeight + 2,
                    width = 32,
                };
                var indexRect = new Rect(indexpRect)
                {
                    x = indexpRect.width + 48,
                    width = indexpRect.width - 32
                };
                var label2Rect = new Rect(labelRect)
                {
                    x = labelRect.x + labelRect.width + 1,
                    width = 32,
                };
                var label3Rect = new Rect(label2Rect)
                {
                    x = label2Rect.x + label2Rect.width + 1,
                    width = 32,
                };
                var label4Rect = new Rect(label3Rect)
                {
                    x = label3Rect.x + label3Rect.width + 1,
                    width = 64,
                };

                //------各プロパティーの SerializedProperty を求める------
                var typeProperty = property.FindPropertyRelative("type");
                var indexProperty = property.FindPropertyRelative("index");
                var aperProperty = property.FindPropertyRelative("addPercent");
                var aconProperty = property.FindPropertyRelative("addConstant");
                var percProperty = property.FindPropertyRelative("percent");

                //------各プロパティーの GUI を描画------

                //バーに表示する内容
                string[] BarLabel = { "能力値増加", "能力値減少", "状態付与", "状態解除" };
                typeProperty.intValue = EditorGUI.Popup(typeRect, typeProperty.intValue, BarLabel);

                if (typeProperty.intValue == 0)
                {
                    indexProperty.intValue = EditorGUI.Popup(indexpRect, indexProperty.intValue, AVD.GetAbyNameJ());
                    EditorGUIUtility.labelWidth = 40;
                    indexProperty.intValue = EditorGUI.IntField(indexRect, "能力ID", indexProperty.intValue);
                    EditorGUIUtility.labelWidth = 30;
                    EditorGUI.LabelField(label2Rect, "% +");
                    aperProperty.intValue = EditorGUI.IntField(labelRect, aperProperty.intValue);
                    aconProperty.intValue = EditorGUI.IntField(label3Rect, aconProperty.intValue);
                    EditorGUI.LabelField(label4Rect, "増加させる");
                }

                if (typeProperty.intValue == 1)
                {
                    indexProperty.intValue = EditorGUI.Popup(indexpRect, indexProperty.intValue, AVD.GetAbyNameJ());
                    EditorGUIUtility.labelWidth = 40;
                    indexProperty.intValue = EditorGUI.IntField(indexRect, "能力ID", indexProperty.intValue);
                    EditorGUIUtility.labelWidth = 30;
                    EditorGUI.LabelField(label2Rect, "% +");
                    aperProperty.intValue = EditorGUI.IntField(labelRect, aperProperty.intValue);
                    aconProperty.intValue = EditorGUI.IntField(label3Rect, aconProperty.intValue);
                    EditorGUI.LabelField(label4Rect, "減少させる");
                }


            }
        }
    }
#endif

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
        public int ItemId
        {
            protected set;
            get;
        }

        //収納されるStatusData
        public StatusData ParentStatus;

        //説明
        public string Description;

        //バフに使用するリストの要素番号
        protected List<int> listAddress = new List<int>();
        public List<int> ListAddress { get { return listAddress; } }

        //手に入れることを試みる
        public bool Attempt_Obtain_Event(StatusData statusData)
        {
            Debug.Log("アイテム入手を試みる");
            Item_DB item_db = GameManager.instance.ItemsList[ItemId];
            //listAddressに何か入っている場合はメソッド終了(想定上はあり得ないけど一応)
            if (listAddress.Count != 0)
            {
                Debug.Log("<color=red>[Obtain_Event]ERRER : 二回以上呼び出されています。</cloor>");
                return false;
            }

            //アイテム所持数の確認
            int pocket = statusData.GetAbilityData("pock");
            //最大アイテム所有数の確認
            int maxPocket = statusData.GetAbilityData("mpck");

            //アイテムがいっぱいの場合はスルーする。
            if (pocket >= maxPocket)
            {
                return false;
            }

            Obtain_Event(statusData);
            return true;
        }

        //手に入れたときのイベント
        protected void Obtain_Event(StatusData statusData)
        {

            Item_DB item_db = GameManager.instance.ItemsList[ItemId];
            ParentStatus = statusData; //収納されるステータスデータを記録しておく
            ParentStatus.AddItem_List(this); //アイテム保持リストにアイテムデータを挿入
            statusData.Add_AbyData("pock", item_db.Cost);

            foreach (AddBuffSource i in item_db.Passive)
            {
                //能力増減編
                if (i.type == 0 || i.type == 1)
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
            Item_DB item_db = GameManager.instance.ItemsList[ItemId];


            if (item_db.Passive != null || item_db.Passive.Count != 0) //パッシブスキルが設定されている。
            {
                Debug.Log("パッシブスキルが設定されている");
                if (listAddress.Count == 0)
                {
                    Debug.Log("[Obtain_Event]ERRER : listAddressに何も入っていません.");
                    return;
                }

                Debug.Log("バフを取り除くぞ");
                for (int i = 0; i < listAddress.Count; i++)
                {
                    ParentStatus.RemoveAbyValueList(item_db.Passive[i].index, listAddress[i]); //バフを取り除く
                }
            }

            Debug.Log("リムーブ");
            ParentStatus.RemoveItem_List(this);
            ParentStatus.Add_AbyData("pock", -item_db.Cost);
        }

    }

    //武器のメタデータ
    public class WeaponData : ItemData
    {
        public WeaponData(int weaponId) : base(weaponId)
        {
            Weapon_DB weapon_db = GameManager.instance.WeaponsList[weaponId];
            ItemId = weaponId;
            PossessionCost = weapon_db.Cost;
            Name = weapon_db.MataName;
        }

        public List<AddBuffSource> Arrange_Buffs = new List<AddBuffSource>(); //追加のバフ

    }

    /// <summary>
    /// ステートのメタデータ
    /// </summary>
    public class StateData
    {
        [SerializeField]
        private string _stateName; //状態の名前

        [SerializeField, Range(0, 10)]
        private int priority; //優先度

        [SerializeField]
        private int release_turn = 3;

        [SerializeField]
        public List<AddBuffSource> buffsParTurn = new List<AddBuffSource>();

        [SerializeField]
        public List<AddBuffSource> passive = new List<AddBuffSource>();

        /// <summary>
        /// 現在の経過ターン
        /// </summary>
        private int crrentTurn = 0;

        //--- --- --- --- --- -- 読み取り -- --- --- --- --- ---

        public string StateName
        {
            get { return _stateName; }
        }

        public int Priority
        {
            get { return priority; }
        }

        public int ReleaseTurn
        {
            get { return release_turn; }
        }

        //--- --- --- --- --- --- --- --- --- --- --- --- --- --- 

        //コンストラクタ
        public StateData(int id)
        {
            State_DB dB = GameManager.instance.StatusList[id];

            //コピー
            _stateName = dB.StateName;
            priority = dB.GetPrio;
            release_turn = dB.Getrelease;
            buffsParTurn = dB.buffsParTurn;
            passive = dB.passive;

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


}
