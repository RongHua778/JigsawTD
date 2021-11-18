using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelDownSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StrategyBase m_Strategy;
    public void SetStrategy(StrategyBase strategy)
    {
        m_Strategy = new StrategyBase(strategy.Attribute, strategy.Quality - 1);
        m_Strategy.SetQualityValue();

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(true, m_Strategy.Attribute.element, m_Strategy.Quality);
        GameManager.Instance.ShowTurretTips(m_Strategy, StaticData.RightMidTipsPos);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
        GameManager.Instance.HideTips();
    }

}
