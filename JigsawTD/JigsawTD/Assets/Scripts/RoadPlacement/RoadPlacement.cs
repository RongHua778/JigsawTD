using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacement : MonoBehaviour
{
    [SerializeField]
    TileSelect[] tileSelects = default;
    [SerializeField]
    GameObject selectArea = default;
    void Start()
    {

    }

    public void ShowSelectionArea()
    {
        selectArea.SetActive(true);
    }
    public void HideSelections()
    {
        selectArea.SetActive(false);
    }

    public void ClearAllSelections()
    {
        foreach(TileSelect select in tileSelects)
        {
            select.ClearShape();
        }
    }
    public void DisplayShapeOnTileSelct(int displayID,TileShape shape)
    {
        tileSelects[displayID].InitializeDisplay(displayID, shape);
    }

}
