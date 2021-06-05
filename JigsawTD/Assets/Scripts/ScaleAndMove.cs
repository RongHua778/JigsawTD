using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleAndMove : MonoBehaviour
{
    Vector2 m_ScreenPos = new Vector2();
    Vector3 oldPosition;
    Camera cam;
    float moveSpeed = 20f;
    private int slideSpeed = 0;
    private float scrollSpeed = 2.5f;
    private float maximum = 10;
    private float minmum = 3;

    private float maxUp = 20;
    private float maxDown = -20;
    private float maxLeft = -20;
    private float maxRight = 20;
    Vector2 CamMovement;
    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
        oldPosition = cam.transform.position;
        Input.multiTouchEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //MobileInput();
        DesktopInput();
        //RTSView();
    }

    private void FixedUpdate()
    {
        if (transform.localPosition.x < maxLeft ||
            transform.localPosition.x > maxRight ||
            transform.localPosition.y > maxUp ||
            transform.localPosition.y < maxDown)
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }

    private void RTSView()
    {
        Vector3 speedHorizon = new Vector3(0, 0, 0);
        Vector3 speedVertical = new Vector3(0, 0, 0);
        Vector3 speed = new Vector3(0, 0, 0);
        Vector3 v1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (v1.x < 0.05f && transform.localPosition.x > maxLeft)
        {
            speedHorizon = Vector3.left * slideSpeed * Time.deltaTime;
        }
        if (v1.x > 1 - 0.05f && transform.localPosition.x < maxRight)
        {
            speedHorizon = Vector3.right * slideSpeed * Time.deltaTime;
        }
        if (v1.y < 0.05f && transform.localPosition.y > maxDown)
        {
            speedVertical = Vector3.down * slideSpeed * Time.deltaTime;
        }
        if (v1.y > 1 - 0.05f && transform.localPosition.y < maxUp)
        {
            speedVertical = Vector3.up * slideSpeed * Time.deltaTime;
        }
        speed = speedHorizon + speedVertical;
        transform.Translate(speed, Space.World);
    }
    private void DesktopInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minmum, maximum);
            cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        }
        if (EventSystem.current.IsPointerOverGameObject())
            return;
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
