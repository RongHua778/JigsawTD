using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementSelectPreview : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] TileSelect m_TileSelect = default;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StrategyElement strategy = m_TileSelect.Shape.m_ElementTurret.Strategy as StrategyElement;
        GameManager.Instance.PreviewComposition(true, strategy.Element, strategy.Quality);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
    }
}
