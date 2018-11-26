using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Input : MonoBehaviour {

    GameManager GM;

	// Use this for initialization
	void Start () {
        GM = GameManager.instance;

	}
	
	// Update is called once per frame
	void Update () {

       
    }


    void KeySecen()
    {
        int horizontal = 0; //-1: 左移動, 1: 右移動
        int vertical = 0; //-1: 下移動, 1: 上移動
        //---移動---
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {



        }

        //-------

    }
}
