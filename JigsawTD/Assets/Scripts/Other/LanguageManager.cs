using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lanuguage;
using Sirenix.OdinInspector;


public class LanguageManager : ScriptableObject
{
    [Searchable]
    public List<LanguageData> dataArray;

}
