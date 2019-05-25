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
            if(dgProduct != null){
                Debug.Log("前のマップがSceneに残ってるんで消しますね");
                dgProduct.RemoveTileGameObject();
                dgProduct = null;
            }
		    dgProduct = dgFactory.create(35, 35);
        }
        if(Input.GetKeyDown(KeyCode.N)){
            Debug.Log("n押されたので，マップ消します");
            if(dgProduct == null){
                Debug.Log("nullやないか～～い！");
            } else {
                dgProduct.RemoveTileGameObject();
                dgProduct = null;
            }
        }
	}
}
