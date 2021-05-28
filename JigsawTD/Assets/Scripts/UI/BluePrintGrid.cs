using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluePrintGrid : MonoBehaviour
{
    public static BluePrintGrid SelectingBluePrint = null;

    [SerializeField] Image compositeIcon = default;
    [SerializeField] ElementGrid[] elementGrids = default;
    private Toggle toggle;

    private Blueprint m_BluePrint;
    private BluePrintShop m_Shop;
    public bool BuildAble = false;

    public Blueprint BluePrint { get => m_BluePrint; set => m_BluePrint = value; }

    public void SetElements(BluePrintShop shop, ToggleGroup group, Blueprint bluePrint)
    {
        m_Shop = shop;
        toggle = this.GetComponent<Toggle>();
        BluePrint = bluePrint;
        BluePrint.CheckElement();
        toggle.group = group;
        compositeIcon.sprite = bluePrint.CompositeTurretAttribute.TurretLevels[0].Icon;
        //��ÿһ���������ͼƬ
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
        BuildAble = BluePrint.CanBuild;
        compositeIcon.color = BuildAble ? Color.white : Color.gray;
    }

    public void MoveToPocket()
    {
        m_Shop.MoveBluePrintToPocket(this);
    }

    public void OnBluePrintSelect()
    {
        if (toggle.isOn)
        {
            if (SelectingBluePrint != null)
                SelectingBluePrint.OnBluePrintDeselect();
            SelectingBluePrint = this;
            GameEvents.Instance.ShowTurretTips(this);
        }
        else
        {
            if (SelectingBluePrint == this)
                OnBluePrintDeselect();
        }

    }

    public void OnBluePrintDeselect()
    {
        GameEvents.Instance.HideTips();
    }

    public void RemoveBuildPrint()
    {
        if (m_Shop.ShopBluePrints.Contains(this))
        {
            m_Shop.ShopBluePrints.Remove(this);
        }
        if (m_Shop.OwnBluePrints.Contains(this))
        {
            m_Shop.OwnBluePrints.Remove(this);
        }
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

    public void CheckElements()
    {
        BluePrint.CheckElement();
        RefreshElementsSprite();
    }
}