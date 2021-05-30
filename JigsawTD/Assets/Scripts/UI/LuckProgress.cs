using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckProgress : MonoBehaviour
{
    [SerializeField] Image luckProgress = default;
    public void SetProgress(int value)
    {
        luckProgress.fillAmount = (float)value / 10f;
    }
}
