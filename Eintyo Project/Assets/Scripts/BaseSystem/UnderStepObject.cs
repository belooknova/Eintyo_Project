using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnderStepObject : MonoBehaviour {
    
    public Sprite texture;//見た目


	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //踏んだ時に実行するイベント
    public virtual void StepEvent(BaseObject baseObject)
    {

    }


}
