using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;
    Blueprint m_BluePrint;
    [SerializeField] InfoBtn m_InfoBtn = default;
    [SerializeField] Text elementSkillDes = default;

    public void SetElements(StrategyComposite strategy)
    {
        m_BluePrint = strategy.CompositeBluePrint;
        List<Composition> compositions = m_BluePrint.Compositions;
        for (int i = 0; i < Elements.Length; i++)
        {
            if (i >= compositions.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            TurretAttribute attribute = ConstructHelper.GetElementAttribute((Element)compositions[i].elementRequirement);
            Elements[i].sprite = attribute.TurretLevels[compositions[i].qualityRequeirement - 1].CannonSprite;
        }
        SetIntensifyInfo();
        if (strategy.ElementSkill1 != null)
            elementSkillDes.text = strategy.ElementSkill1.SkillDescription;
        else
            Debug.LogWarning("没有这个元素技能显示TIPS");

    }

    public void SetIntensifyInfo()
    {
        string text = "元素加成：\n";
        for(int i = 0; i < m_BluePrint.Compositions.Count; i++)
        {
            Composition com = m_BluePrint.Compositions[i];
            if(i<2)
                text += StaticData.GetElementIntensifyText((Element)com.elementRequirement, com.qualityRequeirement) + "\n";
            else
                text += StaticData.GetElementIntensifyText((Element)com.elementRequirement, com.qualityRequeirement);
        }

        m_InfoBtn.SetContent(text);
    }

}
