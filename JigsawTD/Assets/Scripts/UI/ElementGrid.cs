using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementGrid : MonoBehaviour
{
    [SerializeField] Image Img_Icon = default;
    [SerializeField] Text Txt_ElementName = default;
    [SerializeField] Color UnobtainColor = default;
    [SerializeField] GameObject previewGlow = default;
    [SerializeField] GameObject perfectIcon = default;
    private Element element;
    private int quality;

    public void SetElement(Composition composition)
    {
        element = (Element)composition.elementRequirement;
        quality = composition.qualityRequeirement;
        TurretAttribute attribute = ConstructHelper.GetElementAttribute(element);
        Img_Icon.sprite = attribute.TurretLevels[quality - 1].TurretIcon;
        Txt_ElementName.text = attribute.TurretLevels[quality - 1].TurretName.Substring(0, 2);

        if (composition.isPerfect)
            perfectIcon.SetActive(true);
        else
            perfectIcon.SetActive(false);

        if (composition.obtained)
            Img_Icon.color = Color.white;
        else
            Img_Icon.color = UnobtainColor;
    }



    public void SetPreview(bool value, Element element, int quality)
    {
        if (!value)
        {
            previewGlow.SetActive(false);
            return;
        }
        if (this.element == element && this.quality == quality)
        {
            previewGlow.SetActive(true);
        }
        else
        {
            previewGlow.SetActive(false);
        }
    }

}
