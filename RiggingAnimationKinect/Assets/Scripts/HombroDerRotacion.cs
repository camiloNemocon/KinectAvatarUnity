using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HombroDerRotacion : MonoBehaviour
{
    public GameObject HombroDerecho;
    public GameObject objectWithBodySourcViewScipt;

    public int X = 0;
    public int Y = 180;
    public float Z = 0;

    // Update is called once per frame
    void Update()
    {
        if (objectWithBodySourcViewScipt.GetComponent<BodySourceView>().user)
        {
            Z = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().DerHombroY;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetShoulder = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        HombroDerecho.transform.rotation = Quaternion.Slerp(transform.rotation, targetShoulder, Time.deltaTime * 5.0f);
    }
}
