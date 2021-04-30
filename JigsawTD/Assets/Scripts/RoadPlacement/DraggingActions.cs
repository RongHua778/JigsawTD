using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggingActions : MonoBehaviour
{
    bool isDragging = false;
    protected Vector3 pointerOffset;
    private float zDisplacement;
    Camera cam;

    private static DraggingActions _draggingThis;
    public static DraggingActions DraggingThis => _draggingThis;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            //Vector3 mousePos = MouseInWorldCoords();
            //transform.position = new Vector3(Mathf.Round(mousePos.x + pointerOffset.x), Mathf.Round(mousePos.y + pointerOffset.z), transform.position.z);

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
        zDisplacement = transform.position.z - cam.transform.position.z;
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
