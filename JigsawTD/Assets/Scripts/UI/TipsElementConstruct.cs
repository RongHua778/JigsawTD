using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TipsElementConstruct : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool canPreview = false;
    [SerializeField] Image[] Elements = default;
    ElementSkill m_Skill;
    [SerializeField] Text elementSkillName = default;
    [SerializeField] Text elementSkillDes = default;
    [SerializeField] GameObject[] areas = default;
    //[SerializeField] Image intensifyImg = default;
    //[SerializeField] Sprite[] elementSprites = default;
    [SerializeField] ElementBenefitPanel benefitPanel = default;
    public void SetElements(ElementSkill skill)
    {
        areas[1].SetActive(false);
        areas[0].SetActive(true);
        m_Skill = skill;
        benefitPanel.InitializePanel(m_Skill);
        canPreview = true;
        for (int i = 0; i < skill.Elements.Count; i++)
        {
            if (i >= skill.Elements.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            Elements[i].sprite =StaticData.Instance.ElementSprites[skill.Elements[i]];
        }
        //SetIntensifyInfo();

        string key = "";
        foreach (var element in skill.Elements)
        {
            switch (element)
            {
                case 0:
                    key += "A";
                    break;
                case 1:
                    key += "B";
                    break;
                case 2:
                    key += "C";
                    break;
                case 3:
                    key += "D";
                    break;
                case 4:
                    key += "E";
                    break;
            }
        }
        elementSkillName.text = GameMultiLang.GetTraduction(key);
        elementSkillDes.text = GameMultiLang.GetTraduction(key + "INFO");
    }

    public void SetEmpty()//ÏÔÊ¾¿Õ²Û×´Ì¬
    {
        areas[0].SetActive(false);
        areas[1].SetActive(true);
        canPreview = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canPreview)
        {
            benefitPanel.Show();
            GameEvents.Instance.TutorialTrigger(TutorialType.ElementBenefitEnter);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canPreview)
            benefitPanel.Hide();
    }
}
