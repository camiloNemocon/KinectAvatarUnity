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
    private Vector3 codoDer;
    private Quaternion cabezaHead;    
    private Vector3 pelvisSpineBase;
    //private Vector3 centro;
    private Quaternion tobilloIzk;
    private Quaternion tobilloDer;
    private Quaternion torso;
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
    //public GameObject centroTorso;
    
    public float DerHombroY;
    public float IzkHombroY;

    public float tobilloIzkY;
    public float tobilloDerY;

    public float cabezaZ;
    
    public float torsoPecho;
             
    public Text dato;

    public bool user = false;


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
                user = false;
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

                user = true;

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
                pelvis.transform.position = new Vector3(pelvisSpineBase.x, pelvisSpineBase.y+0.1f, pelvisSpineBase.z);
               
                //codos                
                DerCodo.transform.position = new Vector3(codoDer.x, codoDer.y, codoDer.z);
                IzkCodo.transform.position = new Vector3(codoIzk.x, codoIzk.y, codoIzk.z);
            }        
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


            //al cubo joint lo escala, le da un nombre y lo asigna al cuerpo             
            jointObj.transform.localScale = new Vector3(0.000005f, 0.000005f, 0.000005f);
            //jointObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            jointObj.name = jt.ToString();
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

            if (jt.ToString().Equals("ElbowLeft"))
            {
                codoIzk = GetVector3FromJoint(sourceJoint);
            }

            if (jt.ToString().Equals("ElbowRight"))
            {
                codoDer = GetVector3FromJoint(sourceJoint);
            }

            if (jt.ToString().Equals("ShoulderLeft"))
            {
                hombroIzk = GetQuaternionJoint(body, jt);
                IzkHombroY = map(hombroIzk.x, 0.70f, 0.80f, 0, -30);
            }

            if (jt.ToString().Equals("ShoulderRight"))
            {
                hombroDer = GetQuaternionJoint(body, jt);
                DerHombroY = map(hombroDer.x, 0.80f, 0.70f, 0, -30);
            }

            if (jt.ToString().Equals("AnkleLeft"))
            {
                pieIzk = GetVector3FromJoint(sourceJoint);
                //print(pieIzk.x + "   " + pieIzk.y + "   " + pieIzk.z);

                tobilloIzk = GetQuaternionJoint(body, jt);
                if(tobilloIzk.x < -0.6)
                {
                    tobilloIzkY = 90; 
                }
                else
                {
                    tobilloIzkY = map(tobilloIzk.x, 0.9f, -0.5f, 50, 170);
                }
            }

            if (jt.ToString().Equals("AnkleRight"))
            {
                pieDer = GetVector3FromJoint(sourceJoint);
                //print(pieDer.x + "   " + pieDer.y + "   " + pieDer.z);

                tobilloDer = GetQuaternionJoint(body, jt);
                tobilloDerY = map(tobilloDer.x, 0.9f, 0.3f,-30, -160);
            }
                
            if (jt.ToString().Equals("Neck"))
            {
                cabezaHead = GetQuaternionJoint(body, jt);
                float ejeZ = cabezaHead.x;
                if (ejeZ > 0.05f)
                {
                   ejeZ = 0.05f;
                }
                else if (ejeZ < -0.05f)
                {
                    ejeZ = -0.05f;
                }
                else
                {
                    ejeZ = cabezaHead.x;
                }
                cabezaZ = map(ejeZ, -0.05f, 0.05f, -40, 40);
            }

            if (jt.ToString().Equals("SpineMid"))
            {
                torso = GetQuaternionJoint(body, jt);
                torsoPecho = map(torso.w, 0.4f, -0.3f, 110, 240);
            }

            if (jt.ToString().Equals("SpineBase"))
            {
                pelvisSpineBase = GetVector3FromJoint(sourceJoint);
                //print(pelvisSpineBase.x + "   " + pelvisSpineBase.y + "   " + pelvisSpineBase.z);
            }
            

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
        //return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
        return new Vector3(joint.Position.X*2.8f, joint.Position.Y* 2.8f, joint.Position.Z* 2.8f);
    }

    //me retorna la orientación del joint
    public static Quaternion GetQuaternionJoint(Kinect.Body body, Kinect.JointType jointTd)
    {
        var orientacion = body.JointOrientations[jointTd].Orientation;
                
        return new Quaternion(orientacion.X,orientacion.Y,orientacion.Z,orientacion.W);
    }

    public static float map(float x, float x1, float x2, float y1, float y2)
    {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

        return m * x + c;
    }




}
