using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBtn : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] LevelInfoPanel infoPanel = default;

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.Hide();
    }
}
