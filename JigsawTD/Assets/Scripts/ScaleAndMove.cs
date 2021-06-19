using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScaleAndMove : MonoBehaviour
{

    Vector2 m_ScreenPos = new Vector2();
    Vector3 oldPosition;
    Camera cam;
    float moveSpeed = 15f;
    private int slideSpeed = 5;
    private float scrollSpeed = 2.5f;
    private float maximum = 10;
    private float minmum = 3;

    private float maxUp = 13;
    private float maxDown = -13;
    private float maxLeft = -13;
    private float maxRight = 13;
    Vector2 CamMovement;
    // Start is called before the first frame update

    //新手引导
    Vector3 CamInitialPos;
    float CamInitialSize;
    public static bool MoveTurorial = false;
    public static bool SizeTutorial = false;
    public static bool CanControl = false;
    //

    void Start()
    {
        cam = this.GetComponent<Camera>();
        CamInitialPos = cam.transform.position;
        CamInitialSize = cam.orthographicSize;
        oldPosition = cam.transform.position;
        Input.multiTouchEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        //DesktopInput();
        TutorialCounter();
        RTSView();
        //TEST
        if (Input.GetKeyDown(KeyCode.P))
        {
            CanControl = !CanControl;
        }

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
            cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }

        Vector3 speedHorizon = new Vector3(0, 0, 0);
        Vector3 speedVertical = new Vector3(0, 0, 0);
        Vector3 speed = new Vector3(0, 0, 0);
        Vector3 v1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (v1.x < 0.01f && transform.localPosition.x > maxLeft)
        {
            speedHorizon = Vector3.left * slideSpeed * Time.deltaTime;
        }
        if (v1.x > 1 - 0.01f && transform.localPosition.x < maxRight)
        {
            speedHorizon = Vector3.right * slideSpeed * Time.deltaTime;
        }
        if (v1.y < 0.01f && transform.localPosition.y > maxDown)
        {
            speedVertical = Vector3.down * slideSpeed * Time.deltaTime;
        }
        if (v1.y > 1 - 0.01f && transform.localPosition.y < maxUp)
        {
            speedVertical = Vector3.up * slideSpeed * Time.deltaTime;
        }
        speed = speedHorizon + speedVertical;
        transform.Translate(speed, Space.World);
    }
    private void DesktopInput()
    {
        if (!CanControl)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minmum, maximum);
            cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CamMovement = Vector2.zero;
            return;
        }
        if (DraggingActions.DraggingThis == null && Input.GetMouseButton(0))
        {
            CamMovement = Vector3.left * Input.GetAxis("Mouse X") + Vector3.down * Input.GetAxis("Mouse Y");
        }
        else
        {
            CamMovement = Vector2.zero;
        }

    }

    private void LateUpdate()
    {
        transform.Translate(CamMovement * moveSpeed * Time.deltaTime);
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
}
