using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacement : MonoBehaviour
{
    [SerializeField]
    TileSelect[] tileSelects = default;
    [SerializeField]
    GameObject selectArea, confirmArea = default;

    public void ShowArea(int areaID)
    {
        switch (areaID)
        {
            case 0:
                selectArea.SetActive(true);
                confirmArea.SetActive(false);
                break;
            case 1:
                selectArea.SetActive(false);
                confirmArea.SetActive(true);
                break;
        }
    }

    public void HideArea(int id)
    {
        switch (id)
        {
            case 0:
                selectArea.SetActive(false);
                break;
            case 1:
                confirmArea.SetActive(false);
                break;
        }
    }

    public void ClearAllSelections()
    {
        foreach (TileSelect select in tileSelects)
        {
            select.ClearShape();
        }
    }

    public void DisplayShapeOnTileSelct(int displayID, TileShape shape)
    {
        tileSelects[displayID].InitializeDisplay(displayID, shape);
    }

}
