using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpDamage : ReusableObject
{
    Animator anim;
    [SerializeField] Text valueTxt = default;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    public void Jump(int amount,Vector3 pos)
    {
        transform.localScale *= Mathf.Max(0.5f, 0.3f * (Mathf.Log10(amount) + 1));
        transform.position = pos;
        valueTxt.text = amount.ToString();
        anim.SetTrigger("Jump");
    }

    public void RecycleObject()
    {
        transform.localScale = Vector3.one;
        ObjectPool.Instance.UnSpawn(this);
    }

}
