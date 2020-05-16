using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KinectUIWaitCursor : AbstractKinectUICursor {


    public override void Start()
    {
        base.Start();
        // make sure its fill typed
        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial360;
        _image.fillAmount = 0f;
    }

    public override void ProcessData()
    {
        // update pos
        transform.position = _data.GetHandScreenPosition();
        if(_data.IsHovering)
        {
            _image.fillAmount = _data.WaitOverAmount;
        }
        else
        {
            _image.fillAmount = 0f;
        }
    }
}
