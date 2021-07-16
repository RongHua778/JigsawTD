using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckProgress : MonoBehaviour
{
    [SerializeField] GameObject[] luckSlot = default;
    public void SetProgress(int value)
    {
        for (int i = 0; i < luckSlot.Length; i++)
        {
            if (i < value)
            {
                luckSlot[i].SetActive(true);
            }
            else
            {
                luckSlot[i].SetActive(false);
            }
        }
    }
}
