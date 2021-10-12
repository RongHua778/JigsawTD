using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BluePrintGrid : ReusableObject//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static BluePrintGrid SelectingBluePrint = null;
    [SerializeField] Color UnobtainColor = default;
    private bool isLock = false;
    public bool IsLock { get => isLock; set => isLock = value; }
    private bool inShop = false;
    public bool InShop { get => inShop; set => inShop = value; }


    [SerializeField] Text compositeName = default;
    [SerializeField] Image compositeIcon = default;
    [SerializeField] ElementGrid[] elementGrids = default;
    [SerializeField] Toggle m_LockToggle = default;
    private Toggle m_Toggle;
    private BluePrintShopUI m_Shop;
    private Blueprint m_BluePrint;

    public Blueprint BluePrint { get => m_BluePrint; set => m_BluePrint = value; }

    public void SetElements(BluePrintShopUI shop, ToggleGroup group, Blueprint bluePrint)
    {
        m_Shop = shop;
        m_Toggle = this.GetComponent<Toggle>();
        BluePrint = bluePrint;
        BluePrint.CheckElement();
        m_Toggle.group = group;
        compositeName.text = bluePrint.CompositeTurretAttribute.Name;
        compositeIcon.sprite = bluePrint.CompositeTurretAttribute.TurretLevels[0].TurretIcon;
        //给每一个组件设置图片
        RefreshElementsSprite();

    }

    public void PreviewElement(bool value, ElementType element, int quality)
    {
        foreach (var elementGrid in elementGrids)
        {
            elementGrid.SetPreview(value, element, quality);
        }
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
        compositeIcon.color = BluePrint.CheckBuildable() ? Color.white : UnobtainColor;
    }


    public void OnBluePrintSelect()
    {
        if (m_Toggle.isOn)
        {
            if (SelectingBluePrint != null)
                SelectingBluePrint.OnBluePrintDeselect();
            SelectingBluePrint = this;
            GameManager.Instance.ShowBluePrintTips(this);
            Sound.Instance.PlayEffect("Sound_Click");
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

    public void OnLock(bool value)
    {
        IsLock = value;
        m_Shop.OnLockGrid(this, value);
    }

    public void CheckElements()
    {
        BluePrint.CheckElement();
        RefreshElementsSprite();
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    m_Toggle.isOn = true;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    m_Toggle.isOn = false;
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.clickCount == 2)
    //    {
    //        GameManager.Instance.CompositeShape(this);
    //    }
    //}

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        IsLock = false;
        m_LockToggle.isOn = false;
    }

    public void ShowLockBtn(bool value)
    {
        m_LockToggle.gameObject.SetActive(value);
    }
}
