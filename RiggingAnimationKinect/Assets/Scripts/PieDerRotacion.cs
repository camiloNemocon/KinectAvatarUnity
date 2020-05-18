using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieDerRotacion : MonoBehaviour
{
    public GameObject tobilloDerecho;
    public GameObject objectWithBodySourcViewScipt;

    public int X = 180;
    public float Y = -90;
    public float Z = -90;

    // Update is called once per frame
    void Update()
    {
        if (objectWithBodySourcViewScipt.GetComponent<BodySourceView>().user)
        {
            Y = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().tobilloDerY;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetAnkle = Quaternion.Euler(X, Y, Z);
        

        // Dampen towards the target rotation
        tobilloDerecho.transform.rotation = Quaternion.Slerp(transform.rotation, targetAnkle, Time.deltaTime * 5.0f);
    }
}
