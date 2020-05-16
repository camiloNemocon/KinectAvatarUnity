using UnityEngine;
using System.Collections;

public class KinectUIWaitOverButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.childCount == 0) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<KinectUIWaitOverButton>();
        }
	}
}
