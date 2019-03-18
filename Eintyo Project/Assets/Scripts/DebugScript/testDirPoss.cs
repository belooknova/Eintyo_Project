using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDirPoss : MonoBehaviour {

    public GameObject target;

    private BaseObject Base;
    private Vector3 vector;

	// Use this for initialization
	void Start () {
        Base = target.GetComponent<BaseObject>();
        vector = new Vector3(Base.DirX, Base.DirY, 0);
	}
	
	// Update is called once per frame
	void Update () {
        vector = new Vector3(Base.DirX, Base.DirY, 0);
        transform.position = target.transform.position + vector;

        //Debug.Log((int)Input.GetAxisRaw("Horizontal"));
        //Debug.Log((int)Input.GetAxisRaw("Vertical"));
    }
}
