using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Country country;

    public Player(Country _country)
    {
        country = _country;

        switch (country.id)
        {
            case CountryID.Green:
                for (int i = 0; i < UIManager.Instance.imageArmy.Length; ++i)
                    UIManager.Instance.imageArmy[i].sprite = SpriteManager.Instance.armyGreen;
                break;
            case CountryID.Purple:
                for (int i = 0; i < UIManager.Instance.imageArmy.Length; ++i)
                    UIManager.Instance.imageArmy[i].sprite = SpriteManager.Instance.armyPurple;
                break;
            case CountryID.Red:
                for (int i = 0; i < UIManager.Instance.imageArmy.Length; ++i)
                    UIManager.Instance.imageArmy[i].sprite = SpriteManager.Instance.armyRed;
                break;
            case CountryID.Yellow:
                for (int i = 0; i < UIManager.Instance.imageArmy.Length; ++i)
                    UIManager.Instance.imageArmy[i].sprite = SpriteManager.Instance.armyYellow;
                break;
        }
    }
}
