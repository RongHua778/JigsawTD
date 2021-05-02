using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelect : MonoBehaviour
{

    [SerializeField] RenderTexture renderTexture;
    TileShape m_Shape;

    public void InitializeDisplay(int displayID, TileShape shape)
    {
        m_Shape = shape;
        shape.SetUIDisplay(displayID, renderTexture);
    }

    public void OnShapeClick()
    {
        TileShape newShape = Instantiate(m_Shape);
        newShape.SetPreviewPlace();
    }

    public void ClearShape()
    {
        m_Shape.ReclaimTiles();
        Destroy(m_Shape.gameObject);
        m_Shape = null;
    }


}
