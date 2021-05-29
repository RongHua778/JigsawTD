using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileTips : MonoBehaviour
{
    [SerializeField] protected Image Icon = default;
    [SerializeField] protected Text Name = default;
    [SerializeField] protected Text Description = default;
    public virtual void Hide()
    {
        Destroy(this.gameObject);
    }


}
