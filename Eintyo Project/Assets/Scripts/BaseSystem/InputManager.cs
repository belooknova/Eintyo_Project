using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour {

    [Flags]
    private enum InputFlag
    {
        UP=1<<0,
        CANCEL=1<<1,
        MENU=1<<2,

    }

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
