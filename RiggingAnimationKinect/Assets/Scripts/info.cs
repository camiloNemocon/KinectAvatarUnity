using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class info : MonoBehaviour
{
    public Text dato;

    public GameObject ob;


    // Update is called once per frame
    void Update()
    {
        float X = ob.transform.rotation.x;
        float Y = ob.transform.rotation.y;
        float Z = ob.transform.rotation.z;

        dato.text = X.ToString("F2")+"   "+ Y.ToString("F2") + "   " + Z.ToString("F2");

    }
}
