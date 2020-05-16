using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonTest : MonoBehaviour {
    private Button _button;

	// Use this for initialization
	void Start () {
        _button = GetComponent<Button>();
       

        //cuando este sobre boton haga....
        _button.onClick.AddListener(() =>
        {
            print("click");
        });
	}
	
}
