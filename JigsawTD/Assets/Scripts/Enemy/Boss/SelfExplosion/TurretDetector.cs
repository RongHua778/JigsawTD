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
        if (!Turrets.Contains(target))
        {
            Turrets.Add(target);
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TurretContent target = collision.GetComponent<TurretContent>();
        Turrets.Remove(target);
    }
}
