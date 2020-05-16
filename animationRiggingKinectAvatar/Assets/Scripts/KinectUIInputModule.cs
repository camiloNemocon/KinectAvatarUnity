using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Windows.Kinect;

public class KinectUIInputModule : StandaloneInputModule
{
    public KinectUIComponent uıComponent;
    //[Header("Hand transform either to be set here or from script call")]
    //public Transform handTransform;
    //public JointType jointToTrack = JointType.HandLeft;
    //[Header("Properties for wait over a button to perform click action")]
    //public bool waitOverToClick = false;
    //[Range(1, 10)]
    //public float waitTime = 3f;
    //public float handMoveTreshold = .1f;
    //public GameObject waitingGraphic;
    //[Header("Push hand properties")]
    //public bool usePushHandToClick = false;
    //[SerializeField]
    //private float depthChangeTreshold = .4f;
    //[SerializeField]
    //private float dragTreshold = .1f;

    //public HandState handStateVisual;

    //private GameObject _targetObject;
    //private Vector3 _handPosition, _tempWorldPosition;
    //private float _clickCounter;
    //private bool _isPressing = false;
    //private bool _isDraging = false;

    PointerEventData _handPointerData;

    /** Instance */
    static KinectUIInputModule _instance = null;
    public static KinectUIInputModule instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(KinectUIInputModule)) as KinectUIInputModule;

                if (!_instance)
                {
                    if (EventSystem.current)
                        EventSystem.current.gameObject.AddComponent<KinectUIInputModule>();
                    else
                        Debug.LogWarning("Create your UI first");
                }
            }
            return _instance;
        }
    }

    public void TrackBody(Body body)
    {
        //Vector3 position = BodySourceView.GetVector3FromJoint( body.Joints[jointToTrack]);
        //handStateVisual = body.HandRightState;
        //SetHandPosition(position);
        uıComponent.UpdateComponent(body);
    }

    /// <summary>
    /// Sets hand position, must be called from Update
    /// </summary>
    /// <param name="position">Position.</param>
    //public void SetHandPosition(Vector3 position)
    //{
    //    position = Camera.main.WorldToScreenPoint(position);
    //    _handPosition = position;
    //    //Debug.Log("pos " + _position);
    //}

    /// <summary>
    /// Performs click action if hand is over a component
    /// </summary>
    protected void OnClick()
    {
        //if (_targetObject != null && IsPointerOverGameObject(0))
        //{
        //BaseEventData data = GetBaseEventData();
        //data.selectedObject = _targetObject;
        //ExecuteEvents.ExecuteHierarchy(_targetObject, data, ExecuteEvents.submitHandler);

        //eventSystem.SetSelectedGameObject(null);
        //GameObject newPressed = ExecuteEvents.ExecuteHierarchy(_targetObject, _handPointerData, ExecuteEvents.submitHandler);
        //if (newPressed == null)
        //{
        //    // submit handler not found, try select handler instead
        //    newPressed = ExecuteEvents.ExecuteHierarchy(_targetObject, _handPointerData, ExecuteEvents.selectHandler);
        //}
        //if (newPressed != null)
        //{
        //    eventSystem.SetSelectedGameObject(newPressed);
        //}
        //}
        //PointerEventData lookData = GetLookPointerEventData();
        //eventSystem.SetSelectedGameObject(null);
        //if (lookData.pointerCurrentRaycast.gameObject != null)
        //{
        //    GameObject go = lookData.pointerCurrentRaycast.gameObject;
        //    GameObject newPressed = ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);
        //    if (newPressed == null)
        //    {
        //        // submit handler not found, try select handler instead
        //        newPressed = ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.selectHandler);
        //    }
        //    if (newPressed != null)
        //    {
        //        eventSystem.SetSelectedGameObject(newPressed);
        //    }
        //}
        //Debug.Log("OnClick");
    }
    protected void OnClick(Vector3 pos)
    {
        PointerEventData lookData = GetLookPointerEventData(pos);
        eventSystem.SetSelectedGameObject(null);
        if (lookData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject go = lookData.pointerCurrentRaycast.gameObject;
            ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);
        }

    }
    private void OnScroll( Vector3 position , Vector3 scrollDelta)
    {
        PointerEventData lookData = GetLookPointerEventData(position);
        eventSystem.SetSelectedGameObject(null);
        //if (lookData.pointerCurrentRaycast.gameObject != null)
        //{
            Debug.Log("drag");
            GameObject go = lookData.pointerCurrentRaycast.gameObject;
            PointerEventData pEvent = new PointerEventData(eventSystem);
            //			pEvent.worldPosition = _position;
            pEvent.dragging = true;
            //			pEvent.pointerDrag = _targetObject;
            pEvent.scrollDelta = scrollDelta;
            pEvent.useDragThreshold = true;
            ExecuteEvents.ExecuteHierarchy(go, pEvent, ExecuteEvents.scrollHandler);
        //}
    }
    #region OVERRIDE METHODS
    public override void ActivateModule()
    {
        base.ActivateModule();
        if (GetComponent<StandaloneInputModule>()) GetComponent<StandaloneInputModule>().enabled = false;
        if (GetComponent<TouchInputModule>()) GetComponent<TouchInputModule>().enabled = false;
        KinectUIComponent.OnClickEvent += OnClick;
        KinectUIComponent.OnScrollEvent += OnScroll;
        // make sure we dont block our raycast with waiting graphic
        //if (waitingGraphic)
        //{
        //    if (waitingGraphic.GetComponent<CanvasGroup>() == null)
        //    {
        //        CanvasGroup c = waitingGraphic.AddComponent<CanvasGroup>();
        //        c.blocksRaycasts = false;
        //    }
        //}
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        KinectUIComponent.OnClickEvent -= OnClick;
        KinectUIComponent.OnScrollEvent -= OnScroll;
    }

    //public override bool IsPointerOverGameObject(int pointerId)
    //{
    //    if (_handPointerData.pointerCurrentRaycast.gameObject != null)
    //        return true;
    //    return false;
    //    //if (_targetObject != null)
    //    //    return true;
    //    //return false;
    //}

    public override void Process()
    {
        //if (handTransform)
        //{
        //    SetHandPosition(handTransform.position);
        //}
        // SendUpdateEventToSelectedObject();
        ProcessHover();
        //ProcessPress();
        //ProcessDrag();
        //ProcessWaitOverComponent();
    }
    #endregion
    //private PointerEventData GetLookPointerEventData()
    //{
    //    //Vector3 lookPosition = Camera.main.WorldToScreenPoint(_handPosition);
    //    if (_handPointerData == null)
    //    {
    //        _handPointerData = new PointerEventData(eventSystem);
    //    }
    //    _handPointerData.Reset();
    //    _handPointerData.delta = Vector2.zero;
    //    _handPointerData.position = _handPosition;//lookPosition;
    //    _handPointerData.scrollDelta = Vector2.zero;
    //    eventSystem.RaycastAll(_handPointerData, m_RaycastResultCache);
    //    _handPointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
    //    m_RaycastResultCache.Clear();
    //    return _handPointerData;
    //}
    private PointerEventData GetLookPointerEventData(Vector3 componentPosition)
    {
        //Vector3 lookPosition = Camera.main.WorldToScreenPoint(_handPosition);
        if (_handPointerData == null)
        {
            _handPointerData = new PointerEventData(eventSystem);
        }
        _handPointerData.Reset();
        _handPointerData.delta = Vector2.zero;
        _handPointerData.position = componentPosition;//lookPosition;
        _handPointerData.scrollDelta = Vector2.zero;
        eventSystem.RaycastAll(_handPointerData, m_RaycastResultCache);
        _handPointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();
        return _handPointerData;
    }
    //private bool SendUpdateEventToSelectedObject()
    //{
    //    if (eventSystem.currentSelectedGameObject == null)
    //        return false;
    //    BaseEventData data = GetBaseEventData();
    //    ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
    //    return data.used;
    //}
    private void ProcessHover()
    {
        PointerEventData pointer = GetLookPointerEventData(uıComponent.GetHandScreenPosition());//PointerEventData pointer = GetLookPointerEventData();//new PointerEventData(EventSystem.current);
        HandlePointerExitAndEnter(pointer, _handPointerData.pointerCurrentRaycast.gameObject);
        //SetTargetObject(_handPointerData.pointerCurrentRaycast.gameObject);
        // pointer.position = _handPosition;
        //Send ray
        //var raycastResults = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(pointer, raycastResults);
        //if (raycastResults.Count > 0)
        //{
        //    SetTargetObject(raycastResults[0].gameObject);
        //    //			Debug.Log(raycastResults[0].gameObject.name);
        //}
        //else
        //{
        //    DesetTargetObject();
        //}
        //Debug.Log("is over " + IsPointerOverGameObject(0));
    }

    //private void ProcessWaitOverComponent()
    //{
    //    if (!waitOverToClick) return;
    //    if (!IsPointerOverGameObject(0))
    //    {
    //        _clickCounter = 0f;
    //        if (waitingGraphic) waitingGraphic.SetActive(false);
    //        return;
    //    }
    //    _clickCounter += Time.deltaTime;
    //    if (_isDraging)
    //    {
    //        _clickCounter = 0f;
    //        if (waitingGraphic) waitingGraphic.SetActive(false);
    //        return;
    //    }
    //    if (waitingGraphic)
    //    {
    //        waitingGraphic.SetActive(true);
    //        waitingGraphic.transform.position = _handPosition;
    //    }
    //    if (_clickCounter >= waitTime)
    //    {
    //        OnClick();
    //        _clickCounter = 0f;
    //    }
    //}

    //void ProcessPress()
    //{
    //    if (!IsPointerOverGameObject(0))
    //    {
    //        _isPressing = false;
    //        _tempWorldPosition = _handPosition;
    //        return;
    //    }

    //    if (!_isPressing && handStateVisual == HandState.Closed)
    //    {
    //        _isPressing = true;
    //        //_tempWorldPosition = _handPosition;
    //    }
    //    else if (_isPressing && handStateVisual == HandState.Open)
    //    {
    //        OnClick();
    //        _isPressing = false;
    //    }

        /* Push hand try
        if (_handPosition.z - _tempWorldPosition.z > depthChangeTreshold && !_isPressing)
        {
            //			Debug.Log("down");
            _isPressing = true;
            _tempWorldPosition = _handPosition;
            // pointer down
            //PointerEventData pEvent = new PointerEventData(eventSystem);
            //pEvent.pointerEnter = _targetObject;
            //pEvent.worldPosition = _handPosition;
            //ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(_targetObject, pEvent, ExecuteEvents.pointerDownHandler);
        }
        else if (_tempWorldPosition.z - _handPosition.z > depthChangeTreshold && _isPressing)
        {
            //			Debug.Log("up");
            _isPressing = false;
            _tempWorldPosition = _handPosition;
            // pointer up
            //PointerEventData pEvent = new PointerEventData(eventSystem);
            //pEvent.worldPosition = _handPosition;
            //ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(_targetObject, pEvent, ExecuteEvents.pointerUpHandler);
            if (!_isDraging) OnClick();
        }
         * */
    //}

    //private void ProcessDrag()
    //{
    //    if (!IsPointerOverGameObject(0) || !_isPressing || _targetObject == null)
    //    {
    //        _isDraging = false;
    //        return;
    //    }
    //    if (Mathf.Abs(_tempWorldPosition.x - _handPosition.x) > dragTreshold || Mathf.Abs(_tempWorldPosition.y - _handPosition.y) > dragTreshold)
    //    {
    //        _isDraging = true;
    //    }
    //    else
    //    {
    //        _isDraging = false;
    //    }
    //    //		_tempWorldPosition = _position;
    //    if (!_isPressing)
    //    {
    //        return;
    //    }
    //    if (_isDraging)
    //    {
    //        //AxisEventData axisEvent = GetAxisEventData(_tempWorldPosition.x - _handPosition.x,_tempWorldPosition.y - _handPosition.y,0f);
    //        //Debug.Log("dragging" + axisEvent.ToString());
    //        //ExecuteEvents.ExecuteHierarchy(_targetObject, axisEvent, ExecuteEvents.moveHandler);
    //        PointerEventData pEvent = new PointerEventData(eventSystem);
    //        //			pEvent.worldPosition = _position;
    //        pEvent.dragging = true;
    //        //			pEvent.pointerDrag = _targetObject;
    //        pEvent.scrollDelta = (_tempWorldPosition - _handPosition) / 8f;
    //        pEvent.useDragThreshold = true;
    //        ExecuteEvents.ExecuteHierarchy(_targetObject, pEvent, ExecuteEvents.scrollHandler);
    //    }
    //}

    //private void SetTargetObject(GameObject obj)
    //{
    //    if (obj == null || obj == _targetObject) return;
        //we're hovering over a new object, so unhover the current one
        //if ((_targetObject != null) && (_targetObject != obj))
        //{
        //    PointerEventData pEvent = new PointerEventData(eventSystem);
        //    pEvent.worldPosition = _handPosition;
        //    ExecuteEvents.ExecuteHierarchy(_targetObject, pEvent, ExecuteEvents.pointerExitHandler);
        //}
        //if (obj != null)
        //{
        //    //this is the same object that was hovered last time, so bail
        //    if (obj == _targetObject)
        //        return;
        //    //we've entered a new GUI object, so excute that event to highlight it
        //    PointerEventData pEvent = new PointerEventData(eventSystem);
        //    pEvent.pointerEnter = obj;
        //    pEvent.worldPosition = _handPosition;
        //    ExecuteEvents.ExecuteHierarchy(obj, pEvent, ExecuteEvents.pointerEnterHandler);
        //}
    //    _targetObject = obj;
    //    //Debug.Log("temp world pos set " + _tempWorldPosition);
    //    // set temp position to check clicking
    //    _tempWorldPosition = _handPosition;
    //}
    //private void DesetTargetObject()
    //{
    //    // if target is not null deselect it 
    //    if ((_targetObject != null) && !_isPressing)
    //    {
    //        PointerEventData pEvent = new PointerEventData(eventSystem);
    //        pEvent.worldPosition = _handPosition;
    //        ExecuteEvents.ExecuteHierarchy(_targetObject, pEvent, ExecuteEvents.pointerExitHandler);
    //        _targetObject = null;
    //    }
    //}

}
[System.Serializable]
public class KinectUIComponent
{
    public delegate void OnClickDelegate(Vector3 position);
    public static event OnClickDelegate OnClickEvent;
    public delegate void OnScrollDelegate(Vector3 position,Vector3 scrollDelta);
    public static event OnScrollDelegate OnScrollEvent;

