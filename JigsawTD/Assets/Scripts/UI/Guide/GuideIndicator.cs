using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideIndicator : MonoBehaviour
{
    Transform followTr;
    bool isShow = false;

    public void Show(bool value, Transform followTr = null)
    {
        isShow = value;
        this.followTr = followTr;
        gameObject.SetActive(isShow);
    }
    private void FixedUpdate()
    {
        if (isShow)
            transform.position = followTr.position;
    }
}
