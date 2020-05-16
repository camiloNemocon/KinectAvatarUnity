using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using UnityEngine.UI;

public class BodySourceView : MonoBehaviour
{
    //public Material BoneMaterial;

    private Vector3 manoIzk;
    private Vector3 manoDer;
    private Vector3 pieIzk;
    private Vector3 pieDer;
    private Vector3 rodillaIzk;
    private Vector3 rodillaDer;
    private Quaternion hombroIzk;
    private Quaternion hombroDer;
    private Vector3 codoIzk;
    private float codoIzkX;
    private float codoIzkY;
    private float codoIzkZ;
    private Vector3 codoDer;
    private float codoDerX;
    private float codoDerY;
    private float codoDerZ;
    private Quaternion cabezaHead;    
    private Vector3 pelvisSpineBase;
    private Vector3 centro;
    private Quaternion tobilloIzk;
    private Quaternion tobilloDer;
    private Quaternion torso;
    private Quaternion cadera;
    private Quaternion DerechaMano;
    private Quaternion IzquierdaMano;


    public GameObject DerMano;
    public GameObject IzkMano;
    public GameObject DerPie;
    public GameObject IzkPie;
    public GameObject DerRodilla;
    public GameObject IzkRodilla;
    public GameObject DerCodo;
    public GameObject IzkCodo;
    public GameObject pelvis;
    public GameObject centroTorso;

    public float cabezaX;
    public float cabezaY;
    public float cabezaZ;

    public float tobilloIzkY;
    public float tobilloDerY;
    
    public float DerHombroY;
    public float IzkHombroY;

    public float torsoPecho;

    public float giroManoDer;
    public float giroManoIzk;

    public float caderaSpineBase;

    public float alturaPos;



    //public Text dato;


    //Diccionario de Bodies y sus respectivos ID's
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;


    //Diccionario de Joints del cuerpo en el kinect
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    void Start()
    {
        _BodyManager = FindObjectOfType<BodySourceManager>();
    }
    void Update () 
    {

        //Inicialización del kinect
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        //Agregar ID cuando se detecta un cuerpo
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // Eliminar cuerpos que ya no están
        foreach (ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        //Esperar que el Kinect detecte un cuerpo
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            //Se crea un nuevo cuerpo si es detectado
            if (body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    //CREA EL CUERPO mediante los cubos (nombre y ecala) pero no les da ubicación
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    
                }
                
                //ubica los cubos en la posiciones correspondientes a cada joint
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
                
                //manos
                 DerMano.transform.position = new Vector3(manoDer.x, manoDer.y, manoDer.z);
                 IzkMano.transform.position = new Vector3(manoIzk.x, manoIzk.y, manoIzk.z);

                 //pies
                 DerPie.transform.position = new Vector3(redondear(pieDer.x), redondear(pieDer.y), redondear(pieDer.z));
                 IzkPie.transform.position = new Vector3(redondear(pieIzk.x), redondear(pieIzk.y), redondear(pieIzk.z));
                                
                //rodilla
                DerRodilla.transform.position = new Vector3(redondear(rodillaDer.x), redondear(rodillaDer.y), redondear(rodillaDer.z));
                IzkRodilla.transform.position = new Vector3(redondear(rodillaIzk.x), redondear(rodillaIzk.y), redondear(rodillaIzk.z));

                

                //pelvis
                float desplazamientoCadera = 0.75f;
                pelvis.transform.position = new Vector3(pelvisSpineBase.x, pelvisSpineBase.y, pelvisSpineBase.z + desplazamientoCadera);
                alturaPos = pelvisSpineBase.y;

                //centro torso
                centroTorso.transform.position = new Vector3(centro.x, centro.y, centro.z);

                //codos
                float desplazamientoCodos=0;
                if (alturaPos < 0)
                {
                    desplazamientoCodos = 0;
                }
                else
                {
                    desplazamientoCodos = 0.2f;
                }
                DerCodo.transform.position = new Vector3(codoDerX, codoDerY - desplazamientoCodos, codoDerZ);
                IzkCodo.transform.position = new Vector3(codoIzkX, codoIzkY - desplazamientoCodos, codoIzkZ);


              // dato.text = cabezaHead.z.ToString("F2")+ "  "  + cabezaZ;
            }

        // Traking para el componente UI
        //if(body.IsTracked)
        //{
          //  inectInputModule.instance.TrackBody(body);
        //}
    }
}

