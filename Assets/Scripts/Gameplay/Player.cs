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
                UIManager.Instance.imageMoveArmy.sprite = SpriteManager.Instance.armyGreen;
                UIManager.Instance.imageRecruitArmy.sprite = SpriteManager.Instance.armyGreen;
                break;
            case CountryID.Purple:
                UIManager.Instance.imageMoveArmy.sprite = SpriteManager.Instance.armyPurple;
                UIManager.Instance.imageRecruitArmy.sprite = SpriteManager.Instance.armyPurple;
                break;
            case CountryID.Red:
                UIManager.Instance.imageMoveArmy.sprite = SpriteManager.Instance.armyRed;
                UIManager.Instance.imageRecruitArmy.sprite = SpriteManager.Instance.armyRed;
                break;
            case CountryID.Yellow:
                UIManager.Instance.imageMoveArmy.sprite = SpriteManager.Instance.armyYellow;
                UIManager.Instance.imageRecruitArmy.sprite = SpriteManager.Instance.armyYellow;
                break;
        }
    }
}
