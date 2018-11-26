using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public StetusData player_stetus;
    public float spSize = 1;
    public Player player; //プレイヤーのクラス
    public List<GameObject> ActorList = new List<GameObject>();


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
    void Update () {
		


	}


    void PlayerMoveSet(int hor,int ver)
    {

    }

    

}
