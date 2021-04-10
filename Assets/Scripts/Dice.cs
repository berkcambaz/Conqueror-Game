using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice 
{
    public static int Roll()
    {
        return Random.Range(1, 7);
    }
}
