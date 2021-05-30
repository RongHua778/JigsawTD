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

    public void Jump(int amount)
    {
        valueTxt.text = amount.ToString();
        anim.SetTrigger("Jump");
    }

    public void RecycleObject()
    {
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

}
