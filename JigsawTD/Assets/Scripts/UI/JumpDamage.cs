using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpDamage : ReusableObject
{
    Animator anim;
    [SerializeField] Text valueTxt = default;
    //[SerializeField] Color normalColor = default;
    //[SerializeField] Color criticalColor = default;
    private Vector2 randomOffset;
    float offset = 0.1f;
    float size;
    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    public void Jump(int amount, Vector2 pos, bool isCritical)
    {
        size = isCritical ? 0.4f : 0.2f;
        transform.localScale *= Mathf.Clamp(size * (Mathf.Log10(amount) + 1), 0.1f, 5f);
        randomOffset = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset));
        transform.position = pos + randomOffset;
        valueTxt.text = amount.ToString();
        //valueTxt.color = isCritical ? criticalColor : normalColor;
        anim.SetTrigger(isCritical ? "Critical" : "Normal");
    }

    public void RecycleObject()
    {
        transform.localScale = Vector3.one;
        ObjectPool.Instance.UnSpawn(this);
    }

}
