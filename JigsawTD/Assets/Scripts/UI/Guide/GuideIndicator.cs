using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideIndicator : MonoBehaviour
{
    Transform followTr;
    bool isShow = false;

    public void Show(bool value, GameObject FollowObj = null)
    {
        isShow = value;
        if (FollowObj != null)
            this.followTr = FollowObj.transform;
        gameObject.SetActive(isShow);
    }
    private void FixedUpdate()
    {
        if (isShow)
            transform.position = followTr.position;
    }
}
