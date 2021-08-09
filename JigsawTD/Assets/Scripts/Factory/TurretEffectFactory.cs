using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class TurretEffectFactory
{
    public static Dictionary<int, Type> TurretSkillDIC;
    public static Dictionary<List<int>, Type> ElementSkillDIC;
    public static Dictionary<int, Type> TileSkillDIC;

    private static bool isInitialize => TurretSkillDIC != null;

    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(InitialSkill)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(InitialSkill)));
        TurretSkillDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as InitialSkill;
            TurretSkillDIC.Add((int)effect.EffectName, type);
        }

        InitialzieElementDIC();
        InitializeTileSkillDIC();
    }

    private static void InitializeTileSkillDIC()
    {
        var types = Assembly.GetAssembly(typeof(TileSkill)).GetTypes().
         Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TileSkill)));
        TileSkillDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as TileSkill;
            TileSkillDIC.Add((int)effect.TileSkillName, type);
        }
    }

    private static void InitialzieElementDIC()
    {
        var types = Assembly.GetAssembly(typeof(ElementSkill)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ElementSkill)));
        ElementSkillDIC = new Dictionary<List<int>, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as ElementSkill;
            if (!ElementSkillDIC.ContainsKey(effect.Elements))
            {
                ElementSkillDIC.Add(effect.Elements, type);
            }
            else
            {
                Debug.LogWarning("重复的元素搭配：" + effect.Elements[0].ToString() + effect.Elements[1].ToString() + effect.Elements[2].ToString());
            }
        }
    }

    public static InitialSkill GetInitialSkill(int id)
    {
        if (TurretSkillDIC.ContainsKey(id))
        {
            Type type = TurretSkillDIC[id];
            InitialSkill effect = Activator.CreateInstance(type) as InitialSkill;
            return effect;
        }
        return null;
    }

    public static TileSkill GetTileSkill(int id)
    {
        if (TileSkillDIC.ContainsKey(id))
        {
            Type type = TileSkillDIC[id];
            TileSkill effect = Activator.CreateInstance(type) as TileSkill;
            return effect;
        }
        return null;
    }

    public static ElementSkill GetElementSkill(List<int> elements)
    {
        Type type;
        ElementSkill effect;
        foreach (var skill in ElementSkillDIC.Keys)
        {
            List<int> temp = skill.ToList();
            foreach (var element in elements)
            {
                if (temp.Contains(element))
                {
                    temp.Remove(element);
                    if (temp.Count == 0)
                    {
                        type = ElementSkillDIC[skill];
                        effect = Activator.CreateInstance(type) as ElementSkill;
                        return effect;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        Debug.LogWarning("没有这个元素技能" + elements[0].ToString() + elements[1].ToString() + elements[2].ToString());
        return null;
    }

}
