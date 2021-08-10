using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelDownSelect :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StrategyBase m_Strategy;
    public void SetStrategy(StrategyBase strategy)
    {
        m_Strategy = strategy;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(true, m_Strategy.Element, m_Strategy.Quality-1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
    }

}
