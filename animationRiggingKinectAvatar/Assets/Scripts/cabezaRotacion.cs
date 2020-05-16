using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabezaRotacion : MonoBehaviour
{
    public GameObject cabeza;
    public GameObject objectWithBodySourcViewScipt;

    /*public int X = 0;
    public int Y = 90;
    public int Z = -90;*/


    // Update is called once per frame
    void Update()
    {
        float headAroundX = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().cabezaX;
        //float headAroundY = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().cabezaY;
        float headAroundZ = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().cabezaZ;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHead = Quaternion.Euler(headAroundX, 90, headAroundZ);
        //Quaternion targetHead = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        cabeza.transform.rotation = Quaternion.Slerp(transform.rotation, targetHead, Time.deltaTime * 5.0f);
    }
}
