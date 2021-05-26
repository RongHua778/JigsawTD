using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BluePrintShop : MonoBehaviour
{
    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;

    [SerializeField] Transform shopContent = default;
    public void AddBluePrint()
    {
        GameObject bluePrintObj= ObjectPool.Instance.Spawn(bluePrintGridPrefab.gameObject);
        bluePrintObj.transform.SetParent(shopContent);
        bluePrintObj.GetComponent<BluePrintGrid>().SetElements(shopContent.GetComponent<ToggleGroup>());
    }

}
