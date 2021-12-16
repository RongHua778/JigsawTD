using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementBenefitPanel : MonoBehaviour
{
    [SerializeField] Text[] ElementTxt = default;
    Transform root;
    public void InitializePanel(StrategyBase strategy)
    {
        root = transform.Find("Root");
        ElementTxt[0].text = GameMultiLang.GetTraduction(ElementType.GOLD.ToString()) + " " + StaticData.ElementDIC[ElementType.GOLD].GetIntensifyText(strategy.GoldCount);
        ElementTxt[1].text = GameMultiLang.GetTraduction(ElementType.WOOD.ToString()) + " " + StaticData.ElementDIC[ElementType.WOOD].GetIntensifyText(strategy.WoodCount);
        ElementTxt[2].text = GameMultiLang.GetTraduction(ElementType.WATER.ToString()) + " " + StaticData.ElementDIC[ElementType.WATER].GetIntensifyText(strategy.WaterCount);
        ElementTxt[3].text = GameMultiLang.GetTraduction(ElementType.FIRE.ToString()) + " " + StaticData.ElementDIC[ElementType.FIRE].GetIntensifyText(strategy.FireCount);
        ElementTxt[4].text = GameMultiLang.GetTraduction(ElementType.DUST.ToString()) + " " + StaticData.ElementDIC[ElementType.DUST].GetIntensifyText(strategy.DustCount);

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
