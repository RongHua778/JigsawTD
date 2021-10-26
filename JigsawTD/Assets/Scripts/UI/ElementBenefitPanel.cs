using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementBenefitPanel : MonoBehaviour
{
    [SerializeField] Text[] ElementTxt = default;
    [SerializeField] Image[] ElementIcon = default;
    [SerializeField] Sprite[] elementSprites = default;
    Transform root;
    ElementSkill m_Skill;
    public void InitializePanel(ElementSkill skill)
    {
        root = transform.Find("Root");
        m_Skill = skill;
        for (int i = 0; i < ElementTxt.Length; i++)
        {
            ElementType element = (ElementType)m_Skill.Elements[i];
            ElementTxt[i].text = GameMultiLang.GetTraduction(element.ToString()) + StaticData.GetElementIntensifyText(element);
            ElementIcon[i].sprite = elementSprites[(int)element];
        }
    }

    public void Show()
    {
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }

}
