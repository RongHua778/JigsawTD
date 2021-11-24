using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScaleAndMove : IGameSystem
{
    MainUI m_MainUI;
    Vector2 m_ScreenPos = new Vector2();
    Vector3 oldPosition;

    private Vector3 Origin;
    private Vector3 Difference;
    private float SmoothSpeed = 0.6f;
    private bool Drag = false;

    Camera cam;
    //float moveSpeed = 15f;
    private int slideSpeed = 6;
    private float scrollSpeed = 2.5f;
    private float maximum = 10;
    private float minmum = 3;

    private float maxY = 8;
    private float minY = -8;
    private float minX = -8;
    private float maxX = 8;
    Vector2 CamMovement;
    // Start is called before the first frame update

    //新手引导
    Vector3 camInitPos;
    Vector3 CamViewPos;
    float CamInitialSize;
    public bool MoveTurorial = false;
    public bool SizeTutorial = false;
    public bool CanControl = false;
    //
    Vector3 speedHorizon = Vector3.zero;
    Vector3 speedVertical = Vector3.zero;
    Vector3 speed = Vector3.zero;


    [Header("视差控制")]
    [SerializeField] Transform[] backGrounds = default;
    [SerializeField] float parallaxScale = default;
    [SerializeField] float parallaxReductionFactor = default;
    [SerializeField] float smoothing;

    private Transform camTr;


    public void Initialize(MainUI mainUI)
    {
        m_MainUI = mainUI;
        cam = this.GetComponent<Camera>();
        camTr = cam.transform;
        CamViewPos = cam.transform.position;
        camInitPos = cam.transform.position;
        CamInitialSize = cam.orthographicSize;
        oldPosition = cam.transform.position;
        Input.multiTouchEnabled = true;
        MoveTurorial = false;
        SizeTutorial = false;
    }

    public override void GameUpdate()
    {
        DesktopInput();
        TutorialCounter();
        //RTSView();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

    }


    private void TutorialCounter()
    {
        if (MoveTurorial)
        {
            if (Vector2.Distance(transform.position, oldPosition) > 4f)
            {
                MoveTurorial = false;
                CamMovement = Vector2.zero;
                cam.transform.DOMove(camInitPos, 1f);
                CanControl = false;
                GameEvents.Instance.TutorialTrigger(TutorialType.MouseMove);
            }
        }
        if (SizeTutorial)
        {
            if (Mathf.Abs(cam.orthographicSize - CamInitialSize) > 1f)
            {
                SizeTutorial = false;
                cam.transform.DOMove(camInitPos, 1f);
                GameEvents.Instance.TutorialTrigger(TutorialType.WheelMove);
            }
        }
    }



    private void RTSView()
    {
        if (!CanControl)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minmum, maximum);
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }

        speedHorizon = Vector3.zero;
        speedVertical = Vector3.zero;
        speed = Vector3.zero;
        Vector3 v1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (v1.x < 0.01f && transform.localPosition.x > minX)
        {
            speedHorizon = Vector3.left * slideSpeed * Time.deltaTime;
        }
        else if (v1.x > 1 - 0.01f && transform.localPosition.x < maxX)
        {
            speedHorizon = Vector3.right * slideSpeed * Time.deltaTime;
        }
        else if (v1.y < 0.01f && transform.localPosition.y > minY)
        {
            speedVertical = Vector3.down * slideSpeed * Time.deltaTime;
        }
        else if (v1.y > 1 - 0.01f && transform.localPosition.y < maxY)
        {
            speedVertical = Vector3.up * slideSpeed * Time.deltaTime;
        }
        else
        {
            speedVertical = Input.GetAxisRaw("Horizontal") * Vector3.right * slideSpeed * Time.deltaTime;
            speedHorizon = Input.GetAxisRaw("Vertical") * Vector3.up * slideSpeed * Time.deltaTime;
        }
        //speedVertical = Input.GetAxisRaw("Horizontal") * Vector3.right * slideSpeed * Time.deltaTime;
        //speedHorizon = Input.GetAxisRaw("Vertical") * Vector3.up * slideSpeed * Time.deltaTime;
        speed = speedHorizon + speedVertical;
        transform.Translate(speed / m_MainUI.GameSpeed, Space.World);
    }
    private void DesktopInput()
    {
        if (!CanControl)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minmum, maximum);
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CamMovement = Vector2.zero;
            return;
        }
        if (DraggingActions.DraggingThis == null && Input.GetMouseButton(0))
        {
            Difference = cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            //CamMovement = Vector3.left * Input.GetAxis("Mouse X") + Vector3.down * Input.GetAxis("Mouse Y");
        }
        else
        {
            Drag = false;
        }
        if (Drag)
        {
            Vector3 desirePos = Origin - Difference;
            Vector3 smoothPos = Vector3.Lerp(cam.transform.position, desirePos, SmoothSpeed);
            cam.transform.position = smoothPos;
        }


        speedVertical = Input.GetAxisRaw("Horizontal") * Vector3.right * slideSpeed * Time.deltaTime;
        speedHorizon = Input.GetAxisRaw("Vertical") * Vector3.up * slideSpeed * Time.deltaTime;
        speed = speedHorizon + speedVertical;
        transform.Translate(speed / m_MainUI.GameSpeed, Space.World);


    }
    private void MobileInput()
    {
        if (Input.touchCount <= 0)
            return;
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                m_ScreenPos = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                cam.transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x, -Input.touches[0].deltaPosition.y, 0) * Time.deltaTime * 0.1f);
            }
        }
    }

    public void LocatePos(Vector2 pos)
    {
        Vector3 newPos = new Vector3(pos.x, pos.y, cam.transform.position.z);
        cam.transform.DOMove(newPos, 0.5f);
    }

    private void Update()
    {
        Vector2 parallax = (CamViewPos - camTr.position) * parallaxScale;
        for (int i = 0; i < backGrounds.Length; i++)
        {
            float backgroundTargetPosX = backGrounds[i].position.x + parallax.x * (i * parallaxReductionFactor + 1);
            float backgroundTargetPosY = backGrounds[i].position.y + parallax.y * (i * parallaxReductionFactor + 1);
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backGrounds[i].position.z);
            backGrounds[i].position = Vector3.Lerp(backGrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        CamViewPos = camTr.position;
    }
}
