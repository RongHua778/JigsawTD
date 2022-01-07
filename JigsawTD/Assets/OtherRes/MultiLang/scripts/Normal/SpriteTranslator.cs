using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteTranslator : MonoBehaviour
{
    [SerializeField]
    string key;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = GameMultiLang.GetSprite(key);
    }

}
