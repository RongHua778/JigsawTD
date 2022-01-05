using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITujian_ListHolder : MonoBehaviour
{
    [SerializeField] ContentAttribute[] attributes = default;
    [SerializeField] ItemSlot itemSlotPrefab = default;
    Transform itemParent;


    public void SetContent()
    {
        itemParent = transform.Find("ListPanel");
        foreach (var item in attributes)
        {
            ItemSlot slot = Instantiate(itemSlotPrefab, itemParent);
            slot.SetContent(item);
        }
    }

}
