using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : BaseObject {

    [Flags]
    private enum InputFlag
    {
        HOLD = 1 << 0,
        ATTACK = 1 << 1,
        WALK = 1<< 2,
    }

    int hor = 0, ver = 0;
    InputFlag input = 0;


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

        //移動
        if((input & InputFlag.WALK) == InputFlag.WALK)
        {
            if (!((input & InputFlag.HOLD)==InputFlag.HOLD))
            {
                AttemptMove();
                return;
            }
        }

        //通常攻撃
        if ((input & InputFlag.ATTACK) == InputFlag.ATTACK)
        {
            AttemptSimpleAttack();
        }




    }

    public override void Next_status()
    {
        base.Next_status();
        //GameManager.instance.stateTrans = "Enemy";
        
    }

    private void OnDisable()
    {
        GameManager.instance.player = this;
    }

    // horizontal入力とvertical入力の設定
    public void Movekey_Set(int hor, int ver)
    {
        dirx = hor;
        diry = ver;
    }

    //---キー入力の設定---

    //ホールドボタン
    public void Set_Hold_Botton(bool flag)
    {
        if (flag)
        { input |= InputFlag.HOLD; }
        else
        { input &= ~InputFlag.HOLD; } 
    }

    //攻撃ボタン
    public void Set_Attack_Botton(bool flag)
    {
        if (flag)
        { input |= InputFlag.ATTACK; }
        else
        { input &= ~InputFlag.ATTACK; }
    }

    //攻撃ボタン
    public void Set_Walk_Botton(bool flag)
    {
        if (flag)
        { input |= InputFlag.WALK; }
        else
        { input &= ~InputFlag.WALK; }
    }

}
