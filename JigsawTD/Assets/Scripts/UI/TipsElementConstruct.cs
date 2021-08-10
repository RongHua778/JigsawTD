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
        SetIntensifyInfo();
        elementSkillDes.text = GameMultiLang.GetTraduction(skill.SkillDescription);
        //intensifyImg.gameObject.SetActive(strategy.CompositeBluePrint.IntensifyBluePrint);

    }

    public void SetIntensifyInfo()
    {
        string text = StaticData.SetElementSkillInfo(m_Skill);
        m_InfoBtn.SetContent(text);
    }

}
