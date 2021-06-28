using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ReusableObject
{
    [SerializeField] Image healthProgress = default;
    [SerializeField] Image slowIcon = default;
    [SerializeField] Vector2 offset = default;
    [SerializeField] JumpDamage jumpDamagePrefab = default;
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

    public void ShowSlowIcon(bool value)
    {
        slowIcon.gameObject.SetActive(value);
    }
    private void LateUpdate()
    {
        if (followTrans != null)
            transform.position = (Vector2)followTrans.position + offset;
    }


    public void ShowJumpDamage(int amount)
    {
        JumpDamage obj = ObjectPool.Instance.Spawn(jumpDamagePrefab) as JumpDamage;
        obj.transform.localScale *= Mathf.Max(0.5f, 0.3f * (Mathf.Log10(amount) + 1));
        obj.transform.position = followTrans.position;
        obj.Jump(amount);
    }
}
