using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggingActions : MonoBehaviour
{
    protected bool isDragging = false;
    protected Vector3 pointerOffset;
    Camera cam;

    private static DraggingActions _draggingThis;
    public static DraggingActions DraggingThis { get => _draggingThis; set => _draggingThis = value; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDragging)
        {
            OnDraggingInUpdate();
        }
    }

    public virtual void OnStartDrag()
    {

    }

    public virtual void OnEndDrag()
    {

    }

    public virtual void OnDraggingInUpdate()
    {

    }



    protected Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        return cam.ScreenToWorldPoint(screenMousePos);
    }

}
