using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileSelect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
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
        m_Shape.SetPreviewPlace();
        m_Shape = null;
        GameManager.Instance.PreviewComposition(false);
        GameManager.Instance.SelectShape();
    }

    public void ClearShape()
    {
        if (m_Shape == null)
            return;
        m_Shape.ReclaimTiles();
        Destroy(m_Shape.gameObject);
        m_Shape = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ElementStrategy strategy = m_Shape.m_ElementTurret.Strategy as ElementStrategy;
        GameManager.Instance.PreviewComposition(true, strategy.Element, strategy.Quality);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
    }
}
