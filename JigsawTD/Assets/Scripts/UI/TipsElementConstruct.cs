using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;
    ElementSkill m_Skill;
    [SerializeField] InfoBtn m_InfoBtn = default;
    [SerializeField] Text elementSkillName = default;
    [SerializeField] Text elementSkillDes = default;
    [SerializeField] GameObject[] areas = default;
    //[SerializeField] Image intensifyImg = default;
    [SerializeField] Sprite[] elementSprites = default;
    public void SetElements(ElementSkill skill)
    {
        areas[1].SetActive(false);
        areas[0].SetActive(true);
        m_Skill = skill;

        for (int i = 0; i < skill.Elements.Count; i++)
        {
            if (i >= skill.Elements.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            //TurretAttribute attribute = ConstructHelper.GetElementAttribute((Element)skill.Elements[i]);
            Elements[i].sprite = elementSprites[skill.Elements[i]];
        }
        SetIntensifyInfo();

        string key="";
        foreach(var element in skill.Elements)
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
        elementSkillDes.text = GameMultiLang.GetTraduction(key+"INFO");
        //intensifyImg.gameObject.SetActive(strategy.CompositeBluePrint.IntensifyBluePrint);

    }

    public void SetIntensifyInfo()
    {
        string text = StaticData.SetElementSkillInfo(m_Skill);
        m_InfoBtn.SetContent(text);
    }

    public void SetEmpty()//ÏÔÊ¾¿Õ²Û×´Ì¬
    {
        areas[0].SetActive(false);
        areas[1].SetActive(true);
    }
}
