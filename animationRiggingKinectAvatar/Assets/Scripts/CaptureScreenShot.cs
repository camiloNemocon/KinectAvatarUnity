using UnityEngine;
using System.Collections;

public class CaptureScreenShot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void CaptureScreenshot () {
        ScreenCapture.CaptureScreenshot("Screenhot");
	}
}
