using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieIzkRotacion : MonoBehaviour
{
    public GameObject tobilloIzkierdo;
    public GameObject objectWithBodySourcViewScipt;

    public int X = 0;
    public float Y = 90;
    public float Z = -90;

    // Update is called once per frame
    void Update()
    {
        if (objectWithBodySourcViewScipt.GetComponent<BodySourceView>().user)
        {
            Y = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().tobilloIzkY;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetAnkle = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        tobilloIzkierdo.transform.rotation = Quaternion.Slerp(transform.rotation, targetAnkle, Time.deltaTime * 5.0f);
    }
}
