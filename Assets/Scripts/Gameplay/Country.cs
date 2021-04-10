using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country
{
    public CountryID id;

    public int manpower;
    public int armyCount;
    public int gold;
    public int goldEachRound;

    public Country(CountryID _id)
    {
        id = _id;
        manpower = 0;
        armyCount = 0;
        gold = 0;
        goldEachRound = 0;
    }


    public void AddLandmark(LandmarkID _landmarkID)
    {
        switch (_landmarkID)
        {
            case LandmarkID.None:
                break;
            case LandmarkID.Capital:
                break;
            case LandmarkID.Church:
                ++gold;
                ++goldEachRound;
                break;
            case LandmarkID.Forest:
                break;
            case LandmarkID.House:
                ++manpower;
                break;
            case LandmarkID.Mountains:
                break;
            case LandmarkID.Tower:
                break;
            case LandmarkID.Count:
                break;
            default:
                break;
        }
    }

    public void BuyLandmark(LandmarkID _id, int _x, int _y)
    {

    }

    public void RemoveLandmark(int _x, int _y)
    {

    }

    public void AddArmy()
    {
        ++armyCount;
    }

    public void BuyArmy(int _x, int _y)
    {
        if (manpower >= armyCount + 1 && gold >= 2)
        {
            // Buy 1 army for 2 golds
            gold -= 2;
            ++armyCount;

            // Place army to x, y position
        }
    }

    public void RemoveArmy(int _x, int _y)
    {

    }
}

public enum CountryID
{
    None = -1,
    Green,
    Purple,
    Red,
    Yellow,
    Count
}