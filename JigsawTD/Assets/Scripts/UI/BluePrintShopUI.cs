using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class BluePrintShopUI : IUserInterface
{
    private bool isRefreshing = false;
    bool Showing = false;//���ƶ���
    Animator anim;
    public GameObject ShopBtnObj;

    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;
    [SerializeField] Text PerfectElementTxt = default;
    [SerializeField] Text LockCountTxt = default;
    [SerializeField] InfoBtn PerfectInfo = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();//�̵��䷽��
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();//ӵ���䷽��

    int currentLock = 0;
    private TempWord refreshShopTrigger;
    public int CurrentLock
    {
        get => currentLock;
        set
        {
            currentLock = value;
            LockCountTxt.text = GameMultiLang.GetTraduction("SHOPBLUEPRINT") + CurrentLock.ToString() + "/" + GameRes.MaxLock.ToString();
        }
    }

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
            NextRefreshTurnsTxt.text = GameMultiLang.GetTraduction("NEXTREFRESH") + ":" + nextRefreshTrun + GameMultiLang.GetTraduction("WAVE");
        }
    }



    public override void Initialize()
    {
        anim = this.GetComponent<Animator>();
        NextRefreshTrun = 4;
        SetPerfectElementCount(0);
        PerfectInfo.SetContent(GameMultiLang.GetTraduction("PERFECTINFO"));
        CurrentLock = 0;

        refreshShopTrigger = new TempWord(TempWordType.RefreshShop, 0);
    }

    public void PrepareForGuide()
    {
        ShopBtnObj.SetActive(false);
    }

    public void SetPerfectElementCount(int count)
    {
        //PerfectElementTxt.text = GameMultiLang.GetTraduction("OWNPERFECT") + ":" + count;
        PerfectElementTxt.text = count.ToString();
    }

    public void RefreshShop(int cost)//ˢ���̵�
    {
        if (isRefreshing)
            return;
        if (!GameManager.Instance.ConsumeMoney(cost))
            return;
        if (cost != 0)//�Զ�ˢ�²������԰�
            GameEvents.Instance.TempWordTrigger(refreshShopTrigger);
        GameManager.Instance.HideTips();//���ѡ�����Ѿ��򿪵��䷽��رգ����޸�����
        NextRefreshTrun = 3;
        int lockNum = 0;
        foreach (var grid in ShopBluePrints.ToList())
        {
            if (!grid.IsLock)
            {
                RemoveGrid(grid);
            }
            else
            {
                lockNum++;
            }
        }
        StartCoroutine(RefreshShopCor(lockNum));
    }

    private IEnumerator RefreshShopCor(int lockNum)
    {
        isRefreshing = true;
        for (int i = 0; i < GameRes.ShopCapacity - lockNum; i++)
        {
            Blueprint bluePrint = ConstructHelper.GetRandomBluePrintByLevel();
            AddBluePrint(bluePrint, true);
            yield return new WaitForSeconds(0.02f);
        }
        isRefreshing = false;
    }


    public void AddBluePrint(Blueprint bluePrint, bool isShopBluePrint)//������ͼ��isShopblueprint�жϼ����̵껹��ӵ��
    {
        BluePrintGrid bluePrintGrid = ObjectPool.Instance.Spawn(bluePrintGridPrefab) as BluePrintGrid;
        bluePrintGrid.transform.SetParent(shopContent);
        bluePrintGrid.transform.localScale = Vector3.one;
        bluePrintGrid.transform.localPosition = Vector3.zero;
        bluePrintGrid.SetElements(this, shopContent.GetComponent<ToggleGroup>(), bluePrint);
        if (isShopBluePrint)
        {
            //bluePrintGrid.transform.SetAsFirstSibling();
            bluePrintGrid.ShowLockBtn(CurrentLock < GameRes.MaxLock);
            bluePrintGrid.InShop = true;
            ShopBluePrints.Add(bluePrintGrid);
        }
        else
        {
            MoveBluePrintToPocket(bluePrintGrid);
        }
    }

    public void OnLockGrid(bool isLock)
    {
        if (isLock)
        {
            CurrentLock++;
            if (CurrentLock >= GameRes.MaxLock)
            {
                foreach (var grid in ShopBluePrints)
                {
                    if (!grid.IsLock)
                    {
                        grid.ShowLockBtn(false);
                    }
                }
            }
        }
        else
        {
            CurrentLock--;
            foreach (var grid in ShopBluePrints)
            {
                grid.ShowLockBtn(true);
            }
        }
    }

    public void ShopBtnClick()//�����̵����򿪶���
    {
        GameEvents.Instance.TutorialTrigger(TutorialType.ShopBtnClick);
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
        GameManager.Instance.RefreshShop(StaticData.Instance.ShopRefreshCost);
    }



    public void CompositeBluePrint(BluePrintGrid grid)//�ϳɶ�Ӧ���䷽
    {
        grid.BluePrint.BuildBluePrint();
        ConstructHelper.GetCompositeTurretByBluePrint(grid.BluePrint);
        RemoveGrid(grid);
        CheckAllBluePrint();
        GameManager.Instance.HideTips();
    }

    public void RemoveGrid(BluePrintGrid grid)//�Ƴ���Ӧ���䷽���������б�
    {
        if (ShopBluePrints.Contains(grid))
        {
            ShopBluePrints.Remove(grid);
        }
        if (OwnBluePrints.Contains(grid))
        {
            OwnBluePrints.Remove(grid);
        }
        ObjectPool.Instance.UnSpawn(grid);
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


    public void PreviewComposition(bool value, ElementType element, int quality)
    {
        foreach (var blueprint in ShopBluePrints)
        {
            blueprint.PreviewElement(value, element, quality);
        }
        foreach (var blueprint in OwnBluePrints)
        {
            blueprint.PreviewElement(value, element, quality);
        }

    }
}
