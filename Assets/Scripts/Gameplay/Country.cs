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


    public void AddLandmark(LandmarkID _landmarkID, bool _mapInitialized)
    {
        switch (_landmarkID)
        {
            case LandmarkID.None:
                break;
            case LandmarkID.Capital:
                break;
            case LandmarkID.Church:
                ++goldEachRound;
                if (!_mapInitialized)
                    ++gold;
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

    public bool BuyLandmark(LandmarkID _id, ref Province _province, Vector2Int _mousePos)
    {
        bool landmarkPurchased = true;

        switch (_id)
        {
            case LandmarkID.Church:
                if (gold >= 4)
                {
                    gold -= 4;
                    goldEachRound += 1;

                    // Place church to x, y position
                    _province.landmark.id = _id;
                    Game.Instance.map.tilemapLandmarks.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseLandmarks[(int)_id]);
                }
                break;
            case LandmarkID.House:
                if (gold >= 2)
                {
                    gold -= 2;
                    manpower += 1;

                    // Place house to x, y position
                    _province.landmark.id = _id;
                    Game.Instance.map.tilemapLandmarks.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseLandmarks[(int)_id]);
                }
                break;
            case LandmarkID.Tower:
                if (gold >= 5)
                {
                    gold -= 5;

                    // Place tower to x, y position
                    _province.landmark.id = _id;
                    Game.Instance.map.tilemapLandmarks.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseLandmarks[(int)_id]);
                }
                break;
            default:
                landmarkPurchased = false;
                break;
        }

        return landmarkPurchased;
    }

    public void RemoveLandmark(ref Province _province, Vector2Int _mousePos)
    {
        switch (_province.landmark.id)
        {
            case LandmarkID.Church:
                goldEachRound -= 1;
                break;
            case LandmarkID.House:
                manpower -= 1;
                break;
            case LandmarkID.Tower:
                break;
        }

        _province.landmark.id = LandmarkID.None;
        Game.Instance.map.tilemapLandmarks.SetTile((Vector3Int)_mousePos, null);
    }

    public void AddArmy()
    {
        ++armyCount;
    }

    public bool BuyArmy(ref Province _province, Vector2Int _mousePos)
    {
        bool armyPurchased = false;

        if (manpower >= armyCount + 1 && gold >= 2)
        {
            // Buy 1 army for 2 golds
            gold -= 2;
            ++armyCount;

            // Place army to x, y position
            _province.army.id = (ArmyID)_province.countryID;
            Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseArmy[(int)_province.army.id]);

            armyPurchased = true;
        }

        return armyPurchased;
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