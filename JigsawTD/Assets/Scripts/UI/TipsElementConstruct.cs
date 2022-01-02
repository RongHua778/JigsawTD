using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;
    [SerializeField] TextMeshProUGUI elementSkillName = default;
    [SerializeField] TextMeshProUGUI elementSkillDes = default;
    [SerializeField] GameObject[] areas = default;
    [SerializeField] InfoBtn emptyInfo = default;
    private string SkillDes;
    private ElementSkill m_Skill;


    private void Start()
    {
        emptyInfo.SetContent(GameMultiLang.GetTraduction("EMPTYSLOT"));

    }
    public void SetElements(ElementSkill skill)
    {
        areas[1].SetActive(false);//¿Õ×´Ì¬
        areas[0].SetActive(true);//±ê×¼×´Ì¬
        m_Skill = skill;

        //ÉèÖÃÔªËØÏÔÊ¾
        for (int i = 0; i < skill.Elements.Count; i++)
        {
            if (i >= skill.Elements.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            Elements[i].sprite = StaticData.Instance.ElementSprites[skill.Elements[i]];
        }
        //ÃèÊö¼°Ãû³ÆÉèÖÃ
        string key = "";
        foreach (var element in skill.Elements)
        {
            key += StaticData.ElementDIC[(ElementType)element].GetElementName;
        }
        SkillDes = GameMultiLang.GetTraduction(key + "INFO") + StaticData.ElementDIC[m_Skill.IntensifyElement].GetExtraInfo;
        elementSkillName.text = GameMultiLang.GetTraduction(key);
        UpdateDes();
    }

    public void UpdateDes()
    {
        if (m_Skill != null)
            elementSkillDes.text = string.Format(SkillDes,
            "<b>" + m_Skill.DisplayValue + "</b>");
    }

    public void SetEmpty()//ÏÔÊ¾¿Õ²Û×´Ì¬
    {
        m_Skill = null;
        areas[0].SetActive(false);
        areas[1].SetActive(true);
    }


}
