using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	// Use this for initialization
    float startPosZ;
	void Start () {
        startPosZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = new Vector3(Input.mousePosition.x, Input.mousePosition.y, startPosZ -Camera.main.transform.position.z );
		v = Camera.main.ScreenToWorldPoint(v);
//		Debug.Log(Camera.main.ScreenToWorldPoint(v));
		float z = transform.position.z;

		if(Input.GetButtonDown("Fire1")){
			z += .5f;
		}
		if(Input.GetButtonUp("Fire1")){
			z -= .5f;
		}
		transform.position = new Vector3(v.x,v.y,z);
	}
}
