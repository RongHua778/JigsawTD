using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class TurretEffectFactory
{
    public static Dictionary<int, Type> AttackEffectDIC;

    private static bool isInitialize => AttackEffectDIC != null;

    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(TurretSkill)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TurretSkill)));
        AttackEffectDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as TurretSkill;
            AttackEffectDIC.Add((int)effect.EffectName, type);
        }
    }

    public static TurretSkill GetEffect(int id)
    {
        Initialize();
        if (AttackEffectDIC.ContainsKey(id))
        {
            Type type = AttackEffectDIC[id];
            TurretSkill effect = Activator.CreateInstance(type) as TurretSkill;
            return effect;
        }
        return null;
    }

}
