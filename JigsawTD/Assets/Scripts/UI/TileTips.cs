using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileTips : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] protected Image Icon = default;
    [SerializeField] protected Text Name = default;
    [SerializeField] protected Text Description = default;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    public void CloseTips()
    {
        anim.SetBool("isOpen", false);
    }


}
