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
    public void SetElements(ToggleGroup group, Blueprint bluePrint)
    {
        toggle = this.GetComponent<Toggle>();
        m_BluePrint = bluePrint;
        toggle.group = group;
        compositeIcon.sprite = bluePrint.CompositeTurretAttribute.TurretLevels[0].Icon;
        //给每一个组件设置图片
        for (int i = 0; i < elementGrids.Length; i++)
        {
            if (i < bluePrint.CompositeTurretAttribute.elementNumber)
            {
                elementGrids[i].gameObject.SetActive(true);
                elementGrids[i].SetElement(bluePrint.Compositions[i]);
                bluePrint.CheckElement();
            }
            else
            {
                elementGrids[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnBluePrintSelect()
    {
        if (toggle.isOn)
        {
            if (SelectingBluePrint != null)
                SelectingBluePrint.OnBluePrintDeselect();
            SelectingBluePrint = this;
            GameEvents.Instance.ShowTurretTips(m_BluePrint.CompositeTurretAttribute);
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

}
