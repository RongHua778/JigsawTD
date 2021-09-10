using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossComeAnim : IUserInterface
{
    private Animator anim;
    [SerializeField] Image lineMaterial1 = default;

    private Vector2 flowSpeed = new Vector2(0.2f, 0);
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        anim.SetTrigger("BossCome");
        Sound.Instance.PlayEffect("Sound_Warning");
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        lineMaterial1.material.mainTextureOffset += flowSpeed * Time.deltaTime;
    }
}
