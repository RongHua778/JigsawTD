using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ReusableObject
{
    [SerializeField] Image healthProgress = default;
    [SerializeField] Vector2 offset = default;
    public Transform followTrans;
    float fillAmount;
    public float FillAmount
    {
        get => fillAmount;
        set
        {
            fillAmount = value;
            healthProgress.fillAmount = value;
        }
    }



    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        FillAmount = 1;
    }


    private void LateUpdate()
    {
        if (followTrans != null)
            transform.position = (Vector2)followTrans.position + offset;
    }

}
