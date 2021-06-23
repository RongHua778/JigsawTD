using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }

    [SerializeField] Healer healer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy)
        {
                Enemies.Add(enemy);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy)
        {
            Enemies.Remove(enemy);
            enemy.Speed = enemy.initialSpeed;
        }
    }

}
