using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaderaRotacion : MonoBehaviour
{
    public GameObject cadera;
    public GameObject objectWithBodySourcViewScipt;

    /*public int X = 0;
    public int Y = 90;
    public int Z = -90;*/

    // Update is called once per frame
    void Update()
    {
        float SpineBaseAround = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().caderaSpineBase;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetSpineBase = Quaternion.Euler(0, SpineBaseAround, -90);
       // Quaternion targetSpineBase = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        cadera.transform.rotation = Quaternion.Slerp(transform.rotation, targetSpineBase, Time.deltaTime * 5.0f);
    }
}
