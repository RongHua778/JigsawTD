using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBillboard : MonoBehaviour
{
    [SerializeField] TurretItem turretItemPrefab = default;
    [SerializeField] Transform contentParent = default;
    private List<TurretItem> turrets = new List<TurretItem>();
    public void SetBillBoard()
    {
        TurretItem item;
        foreach (var elementTurret in GameManager.Instance.elementTurrets.behaviors)
        {
            item = Instantiate(turretItemPrefab, contentParent);
            item.SetItemData(elementTurret as TurretContent);
            turrets.Add(item);
        }
        foreach (var refactorTurret in GameManager.Instance.compositeTurrets.behaviors)
        {
            item = Instantiate(turretItemPrefab, contentParent);
            item.SetItemData(refactorTurret as TurretContent);
            turrets.Add(item);
        }
        SortBillBoard();
    }

    private void SortBillBoard()
    {
        TurretItem tempItem;
        for (int i = 0; i < turrets.Count - 1; i++)
        {
            for (int j = 0; j < turrets.Count - 1 - i; j++)
            {
                if (turrets[j].TotalDamage > turrets[j + 1].TotalDamage)
                {
                    tempItem = turrets[j];
                    turrets[j] = turrets[j + 1];
                    turrets[j + 1] = tempItem;
                }
            }
        }
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].transform.SetAsFirstSibling();
            turrets[i].SetRank(turrets.Count - i - 1);
        }
    }
}
