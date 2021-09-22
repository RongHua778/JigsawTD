using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMessage : IUserInterface
{
    [SerializeField] Text messageTxt = default;
    public void SetText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(MessageCor(text));
    }

    IEnumerator MessageCor(string content)
    {
        Show();
        messageTxt.text = content;
        yield return new WaitForSeconds(3f);
        Hide();
    }
}
