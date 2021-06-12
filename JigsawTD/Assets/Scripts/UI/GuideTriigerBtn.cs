using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideTriigerBtn : MonoBehaviour
{
    [SerializeField] int guideIndex = default;

    public void OnGuideBtnClick()
    {
        GameEvents.Instance.GuideTrigger(guideIndex);
    }


}
