using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite = default;
   // [SerializeField] Collider2D m_Col = default;
    public void ShowSprite(bool show)
    {
        sprite.enabled = show;
    }

    //public void SetCol(bool active)
    //{
    //    m_Col.enabled = active;
    //}


}
