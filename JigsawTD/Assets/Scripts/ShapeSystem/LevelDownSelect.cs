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
        m_Strategy = new StrategyBase(strategy.m_Att, strategy.Quality - 1);
        m_Strategy.SetQualityValue();

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(true, m_Strategy.m_Att.element, m_Strategy.Quality);
        GameManager.Instance.ShowTurretTips(m_Strategy, new Vector2(Screen.width - 1000, Screen.height / 2));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
        GameManager.Instance.HideTips();
    }

}