    [SerializeField]
    private JointType handType = JointType.HandRight;
    [SerializeField]
    private KinectUIClickGesture handClickGesture = KinectUIClickGesture.HandState;
    [SerializeField]
    private float dragTreshold = .1f;
    [SerializeField]
    private float _scrollSpeed = 7f;

    [Header("WaitOver")]
    [Range(1, 5)]
    [SerializeField]
    private float waitTime = 3f;
    [SerializeField]
    private float handMoveTreshold = .1f;
    //[SerializeField]
    //private GameObject waitingGraphic;
    [Header("Push")]
    [SerializeField]
    private float depthChangeTreshold = .4f;

    private Vector3 _handPosition,_tempHandPosition;
    private HandState _handState;
    private bool _isPressing, _isDraging = false;

    public void UpdateComponent(Body body)
    {
        this._handPosition = GetVector3FromJoint(body.Joints[handType]);
        this._handState = GetStateFromJointType(body, handType);
        Process();
        ProcessDrag();
    }

    public Vector3 GetHandScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(_handPosition);
    }

    private void Process()
    {
        ProcessHandState();
        switch (handClickGesture)
        {
            case KinectUIClickGesture.Push:
                ProcessPush();
                break;
            case KinectUIClickGesture.WaitOver:
                ProcessWaitOver();
                break;
        }
    }
    private void ProcessDrag()
    {
        if (!_isPressing)
        {
            return;
        }
        if (Mathf.Abs(_tempHandPosition.x - GetHandScreenPosition().x) > dragTreshold || Mathf.Abs(_tempHandPosition.y - GetHandScreenPosition().y) > dragTreshold)
        {
            _isDraging = true;
            if (OnScrollEvent != null) OnScrollEvent(GetHandScreenPosition(), (_tempHandPosition - GetHandScreenPosition()) / _scrollSpeed);
        }
        else
        {
            _isDraging = false;
        }
    }

    private void ProcessHandState()
    {
        if (!_isPressing && _handState == HandState.Closed)
        {
            _tempHandPosition = GetHandScreenPosition();
            _isPressing = true;
        }
        else if (_isPressing && (_handState == HandState.Open || _handState == HandState.Unknown))
        {
            _isPressing = false;
            if (OnClickEvent != null && !_isDraging && handClickGesture == KinectUIClickGesture.HandState) OnClickEvent(GetHandScreenPosition());
        }
    }
    private void ProcessPush()
    {
        if (!_isPressing && _handState == HandState.Closed)
        {
            _isPressing = true;
        }
        else if (_isPressing && (_handState == HandState.Open || _handState == HandState.Unknown))
        {
            _isPressing = false;
            if (OnClickEvent != null) OnClickEvent(GetHandScreenPosition());
        }
    }
    private void ProcessWaitOver()
    {
        if (!_isPressing && _handState == HandState.Closed)
        {
            _isPressing = true;
        }
        else if (_isPressing && (_handState == HandState.Open || _handState == HandState.Unknown))
        {
            _isPressing = false;
            if (OnClickEvent != null) OnClickEvent(GetHandScreenPosition());
        }
    }

    private HandState GetStateFromJointType(Body body, JointType type)
    {
        switch (type)
        {
            case JointType.HandLeft:
                return body.HandLeftState;
            case JointType.HandRight:
                return body.HandRightState;
            default:
                Debug.LogWarning("Please select a hand joint, by default right hand will be used!");
                return body.HandRightState;
        }
    }
    private Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}

