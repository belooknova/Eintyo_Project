using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseObject {

    // Use this for initialization
    protected override void Start () {
        base.Start();
		
	}

    public override void TurnUpDate()
    {
        base.TurnUpDate();
        int hor = Random.Range(-1, 1);
        int ver = Random.Range(-1, 1);

        //移動を試みる
        AttemptMove(hor, ver);
    }

    public override void Next_status()
    {
        base.Next_status();
        //GameManager.instance.EnemyStateInc();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
