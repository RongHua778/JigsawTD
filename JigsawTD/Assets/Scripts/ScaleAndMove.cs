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

    private float maxY = 13;
    private float minY = -13;
    private float minX = -13;
    private float maxX = 13;
    Vector2 CamMovement;
    // Start is called before the first frame update

    //新手引导
    Vector3 CamInitialPos;
    float CamInitialSize;
    public static bool MoveTurorial = false;
    public static bool SizeTutorial = false;
    public static bool CanControl = false;
    //
    Vector3 speedHorizon = Vector3.zero;
    Vector3 speedVertical = Vector3.zero;
    Vector3 speed = Vector3.zero;

    //void Start()
    //{
    //    cam = this.GetComponent<Camera>();
    //    CamInitialPos = cam.transform.position;
    //    CamInitialSize = cam.orthographicSize;
    //    oldPosition = cam.transform.position;
    //    Input.multiTouchEnabled = true;
    //}


    public void Initialize(GameManager gamemanager, MainUI mainUI)
    {
        Initialize(gamemanager);
        m_MainUI = mainUI;
        cam = this.GetComponent<Camera>();
        CamInitialPos = cam.transform.position;
        CamInitialSize = cam.orthographicSize;
        oldPosition = cam.transform.position;
        Input.multiTouchEnabled = true;
    }

    public override void GameUpdate()
    {
        DesktopInput();
        TutorialCounter();
        //RTSView();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

        //TEST
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    CanControl = !CanControl;
        //}
    }


    private void TutorialCounter()
    {
        if (MoveTurorial)
        {
            if (Vector2.Distance(transform.position, oldPosition) > 4f)
            {
                MoveTurorial = false;
                CamMovement = Vector2.zero;
                cam.transform.DOMove(CamInitialPos, 1f);
                CanControl = false;
                GameManager.Instance.TriggerGuide(1);
            }
        }
        if (SizeTutorial)
        {
            if (Mathf.Abs(cam.orthographicSize - CamInitialSize) > 1f)
            {
                SizeTutorial = false;
                cam.transform.DOMove(CamInitialPos, 1f);
                GameManager.Instance.TriggerGuide(2);
            }
        }
    }

    private void FixedUpdate()
    {
        //if (transform.localPosition.x < maxLeft ||
        //    transform.localPosition.x > maxRight ||
        //    transform.localPosition.y > maxUp ||
        //    transform.localPosition.y < maxDown)
        //{
        //    transform.position = new Vector3(0, 0, -10);
        //}
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
        if (Input.GetMouseButtonDown(1))
        {
            cam.transform.DOMove(CamInitialPos, 0.6f);
        }

        speedVertical = Input.GetAxisRaw("Horizontal") * Vector3.right * slideSpeed * Time.deltaTime;
        speedHorizon = Input.GetAxisRaw("Vertical") * Vector3.up * slideSpeed * Time.deltaTime;
        speed = speedHorizon + speedVertical;
        transform.Translate(speed / m_MainUI.GameSpeed, Space.World);
        //else
        //{
        //    CamMovement = Vector2.zero;
        //}

    }

    //private void LateUpdate()
    //{


    //    transform.Translate(CamMovement * moveSpeed * Time.deltaTime);
    //}

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
}
