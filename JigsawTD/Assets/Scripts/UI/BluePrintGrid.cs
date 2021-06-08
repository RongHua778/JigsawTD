using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluePrintGrid : ReusableObject
{
    public static BluePrintGrid SelectingBluePrint = null;
    private bool inShop=false;
    public bool InShop { get => inShop; set => inShop = value; }

    [SerializeField] Text compositeName = default;
    [SerializeField] Image compositeIcon = default;
    [SerializeField] ElementGrid[] elementGrids = default;
    private Toggle toggle;
    private Blueprint m_BluePrint;

    public Blueprint BluePrint { get => m_BluePrint; set => m_BluePrint = value; }


    public void SetElements(ToggleGroup group, Blueprint bluePrint)
    {
        toggle = this.GetComponent<Toggle>();
        BluePrint = bluePrint;
        BluePrint.CheckElement();
        toggle.group = group;
        compositeName.text = bluePrint.CompositeTurretAttribute.Name;
        compositeIcon.sprite = bluePrint.CompositeTurretAttribute.TurretLevels[0].Icon;
        //给每一个组件设置图片
        RefreshElementsSprite();

    }

    private void RefreshElementsSprite()
    {
        for (int i = 0; i < elementGrids.Length; i++)
        {
            if (i < BluePrint.CompositeTurretAttribute.elementNumber)
            {
                elementGrids[i].gameObject.SetActive(true);
                elementGrids[i].SetElement(BluePrint.Compositions[i]);
            }
            else
            {
                elementGrids[i].gameObject.SetActive(false);
            }
        }
        compositeIcon.color = BluePrint.CheckBuildable() ? Color.white : Color.gray;
    }


    public void OnBluePrintSelect()
    {
        if (toggle.isOn)
        {
            if (SelectingBluePrint != null)
                SelectingBluePrint.OnBluePrintDeselect();
            SelectingBluePrint = this;
            GameManager.Instance.ShowBluePrintTips(this);
        }
        else
        {
            if (SelectingBluePrint == this)
                OnBluePrintDeselect();
        }

    }

    public void OnBluePrintDeselect()
    {
        GameManager.Instance.HideTips();
    }

    public void CheckElements()
    {
        BluePrint.CheckElement();
        RefreshElementsSprite();
    }
}
