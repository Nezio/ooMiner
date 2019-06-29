using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static void ShiftArrayBackByN<T>(int n, ref T[] array)
    {
        for (int i = n; i < array.Length; i++)
        {
            array[i - n] = array[i];
        }
    }

    public static void Backfill(ref GameObject[] array)
    { // go trough an array and group null values at the front
        // go trough the array backwards
        // if null value is found find front-most non-null value and switch them
        for(int i = array.Length-1; i > 0; i--)
        {
            if(array[i] == null)
            {
                for(int j = 0; j < array.Length-1; j++)
                {
                    if(array[j] != null)
                    { // front-most non-null value found
                        array[i] = array[j];
                        array[j] = null;
                        break;
                    }
                    // if no non-null value is found from the start of the array to current possition: array is already backfilled or empty
                    if (j == i-1)
                        return;
                }
            }
        }
    }

    public static void Backfill(ref List<GameObject> list)
    { // go trough a list and group null values at the front
        // go trough the list backwards
        // if null value is found find front-most non-null value and switch them
        for (int i = list.Count - 1; i > 0; i--)
        {
            if (list[i] == null)
            {
                for (int j = 0; j < list.Count - 1; j++)
                {
                    if (list[j] != null)
                    { // front-most non-null value found
                        list[i] = list[j];
                        list[j] = null;
                        break;
                    }
                    // if no non-null value is found from the start of the array to current possition: list is already backfilled or empty
                    if (j == i - 1)
                        return;
                }
            }
        }
    }

    public static void DestroyFirstN(int n, ref GameObject[] array)
    {
        for (int i = 0; i < n; i++)
        {
            if (array[i] != null)
            {
                GameObject.Destroy(array[i]);
                //Debug.Log("destroyed");
            }
        }
    }


}
