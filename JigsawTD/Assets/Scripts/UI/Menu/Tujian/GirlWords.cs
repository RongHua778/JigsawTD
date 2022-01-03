using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GirlWords : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI word_Txt = default;

    [SerializeField] string[] words = default;

    int currentID;
    private void Start()
    {
        OnGirlClick();
    }
    public void OnGirlClick()
    {
        int id = currentID;
        while (id == currentID)
        {
            id = Random.Range(0, words.Length);
        }
        currentID = id;
        StartCoroutine(TypeSentence(GameMultiLang.GetTraduction(words[currentID])));
    }

    private IEnumerator TypeSentence(string word)
    {

        word_Txt.text = word;
        word_Txt.maxVisibleCharacters = 0;
        word_Txt.ForceMeshUpdate();

        var textInfo = word_Txt.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 0);
        }

        // 按时间逐个显示字符
        var timer = 0f;
        var interval = 0.03f;
        while (word_Txt.maxVisibleCharacters < textInfo.characterCount)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                word_Txt.maxVisibleCharacters++;
            }

            yield return null;
        }
    }

    private void SetCharacterAlpha(int index, byte alpha)
    {
        var materialIndex = word_Txt.textInfo.characterInfo[index].materialReferenceIndex;
        var vertexColors = word_Txt.textInfo.meshInfo[materialIndex].colors32;
        var vertexIndex = word_Txt.textInfo.characterInfo[index].vertexIndex;

        vertexColors[vertexIndex + 0].a = alpha;
        vertexColors[vertexIndex + 1].a = alpha;
        vertexColors[vertexIndex + 2].a = alpha;
        vertexColors[vertexIndex + 3].a = alpha;

    }

}
