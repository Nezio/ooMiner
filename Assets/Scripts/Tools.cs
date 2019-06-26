using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static void ShiftArrayBackbyN<T>(int n, ref T[] array)
    {
        for (int i = n; i < array.Length; i++)
        {
            array[i - n] = array[i];
        }
    }

}
