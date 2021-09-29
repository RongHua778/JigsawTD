using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusSlot : MonoBehaviour
{
    [SerializeField] Image icon = default;
    [SerializeField] Text nameTxt = default;
    public void SetBonusInfo(bool value, TurretAttribute attribute = null)
    {
        if (attribute != null)
        {
            icon.sprite = attribute.Icon;
            nameTxt.text = GameMultiLang.GetTraduction(attribute.Name);
        }
        gameObject.SetActive(value);
    }

}
