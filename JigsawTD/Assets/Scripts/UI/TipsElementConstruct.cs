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
    //[SerializeField] Image intensifyImg = default;

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
            Elements[i].sprite = attribute.TurretLevels[compositions[i].qualityRequeirement - 1].TurretIcon;
        }
        SetIntensifyInfo();

        //intensifyImg.gameObject.SetActive(strategy.CompositeBluePrint.IntensifyBluePrint);

        if (strategy.ElementSkill != null)
            elementSkillDes.text = GameMultiLang.GetTraduction(strategy.ElementSkill.SkillDescription);
        else
            Debug.LogWarning("没有这个元素技能显示TIPS");

    }

    public void SetIntensifyInfo()
    {
        string text = StaticData.GetBluePrintIntensify(m_BluePrint);
        m_InfoBtn.SetContent(text);
    }

}
