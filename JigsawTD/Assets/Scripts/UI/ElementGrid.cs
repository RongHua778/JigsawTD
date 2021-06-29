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
    private Element element;
    private int quality;

    public void SetElement(Composition composition)
    {
        element = (Element)composition.elementRequirement;
        quality = composition.qualityRequeirement;
        TurretAttribute attribute = ConstructHelper.GetElementAttribute(element);
        Img_Icon.sprite = attribute.TurretLevels[quality - 1].CannonSprite;
        Txt_ElementName.text = attribute.TurretLevels[quality - 1].TurretName;

        //switch (element)
        //{
        //    case Element.Gold:
        //        Txt_ElementName.color = StaticData.YellowColor;
        //        break;
        //    case Element.Wood:
        //        Txt_ElementName.color = StaticData.GreenColor;
        //        break;
        //    case Element.Water:
        //        Txt_ElementName.color = StaticData.BlueColor;
        //        break;
        //    case Element.Fire:
        //        Txt_ElementName.color = StaticData.RedColor;
        //        break;
        //    case Element.Dust:
        //        Txt_ElementName.color = StaticData.PurpleColor;
        //        break;
        //}

        if (composition.obtained)
            Img_Icon.color = Color.white;
        else
            Img_Icon.color = UnobtainColor;
    }



    public void SetPreview(bool value, Element element,int quality)
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
