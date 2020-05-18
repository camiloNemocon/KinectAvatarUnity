using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoIzkRotacion : MonoBehaviour
{
    public GameObject ManoIzquierda;
    public GameObject objectWithBodySourcViewScipt;

    public int X = 180;
    public int Y = 90;
    public int Z = 0;

    // Update is called once per frame
    void Update()
    {
        //float HandAround = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().giroManoIzk;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHand = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        ManoIzquierda.transform.rotation = Quaternion.Slerp(transform.rotation, targetHand, Time.deltaTime * 5.0f);
    }
}
