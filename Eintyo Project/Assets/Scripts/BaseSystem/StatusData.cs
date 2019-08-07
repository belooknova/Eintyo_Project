using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EintyoSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StatusData : MonoBehaviour {

    [SerializeField]
    private int _speed = 1; //移動力

    [SerializeField]
    //private List<int> AbyValue = new List<int>{ 20, 20, 10, 10, 100, 100, 5, 5, 5, 10, 95, 10, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
    private List<List<int>> AbyValue = new List<List<int>> {
        new List<int>{20 },     /*体力(要素1以上は使用禁止)*/
        new List<int>{20 },     /*最大体力*/
        new List<int>{10 },     /*スペルポイント(要素1以上は使用禁止)*/
        new List<int>{10 },     /*最大スペルポイント*/
        new List<int>{100 },    /*空腹度(要素1以上は使用禁止)*/
        new List<int>{100 },    /*最大空腹度*/
        new List<int>{5 },      /*攻撃力*/
        new List<int>{5 },      /*防御力*/
        new List<int>{5 },      /*俊敏性*/
        new List<int>{10 },     /*回避*/
        new List<int>{95 },     /*命中率*/
        new List<int>{10 },     /*運*/
        new List<int>{1 },      /*火耐性*/
        new List<int>{1 },      /*水耐性*/
        new List<int>{1 },      /*木耐性*/
        new List<int>{1 },      /*金耐性*/
        new List<int>{1 },      /*土耐性*/
        new List<int>{1 },      /*火攻撃*/
        new List<int>{1 },      /*水攻撃*/
        new List<int>{1 },      /*木攻撃*/
        new List<int>{1 },      /*金攻撃*/
        new List<int>{1 },      /*土攻撃*/
        new List<int>{0 },      /*影制御*/
        new List<int>{0 },      /*アイテム所持数*/
        new List<int>{5 },      /*最大アイテム所持数*/
    };


    static private string[] AbyName = { "hp", "mhp", "sp", "msp", "hun", "mhn", "atk", "def", "agi", "eva", "acc", "luk", "firr", "wtrr", "wodr", "mtlr", "solr", "fira", "wtra", "woda", "mtla", "sola", "sdw", "pock", "mpck" };

    [SerializeField]
    private List<StateData> StateList = new List<StateData>(); //状態を保存しておくリスト

    [SerializeField]
    private List<ItemData> HoldItemList = new List<ItemData>(); //持っているアイテムを保存しておくリスト

    private bool Dead = false;

    private void Start()
    {
        //AbyName = GameManager.instance.AbyName; //読み込み
        //Debug.Log("AbyName.Length:" + AbyName.Length);
        //AbyValue = new int[AbyName.Length]; //要素数を設定する。


    }

    #region 読み取りと書き込みを設定
    /*
    public int Hp
    {
        get { return AbyValue[0][0]; }
    }
    public int HpMax
    {
        get { return AbyValue[1][0]; }
    }
    public int Sp
    {
        get { return AbyValue[2][0];}
    }
    public int SpMax
    {
        get { return AbyValue[3][0]; }
    }
    public int Hun
    {
        get { return AbyValue[4][0]; }
    }
    public int HunMax
    {
        get { return AbyValue[5][0]; }
    }
    public int Atk
    {
        get { return AbyValue[6][0]; }
    }
    public int Def
    {
        get { return AbyValue[7][0]; }
    }
    public int Agi
    {
        get { return AbyValue[8][0]; }
    }
    public int Eva
    {
        get { return AbyValue[9][0]; }
    }
    public int Acc
    {
        get { return AbyValue[10][0]; }
    }
    public int Luk
    {
        get { return AbyValue[11][0]; }
    }
    public int FireRe
    {
        get { return AbyValue[12][0]; }
    }
    public int WaterRe
    {
        get { return AbyValue[13][0]; }
    }
    public int WoodRe
    {
        get { return AbyValue[14][0]; }
    }
    public int MetalRe
    {
        get { return AbyValue[15][0]; }
    }
    public int SoilRe
    {
        get { return AbyValue[16][0]; }
    }
    public int FireAd
    {
        get { return AbyValue[17][0]; }
    }
    public int WaterAd
    {
        get { return AbyValue[18][0]; }
    }
    public int WoodAd
    {
        get { return AbyValue[19][0]; }
    }
    public int MetalAd
    {
        get { return AbyValue[20][0]; }
    }
    public int SoilAd
    {
        get { return AbyValue[21][0]; }
    }
    public int Sdw
    {
        get { return AbyValue[22][0]; }
    }
    */

    public List<ItemData> ItemsList
    {
        get { return HoldItemList; }
    }
    
    public int GetAbilityData(int index) //基本能力値を取得する
    {
        int value;
        try
        { value = AbyValue[index][0]; }
        catch
        { value = -1; }
        return value;
    }
    public int GetAbilityData(string label) //基本能力値を取得する(文字)
    {
        return GetAbilityData(GetAvilityId(label));
    }
    public int GetSumAbility(int index) //能力値の合計を取得する
    {
        int sum = 0;
        List<int> vs;
        try
        {
            vs = AbyValue[index];
            for (int i = 0; i < vs.Count; i++) { sum += vs[i]; }
        } catch { sum = -1; }
        return sum;
    }
    public int GetSumAbility(string label) //能力値の合計を取得する(文字)
    {
        return GetAbilityData(GetAvilityId(label));
    }
    public int GetAvilityId(string label)
    {
        int outValue = 0;
        for(int i=0; i<AbyName.Length; i++)
        {
            if (AbyName[i] == label)
            {
                outValue = i;
            }
        }
        return outValue;
    }

    public void Set_AbyData(int index, int index2, int value) //能力値に値を代入
    {
        AbyValue[index][index2] = value;
    }
    public void Set_AbyData(int index, int value) //基本能力値を代入
    {
        Set_AbyData(index, 0, value);
    }
    public void Set_AbyData(string label, int value) //基本能力値を代入(文字)
    {
        Set_AbyData(GetAvilityId(label), value);
    }
    public void Add_AbyData(int index, int index2, int value) //能力値に加算する
    {
        AbyValue[index][index2] += value;
    } 
    public void Add_AbyData(int index, int value) //基本能力値に加算する
    {
        Add_AbyData(index, 0, value);

    } 
    public void Add_AbyData(string label, int value) //基本能力値に加算する(文字)
    {
        Add_AbyData(GetAvilityId(label), value);
    }

    #endregion

    public int AddAbyValueList(int index, int value) //能力値リストに追加能力をAddる
    {
        int outindex = -1;
        try
        {
            AbyValue[index].Add(value);
            outindex = AbyValue[index].Count - 1;
            Debug.Log("[test]outindex : "+outindex);
        }
        catch
        {
            Debug.Log("[AddAbyValueList]ERRER : Out Of Range(AbyValue)");
        }
            return outindex;
    }
    public void RemoveAbyValueList(int index, int index2) //能力値リストから能力をRemoveる
    {
        if (index2 <= 1 || index2 > AbyValue[index].Count - 1) return;
        Debug.Log("[test]index2 : "+index2);
        AbyValue[index].RemoveAt(index2);
    }

    //アイテムリストに挿入
    public void AddItem_List(ItemData data)
    {
        if (HoldItemList.Count >= GetAbilityData("mpck"))
        {
            Debug.Log("アイテムがいっぱいです");
            return;
        }
        HoldItemList.Add(data);
    }

    //アイテムリストからアイテムを除外する
    public void RemoveItem_List(ItemData data)
    {
        if (!HoldItemList.Contains(data))
        {
            Debug.Log("アイテムがない");
            return;
        }
        HoldItemList.Remove(data);
    }

    //辞書を取得する
    public Dictionary<string, int> GetDict()
    {
        Dictionary<string, int> outdict = new Dictionary<string, int>();

        for (int i=0; i < AbyValue.Count; i++)
        {
            outdict.Add(AbyName[i], GetSumAbility(i));
        }

        return outdict;
    }

    //ダメージを受ける
    public void Recieve_Damage(int value)
    {
        AbyValue[0][0] -= value;
        if (AbyValue[0][0] <= 0)
        {
            AbyValue[0][0] = 0;
            Dead = true;
        }
    }

    //状態を与えられる
    public void Recieve_State(int index)
    {
        StateData stateData = new StateData(index);

        if (!StateList.Contains(stateData))
        {
            StateList.Add(stateData);
        }
    }

    //能力値の空きを調べて、
    //public void 


#if UNITY_EDITOR

    /*-----inspector拡張コード-----*/
    [CustomEditor(typeof(StatusData))]
    public class StatusData_Editor : Editor
    {
        bool isBaseStatus = true;
        bool isStatePercent = false;
        bool isAttribute = false;
        bool isItems = false;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            //return;

            StatusData SD = target as StatusData;

            //--------------------------------------------

            if (isBaseStatus = EditorGUILayout.Foldout(isBaseStatus, "基本能力値"))
            {

                EditorGUILayout.LabelField("【ヒットポイント(現在/最大)】");
                EditorGUILayout.BeginHorizontal();
                SD.AbyValue[0][0] = EditorGUILayout.IntField(SD.AbyValue[0][0], GUILayout.Width(48));
                SD.AbyValue[1][0] = EditorGUILayout.IntField(SD.AbyValue[1][0], GUILayout.Width(48));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("【スペルポイント(現在/最大)】");
                EditorGUILayout.BeginHorizontal();
                SD.AbyValue[2][0] = EditorGUILayout.IntField(SD.AbyValue[2][0], GUILayout.Width(48));
                SD.AbyValue[3][0] = EditorGUILayout.IntField(SD.AbyValue[3][0], GUILayout.Width(48));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("【空腹ポイント(現在/最大)】");
                EditorGUILayout.BeginHorizontal();
                SD.AbyValue[4][0] = EditorGUILayout.IntField(SD.AbyValue[4][0], GUILayout.Width(48));
                SD.AbyValue[5][0] = EditorGUILayout.IntField(SD.AbyValue[5][0], GUILayout.Width(48));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                SD.AbyValue[6][0] = EditorGUILayout.IntField("【攻撃力】", SD.AbyValue[6][0]);
                SD.AbyValue[7][0] = EditorGUILayout.IntField("【防御力】", SD.AbyValue[7][0]);
                SD.AbyValue[8][0] = EditorGUILayout.IntField("【俊敏性】", SD.AbyValue[8][0]);
                SD.AbyValue[9][0] = EditorGUILayout.IntField("【回避率】", SD.AbyValue[9][0]);
                SD.AbyValue[10][0] = EditorGUILayout.IntField("【命中率】", SD.AbyValue[10][0]);
                SD.AbyValue[11][0] = EditorGUILayout.IntField("【運】", SD.AbyValue[11][0]);
                SD.AbyValue[21][0] = EditorGUILayout.IntField("【影制御】", SD.AbyValue[21][0]);

            }

            if (isAttribute = EditorGUILayout.Foldout(isAttribute, "基本属性値"))
            {
                SD.AbyValue[12][0] = EditorGUILayout.IntField("【火耐性】", SD.AbyValue[12][0]);
                SD.AbyValue[13][0] = EditorGUILayout.IntField("【水耐性】", SD.AbyValue[13][0]);
                SD.AbyValue[14][0] = EditorGUILayout.IntField("【木耐性】", SD.AbyValue[14][0]);
                SD.AbyValue[15][0] = EditorGUILayout.IntField("【金耐性】", SD.AbyValue[15][0]);
                SD.AbyValue[16][0] = EditorGUILayout.IntField("【土耐性】", SD.AbyValue[16][0]);
                EditorGUILayout.Space();
                SD.AbyValue[17][0] = EditorGUILayout.IntField("【火攻撃】", SD.AbyValue[17][0]);
                SD.AbyValue[18][0] = EditorGUILayout.IntField("【水攻撃】", SD.AbyValue[18][0]);
                SD.AbyValue[19][0] = EditorGUILayout.IntField("【木攻撃】", SD.AbyValue[19][0]);
                SD.AbyValue[20][0] = EditorGUILayout.IntField("【金攻撃】", SD.AbyValue[20][0]);
                SD.AbyValue[21][0] = EditorGUILayout.IntField("【土攻撃】", SD.AbyValue[21][0]);
            }

            if (isStatePercent = EditorGUILayout.Foldout(isStatePercent,"状態"))
            {
                EditorGUILayout.LabelField("現在の状態");
                StateDataManager STD = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;
                foreach (StateData s in SD.StateList)
                {
                    EditorGUILayout.LabelField("---------------");
                    EditorGUILayout.LabelField("["+ s.StateName +"]");
                    EditorGUILayout.LabelField("説明：");
                }

            }

            if(isItems = EditorGUILayout.Foldout(isItems, "所持しているアイテム"))
            {
                EditorGUILayout.LabelField("最大アイテム所持数: " + SD.GetSumAbility("mpck"));
                EditorGUILayout.LabelField("--- --- --- --- --- --- ---");

                foreach(ItemData i in SD.HoldItemList)
                {
                    EditorGUILayout.LabelField("〇["+ i.ItemId +"]" + i.Name);
                }
            }

            base.OnInspectorGUI();

        }
    }
#endif
}
