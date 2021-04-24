using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject messagePanel;
    [SerializeField]
    TMP_Text messageTxt;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onMessage += ShowMessage;
    }

    private void ShowMessage(string content)
    {
        StartCoroutine(MessageCor(content));
    }

    IEnumerator MessageCor(string content)
    {
        messagePanel.SetActive(true);
        messageTxt.text = content;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);
        messageTxt.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
