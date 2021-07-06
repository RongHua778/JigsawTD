using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    Enemy enemy;
    public Enemy Enemy { get => enemy; set => enemy = value; }
    List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }


    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = (Enemy)collision.GetComponent<TargetPoint>().Enemy;
        if (enemy != this.Enemy)
        {
            Enemies.Add(enemy);
            enemy.AffectHealerCount += 1;
            enemy.ProgressFactor = enemy.Speed * enemy.Adjust;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = (Enemy)collision.GetComponent<TargetPoint>().Enemy;
        if (enemy != this.Enemy && enemy.gameObject.activeSelf)
        {
            Enemies.Remove(enemy);
            enemy.AffectHealerCount -= 1;
            enemy.ProgressFactor = enemy.Speed * enemy.Adjust;
        }
    }

}
