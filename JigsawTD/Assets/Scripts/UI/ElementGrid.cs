using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementGrid : MonoBehaviour
{
    [SerializeField] Image Img_Icon = default;
    [SerializeField] Text Txt_ElementName = default;

    public void SetElement(TurretAttribute attribute)
    {
        Img_Icon.sprite = attribute.TurretLevels[0].Icon;
        Txt_ElementName.text = attribute.TurretLevels[0].TurretName;
    }

}
