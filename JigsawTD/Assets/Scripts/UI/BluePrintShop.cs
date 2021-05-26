using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BluePrintShop : MonoBehaviour
{
    bool Showing = false;
    Animator anim;
    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;

    [SerializeField] Transform shopContent = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        RefreshShop();
    }

    public void RefreshShop()
    {
        foreach(var grid in ShopBluePrints)
        {
            Destroy(grid.gameObject);
        }
        ShopBluePrints.Clear();
        for (int i = 0; i < 3; i++)
        {
            TurretAttribute compositeTurret = StaticData.Instance.GetRandomCompositionTurret();
            Blueprint bluePrint = PlayerManager.Instance.GetSingleBluePrint(compositeTurret);
            AddBluePrint(bluePrint);
        }
    }

    public void AddBluePrint(Blueprint bluePrint)
    {
        GameObject bluePrintObj = ObjectPool.Instance.Spawn(bluePrintGridPrefab.gameObject);
        bluePrintObj.transform.SetParent(shopContent);
        bluePrintObj.transform.SetAsFirstSibling();
        bluePrintObj.GetComponent<BluePrintGrid>().SetElements(shopContent.GetComponent<ToggleGroup>(),bluePrint);
        ShopBluePrints.Add(bluePrintObj.GetComponent<BluePrintGrid>());
    }

    public void ShopBtnClick()
    {
        Showing = !Showing;
        anim.SetBool("Showing", Showing);
    }



}
