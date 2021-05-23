using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacement : MonoBehaviour
{
    [SerializeField]
    TileSelect[] tileSelects = default;
    [SerializeField]
    GameObject[] areas;

    public void ShowArea(int exceptId)
    {
        for(int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
        areas[exceptId].SetActive(true);
    }

    public void HideArea()
    {
        for(int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
    }

    public void ClearAllSelections()
    {
        HideArea();
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
