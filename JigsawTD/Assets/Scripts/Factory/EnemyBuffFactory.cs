using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class EnemyBuffFactory
{
    public static Dictionary<int, Type> EnemyBuffDIC;
    private static bool isInitialize => EnemyBuffDIC != null;

    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(EnemyBuff)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EnemyBuff)));
        EnemyBuffDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var buff = Activator.CreateInstance(type) as EnemyBuff;
            EnemyBuffDIC.Add((int)buff.BuffName, type);
        }
    }

    public static EnemyBuff GetBuff(int id)
    {
        Initialize();
        if (EnemyBuffDIC.ContainsKey(id))
        {
            Type type = EnemyBuffDIC[id];
            EnemyBuff buff = Activator.CreateInstance(type) as EnemyBuff;
            return buff;
        }
        return null;
    }

}
