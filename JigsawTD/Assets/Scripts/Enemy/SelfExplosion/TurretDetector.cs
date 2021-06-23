using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetector : MonoBehaviour
{

    List<TurretContent> turrets = new List<TurretContent>();

    public List<TurretContent> Turrets { get => turrets; set => turrets = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TurretContent target = collision.GetComponent<TurretContent>();
        if (target)
        {


            if (!Turrets.Contains(target))
            {
                Turrets.Add(target);
                //Debug.Log("got!" + Turrets.Count);
                //Debug.LogWarning("hehe");
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TurretContent target = collision.GetComponent<TurretContent>();
        if (target)
        {
            Turrets.Remove(target);
            //Debug.Log("got!" + Turrets.Count);
        }

    }
}
