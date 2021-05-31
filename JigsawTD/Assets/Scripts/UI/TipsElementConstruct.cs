using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipsElementConstruct : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] Elements = default;
    Blueprint m_BluePrint;

    public void SetElements(Blueprint bluePrint)
    {
        m_BluePrint = bluePrint;
        List<Composition> compositions = bluePrint.Compositions;
        for (int i = 0; i < Elements.Length; i++)
        {
            if (i >= compositions.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            TurretAttribute attribute = GameManager.Instance.GetElementAttribute((Element)compositions[i].elementRequirement);
            Elements[i].sprite = attribute.TurretLevels[compositions[i].levelRequirement - 1].Icon;
        }
    }

    public string GetInfo()
    {
        string text = "";
        if (m_BluePrint.CompositeAttackDamage > 0)
        {
            text += "π•ª˜+" + m_BluePrint.CompositeAttackDamage*100 + "%\n";
        }
        if (m_BluePrint.CompositeAttackSpeed > 0)
        {
            text += "π•ÀŸ+" + m_BluePrint.CompositeAttackSpeed * 100 + "%\n";
        }
        if (m_BluePrint.CompositeSlowRate > 0)
        {
            text += "ºıÀŸ+" + m_BluePrint.CompositeSlowRate + "\n";
        }
        if (m_BluePrint.CompositeCriticalRate > 0)
        {
            text += "±©ª˜¬ +" + m_BluePrint.CompositeCriticalRate * 100 + "%\n";
        }

        if (m_BluePrint.CompositeSputteringRange > 0)
        {
            text += "Ω¶…‰+" + m_BluePrint.CompositeSputteringRange + "\n";
        }
        return text; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
         LevelUIManager.Instance.ShowTempTips(GetInfo(), transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LevelUIManager.Instance.HideTempTips();
    }
}
