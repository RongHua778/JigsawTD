using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ReusableObject
{
    [SerializeField] Image healthProgress = default;
    [SerializeField] Image slowIcon = default;
    [SerializeField] Image damageIcon = default;
    [SerializeField] Image promoteIcon = default;

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

    public override void OnUnSpawn()
    {
        FillAmount = 1;
        ShowPromoteIcon(false);
        ShowSlowIcon(false);
        ShowDamageIcon(false);
    }

    public void ShowSlowIcon(bool value)
    {
        slowIcon.gameObject.SetActive(value);
    }

    public void ShowDamageIcon(bool value)
    {
        damageIcon.gameObject.SetActive(value);
    }

    public void ShowPromoteIcon(bool value)
    {
        promoteIcon.gameObject.SetActive(value);
    }
    private void LateUpdate()
    {
        if (followTrans != null)
            transform.position = (Vector2)followTrans.position + offset;
    }
}