private float redondear(float articulacion)
{
        return Mathf.Round(articulacion * Mathf.Pow(10, 2)) / 100;
}

//CREA EL CUERPO mediante los cubos (nombre y ecala) pero no les da ubicación
private GameObject CreateBodyObject(ulong id)
{
    //crea un cuerpo y le da un id
    GameObject body = new GameObject("Body:" + id);

    //recorre los joint del cuerpo en el kinect 
    for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
    {
        //por cada joint crea un cubo
        GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //genera una linea entre cada joint 
       /* LineRenderer lr = jointObj.AddComponent<LineRenderer>();
        lr.SetVertexCount(2);
        lr.material = BoneMaterial;
        lr.SetWidth(0.02f, 0.02f);*/

            //al cubo joint lo escala, le da un nombre y lo asigna al cuerpo             
            jointObj.transform.localScale = new Vector3(0.000005f, 0.000005f, 0.000005f);
            //jointObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            jointObj.name = jt.ToString();
            //print(jointObj.name);
            jointObj.transform.parent = body.transform;
        }
        
        return body;
    }

    //ubica los cubos en la posiciones correspondientes a cada joint
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        //recorre los joint del cuerpo en el kinect 
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            //GUARDA EL JOINT en la variable sourceJoint
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            //encuentra y relaciona el arreglo de joints del cuerpo creado con el join del opbjeto cuerpo
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            //luego de encontrar el correspondiente joint, le da su correspondiente ubicación
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            //var orientacion1 = body.JointOrientations[jt].Orientation;
            //print(orientacion1.X + "  " + jt.ToString());
            // print(jointObj.JointOrienta + "   " + jointObj.rotation.y + "   " + jointObj.rotation.z);


            //guarda la ubicación de cada joint del cuerpo en una variable
            if (jt.ToString().Equals("HandLeft"))
            {
                manoIzk = GetVector3FromJoint(sourceJoint);
                //print(manoIzk.x + "   " + manoIzk.y + "   " + manoIzk.z);
            }

            if (jt.ToString().Equals("HandRight"))
            {
                manoDer = GetVector3FromJoint(sourceJoint);
                //print(manoDer.x + "   " + manoDer.y + "   " + manoDer.z);
            }

            if (jt.ToString().Equals("KneeLeft"))
            {
                rodillaIzk = GetVector3FromJoint(sourceJoint);
                //print(rodillaIzk.x + "   " + rodillaIzk.y + "   " + rodillaIzk.z);
            }

            if (jt.ToString().Equals("KneeRight"))
            {
                rodillaDer = GetVector3FromJoint(sourceJoint);
                //print(rodillaDer.x + "   " + rodillaDer.y + "   " + rodillaDer.z);
            }

            if (jt.ToString().Equals("AnkleLeft"))
            {
                pieIzk = GetVector3FromJoint(sourceJoint);
                //print(pieIzk.x + "   " + pieIzk.y + "   " + pieIzk.z);

                tobilloIzk = GetQuaternionJoint(body, jt);

                float ejeX = tobilloIzk.x;
                if(tobilloIzk.x < -3)
                {
                    tobilloIzkY = 90; 
                }
                else
                {
                    tobilloIzkY = map(tobilloIzk.x, 9.3f, -2.7f, 50, 170);
                }

            }

            if (jt.ToString().Equals("AnkleRight"))
            {
                pieDer = GetVector3FromJoint(sourceJoint);
                //print(pieDer.x + "   " + pieDer.y + "   " + pieDer.z);

                tobilloDer = GetQuaternionJoint(body, jt);
                tobilloDerY = map(tobilloDer.x, 0.5f, 9.3f,10, 120);               
            }

            if (jt.ToString().Equals("ElbowLeft"))
            {
               codoIzk = GetVector3FromJoint(sourceJoint);

               codoIzkX = codoIzk.x;
               codoIzkY = codoIzk.y;
               codoIzkZ = codoIzk.z;
               
            }

            if (jt.ToString().Equals("ElbowRight"))
            {
               codoDer = GetVector3FromJoint(sourceJoint);

               codoDerX = codoDer.x; 
               codoDerY = codoDer.y;
               codoDerZ = codoDer.z;

               
               
            }

            if (jt.ToString().Equals("SpineBase"))
            {
                pelvisSpineBase = GetVector3FromJoint(sourceJoint);
                //print(pelvisSpineBase.x + "   " + pelvisSpineBase.y + "   " + pelvisSpineBase.z);

                cadera = GetQuaternionJoint(body, jt);
                caderaSpineBase = map(cadera.w, 4.9f, -3f, 10, 180);

               // dato.text = cadera.w.ToString("F2")+ "   " + caderaSpineBase;
            }

            if (jt.ToString().Equals("SpineMid"))
            {
                centro = GetVector3FromJoint(sourceJoint);
                //print(centro.x + "   " + centro.y + "   " + centro.z);

                torso = GetQuaternionJoint(body, jt);
                torsoPecho = map(torso.w, 4f, -3f, 20, 160);
            }

            if (jt.ToString().Equals("Neck"))
            {
                if(torsoPecho > 120 || torsoPecho < 60 || alturaPos < 0)
                {
                    cabezaX = 0;
                    cabezaZ = -90;
                }
                else
                {
                    cabezaHead = GetQuaternionJoint(body, jt);
                    cabezaX = map(cabezaHead.x, -0.2f, 0.2f, -50, 50);
                    //cabezaY = map(cabezaHead.y, 9.84f, 9.94f, 40, 130);

                    float ejeZ = cabezaHead.z;
                    if (ejeZ < -0.6)
                    {
                        ejeZ = -0.6f;
                    }
                    else if (ejeZ > -0.2)
                    {
                        ejeZ = -0.2f;
                    }
                    cabezaZ = map(ejeZ, -0.6f, -0.2f, -130, -40);

                    //dato.text = cabezaHead.z.ToString("F2") + "  " + cabezaZ;
                }
                           
            }

            if (jt.ToString().Equals("ShoulderLeft"))
            {
                hombroIzk = GetQuaternionJoint(body, jt);
                IzkHombroY = map(hombroIzk.x, 8.3f, 7.3f, 0, -30);

                //dato.text = hombroIzk.x.ToString("F2") + "  " + IzkHombroY;
            }


            if (jt.ToString().Equals("ShoulderRight"))
            {
                hombroDer = GetQuaternionJoint(body, jt);
                DerHombroY = map(hombroDer.x, 8.2f, 7.7f, 0, -20);               
            }
       
            //dato.text = cadera.w.ToString("F2");


            /*if (jt.ToString().Equals("HandRight"))
            {
                DerechaMano = GetQuaternionJoint(body, jt);

                giroManoDer = map(DerechaMano.z, 6, -6, -40, -140);
            }*/

            /*LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if(targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }*/

        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    

    //me retorna la posición del joint
    public static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    //me retorna la orientación del joint
    public static Quaternion GetQuaternionJoint(Kinect.Body body, Kinect.JointType jointTd)
    {
        var orientacion = body.JointOrientations[jointTd].Orientation;
                
        return new Quaternion(orientacion.X*10,orientacion.Y*10,orientacion.Z*10,orientacion.W*10);
    }

    public static float map(float x, float x1, float x2, float y1, float y2)
    {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

        return m * x + c;
    }




}
