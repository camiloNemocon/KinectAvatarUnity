using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoRotacion : MonoBehaviour
{
    public GameObject Torso;
    public GameObject objectWithBodySourcViewScipt;

    /*public int X = 0;
    public int Y = 90;
    public int Z = -90;*/
    
    // Update is called once per frame
    void Update()
    {
        float SpineAround = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().torsoPecho;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetSpine = Quaternion.Euler(0, SpineAround, -90);

        // Dampen towards the target rotation
        Torso.transform.rotation = Quaternion.Slerp(transform.rotation, targetSpine, Time.deltaTime * 5.0f);
    }
}
