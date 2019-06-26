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

    public static void BackfillArray(ref GameObject[] array)
    { // go trough an array and group null values at the front
        // go trough the array backwards
        // if null value is found find front-most non-null value and switch them

        // if array is empty -> don't do anything


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

}
