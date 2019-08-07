using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EintyoSystem;

public class GameManager : MonoBehaviour {

    //アクセスできる変数
    public static GameManager instance = null;
    public float spSize = 1.28f;
    public Player player; //プレイヤーのクラス
    public List<GameObject> ActorList = new List<GameObject>();

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

    private string stateTrans = "Input";
    private StateDataManager BD_State;
    private SkillDataManager BD_skills;

    //-----敵関係の変数-----
    List<Enemy> EnemyList = new List<Enemy>(); //今ステージに居る敵を管理するリスト
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
        Buttle_Input_Transition();

    }

    void Buttle_Input_Transition()
    {
        //◇入力状態
        if (stateTrans == "Input")
        {
            int horizontal = 0; //-1: 左移動, 1: 右移動
            int vertical = 0; //-1: 下移動, 1: 上移動
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
                stateTrans = "Player";
            }

            //攻撃する
            if (ATTACK)
            {
                stateTrans = "Player";
            }
        }

        //◇プレイヤー行動
        if (stateTrans == "Player")
        {
            player.TurnUpDate();
            stateTrans = "Enemy";
        }

        //◇敵行動
        if (stateTrans == "Enemy")
        {
            //リストに入っている敵の行動を実行
            EnemyListUpdate();
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].TurnUpDate();
            }

            //stateTrans = "Enemy_Wait";
            stateTrans = "TurnEnd";


        }

        //◇ターン終了
        if (stateTrans == "TurnEnd")
        {
            EnemyStateClear();
            stateTrans = "Input";

        }
    }

    //敵リストを更新する
    /// <summary>
    /// 敵リストを更新する
    /// </summary>
    void EnemyListUpdate()
    {
        EnemyList.Clear();
        for (int i = 0; i < ActorList.Count; i++)
        {
            if (ActorList[i].gameObject.GetComponent<Player>() == null)
            {
                EnemyList.Add(ActorList[i].gameObject.GetComponent<Enemy>());

            }

        }
    }

    //enemy_stateに1足すだけ
    public void EnemyStateInc()
    {
        this.enemies_state++;
    }

    //enemy_stateを初期化するだけ
    /// <summary>
    /// enemy_stateを初期化
    /// </summary>
    void EnemyStateClear()
    {
        this.enemies_state = 0;
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

