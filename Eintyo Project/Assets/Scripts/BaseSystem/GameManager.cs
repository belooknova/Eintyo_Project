using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public StetusData player_stetus;

    public float spSize = 1;

    //お好きな生成したいダンジョン
    [SerializeField] private DgFactoryBase dgFactory;

    private DgBase dgProduct;

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
        if (Input.GetKey(KeyCode.Return)) {
            Debug.Log("Pressed Enter Key!");
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("スペースキー押した！");
		    dgProduct = dgFactory.create(25, 25);
        }
	}
}
