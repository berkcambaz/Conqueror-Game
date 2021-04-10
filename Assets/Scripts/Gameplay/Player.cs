using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Country country;

    public Player(Country _country)
    {
        country = _country;

        for (int i = 0; i < UIManager.Instance.imageArmy.Length; ++i)
            UIManager.Instance.imageArmy[i].sprite = SpriteManager.Instance.armies[(int)country.id];
    }
}
