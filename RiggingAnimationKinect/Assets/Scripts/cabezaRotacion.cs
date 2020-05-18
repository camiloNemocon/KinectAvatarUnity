using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabezaRotacion : MonoBehaviour
{
    public GameObject cabeza;
    public GameObject objectWithBodySourcViewScipt;

    public float X = 0;
    public float Y = 180;
    public float Z = 0;


    // Update is called once per frame
    void Update()
    {
        Z = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().cabezaZ;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHead = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        cabeza.transform.rotation = Quaternion.Slerp(transform.rotation, targetHead, Time.deltaTime * 5.0f);
    }
}
