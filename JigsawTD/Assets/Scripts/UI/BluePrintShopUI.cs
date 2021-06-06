using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class BluePrintShopUI : IUserInterface
{
    bool Showing = false;//���ƶ���
    Animator anim;

    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();//�̵��䷽��
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();//ӵ���䷽��

    int shopCapacity = 3;
    int nextRefreshTrun = 0;
    public int NextRefreshTrun //�´��Զ�ˢ�»غ�
    {
        get => nextRefreshTrun;
        set
        {
            nextRefreshTrun = value;
            if (nextRefreshTrun <= 0)
            {
                nextRefreshTrun = 3;
                GameManager.Instance.RefreshShop(0);
            }
            NextRefreshTurnsTxt.text = nextRefreshTrun + "�غϺ�ˢ��";
        }
    }

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        anim = this.GetComponent<Animator>();
        NextRefreshTrun = 3;
    }

    public void RefreshShop(int level, int cost)//ˢ���̵�
    {
        foreach (var grid in ShopBluePrints.ToList())
        {
            RemoveGrid(grid);
        }
        ShopBluePrints.Clear();
        for (int i = 0; i < shopCapacity; i++)
        {
            Blueprint bluePrint = ConstructHelper.GetRandomBluePrintByLevel(level);
            AddBluePrint(bluePrint, true);
        }
    }

    public void AddBluePrint(Blueprint bluePrint, bool isShopBluePrint)//������ͼ��isShopblueprint�жϼ����̵껹��ӵ��
    {
        GameObject bluePrintObj = ObjectPool.Instance.Spawn(bluePrintGridPrefab.gameObject);
        bluePrintObj.transform.SetParent(shopContent);
        BluePrintGrid grid = bluePrintObj.GetComponent<BluePrintGrid>();
        grid.SetElements(shopContent.GetComponent<ToggleGroup>(), bluePrint);
        if (isShopBluePrint)
        {
            bluePrintObj.transform.SetAsFirstSibling();
            grid.InShop = true;
            ShopBluePrints.Add(grid);
        }
        else
        {
            MoveBluePrintToPocket(grid);
        }
    }

    public void ShopBtnClick()//�����̵����򿪶���
    {
        Showing = !Showing;
        if (Showing)
            Show();
        else
            Hide();
    }

    public override void Hide()
    {
        anim.SetBool("Showing", false);
    }

    public override void Show()
    {
        anim.SetBool("Showing", true);
    }

    public void MoveBluePrintToPocket(BluePrintGrid grid)//���̵��䷽����ӵ��
    {
        grid.InShop = false;
        grid.transform.SetAsLastSibling();
        OwnBluePrints.Add(grid);
        if (ShopBluePrints.Contains(grid))
            ShopBluePrints.Remove(grid);
        grid.transform.SetAsLastSibling();
    }

    public void RefreshBtnClick()
    {
        GameManager.Instance.RefreshShop(20);
    }



    public void CompositeBluePrint(BluePrintGrid grid)//�ϳɶ�Ӧ���䷽
    {
        if (grid.BluePrint.CheckBuildable())
        {
            grid.BluePrint.BuildBluePrint();
            ConstructHelper.GetCompositeTurretByBluePrint(grid.BluePrint);
            RemoveGrid(grid);
            CheckAllBluePrint();
            Hide();
            GameManager.Instance.HideTips();
            //���ý�����Ϣ
            //GameEndPanel.TotalComposite++;
        }
        else
        {
            GameManager.Instance.ShowMessage("�ϳ������زĲ���");
        }

    }

    private void RemoveGrid(BluePrintGrid grid)//�Ƴ���Ӧ���䷽���������б�
    {
        if (ShopBluePrints.Contains(grid))
        {
            ShopBluePrints.Remove(grid);
        }
        if (OwnBluePrints.Contains(grid))
        {
            OwnBluePrints.Remove(grid);
        }
        ObjectPool.Instance.UnSpawn(grid.gameObject);
    }

    public void CheckAllBluePrint()//��������䷽�Ƿ��ɺϳ�����
    {
        foreach (var bluePrint in ShopBluePrints)
        {
            bluePrint.CheckElements();
        }
        foreach (var bluePrint in OwnBluePrints)
        {
            bluePrint.CheckElements();
        }
    }

    public void GetARandomBluePrintToPocket(int level)//����ֵ����������һ���䷽
    {
        Blueprint bluePrint = ConstructHelper.GetRandomBluePrintByLevel(level);
        AddBluePrint(bluePrint, false);
    }

}
