using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluePrintGrid : MonoBehaviour
{
    public static BluePrintGrid SelectingBluePrint = null;

    [SerializeField] ElementGrid compositeGrid = default;
    [SerializeField] ElementGrid[] elementGrids = default;
    [SerializeField] private GameTile compositeTurretTile = default;
    [SerializeField] Toggle toggle = default;
    public void SetElements(ToggleGroup group)
    {
        toggle.group = group;
        //给每一个组件设置图片
    }

    public void OnBluePrintSelect()
    {
        if (toggle.isOn)
        {
            if (SelectingBluePrint != null)
                SelectingBluePrint.OnBluePrintDeselect();
            SelectingBluePrint = this;
            //GameEvents.Instance.ShowTileTips(compositeTurretTile);
        }
        else
        {

        }
    }

    public void OnBluePrintDeselect()
    {
        //toggle.isOn = false;
    }

}
