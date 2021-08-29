using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareInfoSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] quilityStars = default;

    public void SetRare(int quality)
    {
        foreach (var obj in quilityStars)
        {
            obj.SetActive(false);
        }
        for(int i = 0; i < quality; i++)
        {
            quilityStars[i].SetActive(true);
        }
    }
}
