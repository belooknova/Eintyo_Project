using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //アクセスできる変数
    public static GameManager instance = null;
    public StatusData player_stetus;
    public float spSize = 1;
    public Player player; //プレイヤーのクラス
    public List<GameObject> ActorList = new List<GameObject>();


    private string stateTrans = "Input";

    [SerializeField, Header("DB Setting")]
    private StateDataManager BD_State;

    [SerializeField]
    private SkillDataManager BD_skills;

    //-----敵関係の変数-----
    List<Enemy> EnemyList = new List<Enemy>(); //今ステージに居る敵を管理するリスト
    private int enemies_state = 0; //要らないかも？

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        //◇入力状態
        if (stateTrans == "Input")
        {
            int horizontal = 0; //-1: 左移動, 1: 右移動
            int vertical = 0; //-1: 下移動, 1: 上移動
                              //---移動---
            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");


            player.Movekey_Set(horizontal, vertical);
            if (horizontal != 0 || vertical != 0)
            {
                //player.Movekey_Set(horizontal, vertical);
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
            for (int i=0; i<EnemyList.Count; i++)
            {
                EnemyList[i].TurnUpDate();
            }
            
            //stateTrans = "Enemy_Wait";
            stateTrans = "TurnEnd";
        }

        /*
        //廃止しました・・・(何か起きたときのために残しておきます)
        //◇敵の行動終わり待ち
        if (stateTrans == "Enemy_Wait")
        {
            Debug.Log(enemies_state+"    "+EnemyList.Count);
            if (EnemyList.Count <= enemies_state)
            {

                stateTrans = "TurnEnd";
            }
        }
        */

        //◇ターン終了
        if(stateTrans == "TurnEnd")
        {
            EnemyStateClear();
            stateTrans = "Input";

        }


    }

    //敵リストを更新する
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


}

