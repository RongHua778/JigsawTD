using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;

    public void SetElements(List<Composition> compositions)
    {
        for (int i = 0; i < Elements.Length; i++)
        {
            if (i >= compositions.Count)
            {
                Elements[i].gameObject.SetActive(false);
                continue;
            }
            Elements[i].gameObject.SetActive(true);
            TurretAttribute attribute = StaticData.Instance.GetElementsAttributes((Element)compositions[i].elementRequirement);
            Elements[i].sprite = attribute.TurretLevels[compositions[i].levelRequirement - 1].Icon;
        }
    }
}
