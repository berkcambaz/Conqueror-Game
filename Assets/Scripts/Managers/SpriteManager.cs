using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    public Sprite armyGreen;
    public Sprite armyPurple;
    public Sprite armyRed;
    public Sprite armyYellow;

    public void Init()
    {
        Instance = this;
    }
}
