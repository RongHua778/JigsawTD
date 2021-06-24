using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;
    Blueprint m_BluePrint;
    [SerializeField] InfoBtn m_InfoBtn = default;

    public void SetElements(Blueprint bluePrint)
    {
        m_BluePrint = bluePrint;
        List<Composition> compositions = bluePrint.Compositions;
        for (int i = 0; i < Elements.Length; i++)
        {
            if (i >= compositions.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            TurretAttribute attribute = ConstructHelper.GetElementAttribute((Element)compositions[i].elementRequirement);
            Elements[i].sprite = attribute.TurretLevels[compositions[i].qualityRequeirement - 1].CannonSprite;
        }
        SetIntensifyInfo();
    }

    public void SetIntensifyInfo()
    {
        string text = "";
        foreach (var com in m_BluePrint.Compositions)
        {
            text += StaticData.GetElementIntensifyText((Element)com.elementRequirement, com.qualityRequeirement) + "\n";
        }
        m_InfoBtn.SetContent(text);
    }

}
