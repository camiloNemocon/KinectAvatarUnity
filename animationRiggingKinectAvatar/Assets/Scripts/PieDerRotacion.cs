using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieDerRotacion : MonoBehaviour
{
    public GameObject tobilloDerecho;
    public GameObject objectWithBodySourcViewScipt;


    // Update is called once per frame
    void Update()
    {
        float AnkleAroundY = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().tobilloDerY;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetAnkle = Quaternion.Euler(0, AnkleAroundY, 90);
        

        // Dampen towards the target rotation
        tobilloDerecho.transform.rotation = Quaternion.Slerp(transform.rotation, targetAnkle, Time.deltaTime * 5.0f);
    }
}
