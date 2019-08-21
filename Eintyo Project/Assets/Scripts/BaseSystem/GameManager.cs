using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EintyoSystem;

public class GameManager : MonoBehaviour {

    //アクセスできる変数
    public static GameManager instance = null;
    public float spSize = 1.28f;
    public Player player; //プレイヤーのクラス

    //----用語----
    [HideInInspector]
    public string[] AbyNameJ;
    [HideInInspector]
    public string[] AbyName;
    private string[] _typeList;

    public string[] TypeList { get { return _typeList; } }

    //----データベース----
    [HideInInspector]
    public List<Skill_DB> SkillsList { get; private set; }
    [HideInInspector]
    public List<State_DB> StatusList { get; private set; }
    [HideInInspector]
    public List<Item_DB> ItemsList { get; private set; }
    [HideInInspector]
    public List<Weapon_DB> WeaponsList { get; private set; }

    [SerializeField]
    private string stateTurns = "Ready";
    private int cullentIndex_StateTrans = 0;

    private StateDataManager BD_State;
    private SkillDataManager BD_skills;

    //-----敵関係の変数-----
    /// <summary>
    /// 現在ステージに存在している敵を格納する
    /// </summary>
    List<Enemy> EnemyList = new List<Enemy>(); //今ステージに居る敵を管理するリスト

    /// <summary>
    /// 現在ステージ内に居るアクターを格納する
    /// </summary>
    public List<GameObject> actorList = new List<GameObject>();
    public LayerMask layerMask_Actor;


    private int enemies_state = 0; //要らないかも？

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //collDataBase();//データベースを呼び出す
    }

    // Use this for initialization
    void Start () {
        
        CollDataBase();
    }

    ItemData Idata;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ItemData data = new ItemData(0);
            Idata = data;
            data.Attempt_Obtain_Event(player.MyStatus);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Idata.Unobtain_Event();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Buttle_Input_Transition();
        Transition_State_Buttle();
    }
    
    /// <summary>
    /// 自ターン・入力
    /// </summary>
    void Input_StateTruns()
    {
        int horizontal = 0; //-1: 左移動, 1: 右移動
        int vertical = 0;   //-1: 下移動, 1: 上移動
                            //---移動---
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        //---ボタンの設定--- --- --- --- --- --- --- --- --- --- ---
        bool WALK = (horizontal != 0 || vertical != 0);
        player.Set_Walk_Botton(WALK);

        bool HOLD = Input.GetButton("Hold");
        player.Set_Hold_Botton(HOLD);

        bool ATTACK = Input.GetButtonDown("OK");
        player.Set_Attack_Botton(ATTACK);
        //--- --- --- --- --- --- --- --- --- --- --- --- --- --- ---

        //Debug.Log("hor: " + horizontal + "  ver: " + vertical);

        //移動する
        if (WALK)
        {
            player.Movekey_Set(horizontal, vertical);

            //Debug.Log("hor: " + horizontal + "  ver: " + vertical);
            stateTurns = "Player";
        }

        //攻撃する
        if (ATTACK)
        {
            stateTurns = "Player";
        }
    }

    /// <summary>
    /// 戦闘時の状態遷移
    /// </summary>
    void Transition_State_Buttle()
    {
        if (stateTurns == "Action")
        {
            bool flag_Exec_TUD = true;

            //actorListの探索を終えた
            if (actorList.Count-1 < cullentIndex_StateTrans)
            {
                stateTurns = "Ready";
                flag_Exec_TUD = false;
            }

            //プレイヤーの番が来たら別処理
            if (flag_Exec_TUD)
            {
                if (actorList[cullentIndex_StateTrans].GetComponent<Player>() == player)
                {
                    stateTurns = "Input";
                    flag_Exec_TUD = false;
                }
            }
            if (flag_Exec_TUD) Exec_TurnUpDate();

        }

        //ループの準備
        if (stateTurns == "Ready")
        {
            //ActorList_Update();
            cullentIndex_StateTrans = 0;
            stateTurns = "Action";
            //stateTurns = "Input";
        }


        if (stateTurns == "Input")
        {
            Input_StateTruns();
        }
        
        if (stateTurns == "Player")
        {
            Exec_TurnUpDate();
            stateTurns = "Action";
            //stateTurns = "Input";
        }
    }

    /// <summary>
    /// TurnUpDateを実行(処理をまとめるためにメソッド化)
    /// </summary>
    void Exec_TurnUpDate()
    {
        var I_TrunUpDate = actorList[cullentIndex_StateTrans].GetComponent<Interface_trunUpDate>();

        //実行処理
        if (I_TrunUpDate != null)
        {
            I_TrunUpDate.TurnUpDate(); //ターンを実行
        }
        cullentIndex_StateTrans++;
    }

    /// <summary>
    /// アクターリストを更新する(探索を行う)
    /// *やや重いので乱用禁止(毎フレーム呼び出すのはやめよう)
    /// </summary>
    public void ActorList_Update()
    {
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            if(CompareLayer(layerMask_Actor, obj.layer))
            {
                ActorList_Add(obj);
            }
        }
    }

    /// <summary>
    /// アクターリストに指定したオブジェクトを追加する
    /// (Interface_trunUpDateがついたオブジェクトのstart()に記述すること)
    /// </summary>
    /// <param name="gameObject"></param>
    public void ActorList_Add(GameObject gameObject)
    {
        //アクターリストに追加していいのか判定する
        if (gameObject.GetComponent<Interface_trunUpDate>() == null) return;

        if (!actorList.Contains(gameObject))
        {
            actorList.Add(gameObject);
        }
    }


    /// <summary>
    /// LayerMaskに対象のLayerが含まれているかチェックする
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    bool CompareLayer(LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    //ステート(バフ)データベースを取得する
    public StateDataManager GetDB_States()
    {
        return this.BD_State;
    }

    //スキルデータベースを取得する
    public SkillDataManager GetDB_Skills()
    {
        return this.BD_skills;
    }

    //--- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
    SkillDataManager DataManager;
    Skill_DB skill_DB;

    //データベースの読み込み
    private void CollDataBase()
    {
        SkillDataManager SKDM = Resources.Load("DataBase/SkillDataManager", typeof(SkillDataManager)) as SkillDataManager;
        StateDataManager STDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;
        type_DB TDB = Resources.Load("DataBase/Type", typeof(type_DB)) as type_DB;
        ItemDataManager ITDM = Resources.Load("DataBase/ItemDataManager", typeof(ItemDataManager)) as ItemDataManager;


        _typeList = TDB.Attributes_Array;
        SkillsList = SKDM.GetSkillLists();
        StatusList = STDM.GetStateLists();
        ItemsList = ITDM.GetItemLists();

        //skill_DB = DataManager.GetSkillLists()[0];
        
    }

    //

}

