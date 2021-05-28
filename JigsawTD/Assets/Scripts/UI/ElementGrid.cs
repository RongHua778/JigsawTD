using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementGrid : MonoBehaviour
{
    [SerializeField] Image Img_Icon = default;
    [SerializeField] Text Txt_ElementName = default;
    [SerializeField] Color UnobtainColor = default;

    public void SetElement(Composition composition)
    {
        TurretAttribute attribute = GameManager.Instance.GetElementAttribute((Element)composition.elementRequirement);
        Img_Icon.sprite = attribute.TurretLevels[composition.levelRequirement - 1].Icon;
        Txt_ElementName.text = attribute.TurretLevels[composition.levelRequirement - 1].TurretName;
        if (composition.obtained)
            Img_Icon.color = Color.white;
        else
            Img_Icon.color = UnobtainColor;
    }

}
