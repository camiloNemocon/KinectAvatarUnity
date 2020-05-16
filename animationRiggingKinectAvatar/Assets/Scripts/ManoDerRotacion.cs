using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoDerRotacion : MonoBehaviour
{
    public GameObject ManoDerecho;
    public GameObject objectWithBodySourcViewScipt;

   /* public int X = 0;
    public int Y = -90;
    public int Z = 0;*/

    // Update is called once per frame
    void Update()
    {
        float HandAround = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().giroManoDer;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHand = Quaternion.Euler(0, HandAround, 0);

        // Dampen towards the target rotation
        ManoDerecho.transform.rotation = Quaternion.Slerp(transform.rotation, targetHand, Time.deltaTime * 5.0f);
    }
}
