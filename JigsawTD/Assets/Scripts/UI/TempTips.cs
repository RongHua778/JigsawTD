using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempTips : MonoBehaviour
{
    public Canvas myCanvas;
    public RectTransform rect;
    public Image boxFrame;
    public Text HorizontalText;
    public Text VerticalText;

    public int minFrameWidth = 100;
    public int maxFrameWidth = 600;

    public int rowHeight = 80;

    public Color textColor = new Color(0, 0, 0, 1);
    public Color hideColor = new Color(0, 0, 0, 0);

    private float offsetDistance;
    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
        offsetDistance = boxFrame.rectTransform.sizeDelta.y - HorizontalText.rectTransform.sizeDelta.y / 2;
        VerticalText.color = hideColor;
        HorizontalText.color = textColor;
        //StartCoroutine(AdjustSize());
    }

    private void AdjustDialogBoxSize()
    {
        if (VerticalText.rectTransform.sizeDelta.y > rowHeight)
        {
            VerticalText.color = textColor;
            HorizontalText.color = hideColor;
            boxFrame.rectTransform.sizeDelta = new Vector2(minFrameWidth + VerticalText.rectTransform.sizeDelta.x / 2, VerticalText.rectTransform.sizeDelta.y / 2 + offsetDistance);
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(minFrameWidth + VerticalText.rectTransform.sizeDelta.x / 2, VerticalText.rectTransform.sizeDelta.y / 2 + offsetDistance);
        }
        else
        {
            boxFrame.rectTransform.sizeDelta = new Vector2(minFrameWidth + HorizontalText.rectTransform.sizeDelta.x / 2, HorizontalText.rectTransform.sizeDelta.y / 2 + offsetDistance);
        }

    }

    public void SendText(string input,Vector2 pos)
    {
        VerticalText.text = input;
        HorizontalText.text = input;
        StartCoroutine(AdjustSize(pos));
    }

    public void SetPos(Vector2 pos)
    {
        //Vector2 NewPos = pos + new Vector2(0, rect.sizeDelta.y / 2 + 30);

        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, pos, myCanvas.worldCamera, out newPos);
        transform.position = myCanvas.transform.TransformPoint(newPos);
    }

    IEnumerator AdjustSize(Vector2 pos)
    {
        yield return new WaitForEndOfFrame();
        AdjustDialogBoxSize();
        SetPos(pos);
    }

    void Update()
    {
       
        // 世界空间
        //Vector2 minworld = transform.TransformPoint(rect.rect.min);
        //Vector2 maxworld = transform.TransformPoint(rect.rect.max);
        //Vector2 sizeworld = maxworld - minworld;

        //// 保持最小的位置在屏幕边界-大小
        //maxworld = new Vector2(Screen.width, Screen.height) - sizeworld;

        //// 保持位置在(0,0)和maxworld之间

        //float y = Mathf.Clamp(minworld.y, 0, maxworld.y);

        //float x = Mathf.Clamp(minworld.x, 0, maxworld.x);

        //// set new position to xy(=local) + offset(=world)设置新位置为xy(=本地)+偏移量(=世界)
        //Vector2 offset = (Vector2)transform.position - minworld;
        //transform.position = new Vector2(x, y) + offset;
    }




}
