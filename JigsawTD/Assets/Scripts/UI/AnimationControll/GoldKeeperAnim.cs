using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GoldKeeperAnim : IUserInterface
{
    [SerializeField] Material lineMaterial = default;
    [SerializeField] Material speedMaterial = default;
    [SerializeField] Material bonusMaterial = default;
    private Animator anim;
    private Vector2 flowSpeed = new Vector2(0.2f, 0);
    void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        anim.SetTrigger("Come");
        Sound.Instance.PlayEffect("Sound_GoldKeeper");
        StartCoroutine(BonusAnim());
    }

    IEnumerator BonusAnim()
    {
        bonusMaterial.SetFloat("_ShineLocation", 0f);
        yield return new WaitForSeconds(1f);
        bonusMaterial.DOFloat(1f, "_ShineLocation", 1.5f);
        yield return new WaitForSeconds(1f);

    }



    // Update is called once per frame
    private void FixedUpdate()
    {
        if (m_Active)
        {
            lineMaterial.mainTextureOffset += flowSpeed * Time.deltaTime;
            speedMaterial.mainTextureOffset -= flowSpeed * 4 * Time.deltaTime;
        }
    }

}
