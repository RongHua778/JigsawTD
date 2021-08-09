using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;
    Blueprint m_BluePrint;
    ElementSkill m_Skill;
    [SerializeField] InfoBtn m_InfoBtn = default;
    [SerializeField] Text elementSkillDes = default;
    //[SerializeField] Image intensifyImg = default;

    public void SetElements(ElementSkill skill)
    {
        m_Skill = skill;
        //m_BluePrint = strategy.CompositeBluePrint;
        //List<Composition> compositions = m_BluePrint.Compositions;
        for (int i = 0; i < skill.Elements.Count; i++)
        {
            if (i >= skill.Elements.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            TurretAttribute attribute = ConstructHelper.GetElementAttribute((Element)skill.Elements[i]);
            Elements[i].sprite = attribute.TurretLevels[0].TurretIcon;
        }
        //SetIntensifyInfo();
        elementSkillDes.text = GameMultiLang.GetTraduction(skill.SkillDescription);
        //intensifyImg.gameObject.SetActive(strategy.CompositeBluePrint.IntensifyBluePrint);

        //if (strategy.ElementSkill != null)
        //    elementSkillDes.text = GameMultiLang.GetTraduction(strategy.ElementSkill.SkillDescription);
        //else
        //    Debug.LogWarning("没有这个元素技能显示TIPS");

    }

    public void SetIntensifyInfo()
    {
        string text = StaticData.GetBluePrintIntensify(m_BluePrint);
        m_InfoBtn.SetContent(text);
    }

}
