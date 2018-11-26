using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject {

    // Use this for initialization
    protected override void Start() {
        base.Start();
        priority = -1;
        GameManager.instance.player = this;
	}

    private void OnDisable()
    {
        GameManager.instance.player_stetus = stetus;
    }

    // Update is called once per frame
    void Update () {
        
        

    

    }


}
