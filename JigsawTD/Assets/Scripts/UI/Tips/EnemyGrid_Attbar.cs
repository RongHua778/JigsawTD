using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGrid_Attbar : MonoBehaviour
{
    [SerializeField] GameObject[] energys = default;
    public void SetAtt(int att)
    {
        for (int i = 0; i < energys.Length; i++)
        {
            energys[i].SetActive(i < att);
        }
    }
}
