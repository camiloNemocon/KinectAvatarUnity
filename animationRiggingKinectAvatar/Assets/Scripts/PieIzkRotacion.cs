using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieIzkRotacion : MonoBehaviour
{
    public GameObject tobilloIzkierdo;
    public GameObject objectWithBodySourcViewScipt;


    // Update is called once per frame
    void Update()
    {
        float AnkleAroundY = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().tobilloIzkY;


        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetAnkle = Quaternion.Euler(0, AnkleAroundY, -90);

        // Dampen towards the target rotation
        tobilloIzkierdo.transform.rotation = Quaternion.Slerp(transform.rotation, targetAnkle, Time.deltaTime * 5.0f);
    }
}
