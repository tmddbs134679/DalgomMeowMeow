using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public static class EnumExtensions
{
    public static int CountBySuffix<T>(string suffix) where T : Enum
    {
        return Enum.GetNames(typeof(T)).Count(name => name.EndsWith(suffix));
    }
}
