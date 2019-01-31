using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject {
    int hor = 0 , ver = 0;
    

    // Use this for initialization
    protected override void Start() {
        base.Start();
        GetComponent<StatusData>().priority = -1;
        GameManager.instance.player = this;
        //animatorの設定
        

	}

    protected override void Update()
    {
        /*
        //anim.SetBool("Moving", isMoving);
        if (hor == 0 && ver == 0)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }

        anim.SetFloat("dir_x",hor);
        anim.SetFloat("dir_y", ver);
        anim.SetInteger("dir", dir);
        */    
        base.Update();
    }

    //ターン用UpDate
    public override void TurnUpDate()
    {
        base.TurnUpDate();
        if (hor != 0 || ver != 0)
        {
            //移動できるかどうか調べる
            AttemptMove(hor, ver);
        }
        

    }

    public override void Next_status()
    {
        base.Next_status();
        //GameManager.instance.stateTrans = "Enemy";
        
    }

    private void OnDisable()
    {
        GameManager.instance.player_stetus = GetStatus();
    }

    // horizontal入力とvertical入力の設定
    public void Movekey_Set(int hor, int ver)
    {
        this.hor = hor;
        this.ver = ver;
    }
}
