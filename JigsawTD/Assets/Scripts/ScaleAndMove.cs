using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleAndMove : MonoBehaviour
{
    Vector2 m_ScreenPos = new Vector2();
    Vector3 oldPosition;
    Camera cam;
    float moveSpeed = 60f;

    private float scrollSpeed = 2.5f;
    private float maximum = 10;
    private float minmum = 3;
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
        MobileInput();
        DesktopInput();
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
            transform.Translate(Vector3.left * Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * Input.GetAxis("Mouse Y") * -moveSpeed * Time.deltaTime);
        }

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
