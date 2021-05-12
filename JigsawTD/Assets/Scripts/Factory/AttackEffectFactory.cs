using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class AttackEffectFactory
{
    public static Dictionary<int, Type> AttackEffectDIC;

    private static bool isInitialize => AttackEffectDIC != null;

    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(AttackEffect)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AttackEffect)));
        AttackEffectDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as AttackEffect;
            AttackEffectDIC.Add((int)effect.EffectName, type);
        }
    }

    public static AttackEffect GetEffect(int id)
    {
        Initialize();
        if (AttackEffectDIC.ContainsKey(id))
        {
            Type type = AttackEffectDIC[id];
            AttackEffect effect = Activator.CreateInstance(type) as AttackEffect;
            return effect;
        }
        return null;
    }

}
