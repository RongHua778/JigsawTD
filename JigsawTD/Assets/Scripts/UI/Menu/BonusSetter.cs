using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSetter : MonoBehaviour
{
    [SerializeField] BonusSlot[] slots = default;

    public void SetInfo()
    {
        List<TurretAttribute> attributes = LevelManager.Instance.CurrentLevel.Bonus;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < attributes.Count)
                slots[i].SetBonusInfo(true, attributes[i]);
            else
                slots[i].SetBonusInfo(false);
        }
    }
}
