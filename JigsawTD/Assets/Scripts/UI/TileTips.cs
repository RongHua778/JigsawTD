using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileTips : MonoBehaviour
{
    [SerializeField] protected Image Icon = default;
    [SerializeField] protected TMP_Text Name = default;
    [SerializeField] protected TMP_Text Description = default;
    [SerializeField] protected GameObject[] LevelSlots = default;
    public virtual void Hide()
    {
        Destroy(this.gameObject);
    }


}
