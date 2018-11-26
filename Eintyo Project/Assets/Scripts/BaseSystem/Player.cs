using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject {

    // Use this for initialization
    protected override void Start() {
        base.Start();
	}

    private void OnDisable()
    {
        GameManager.instance.player_stetus = stetus;
    }

    // Update is called once per frame
    void Update () {
        //if (!GameManager.instance.playersTurn)
        //    return;

        float size = GameManager.instance.spSize;

        int horizontal = 0; //-1: 左移動, 1: 右移動
        int vertical = 0; //-1: 下移動, 1: 上移動

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        

        /*
        if (horizontal != 0)
        {
            vertical = 0;
        }
        */

        if (horizontal != 0 || vertical != 0)
        {
            //Wall: ジェネリックパラメーター<T>に渡す型引数
            //Playerの場合はWall以外判定する必要はない
            
            AttemptMove(horizontal, vertical);
        }

    }


}
