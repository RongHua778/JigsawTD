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



    private void OnMouseDown()
    {
        isDragging = true;
        _draggingThis = this;
        pointerOffset = transform.position - MouseInWorldCoords();
        pointerOffset.z = 0;
        OnStartDrag();
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            _draggingThis = null;
            OnEndDrag();
        }
    }

    protected Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        return cam.ScreenToWorldPoint(screenMousePos);
    }

}
