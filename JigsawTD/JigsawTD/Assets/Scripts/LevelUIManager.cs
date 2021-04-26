using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject messagePanel;
    [SerializeField]
    TMP_Text messageTxt;

    [SerializeField] RoadPlacement _roadPlacament=default;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onMessage += ShowMessage;

    }

    public void DisplayShape(int displayID,TileShape shape)
    {
        _roadPlacament.DisplayShapeOnTileSelct(displayID, shape);
    }

    private void ShowMessage(string content)
    {
        StartCoroutine(MessageCor(content));
    }

    IEnumerator MessageCor(string content)
    {
        messagePanel.SetActive(true);
        messageTxt.text = content;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);
        messageTxt.text = "";
    }

    public void ShowSelections()
    {
        _roadPlacament.ShowSelectionArea();
    }
}
