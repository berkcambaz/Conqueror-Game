using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    public Player player;

    public void Init()
    {
        Instance = this;
    }

    public string GetProvinceText(Province _province)
    {
        string text = "";

        switch (_province.country.id)
        {
            case CountryID.Green:
                text += "Green's Province";
                break;
            case CountryID.Purple:
                text += "Purple's Province";
                break;
            case CountryID.Red:
                text += "Red's Province";
                break;
            case CountryID.Yellow:
                text += "Yellow's Province";
                break;
        }

        switch (_province.occupiedBycountry.id)
        {
            case CountryID.Green:
                text += " (Occupied by Green)";
                break;
            case CountryID.Purple:
                text += " (Occupied by Purple)";
                break;
            case CountryID.Red:
                text += " (Occupied by Red)";
                break;
            case CountryID.Yellow:
                text += " (Occupied by Yellow)";
                break;
        }

        return text;
    }

    public string GetLandmarkText(Province _province)
    {
        string text = "";

        switch (_province.landmarkID)
        {
            case LandmarkID.None:
                text = "- No Landmark";

                if (player.country.id == _province.country.id)
                {
                    UIManager.Instance.actionBuildChurch.SetActive(true);
                    UIManager.Instance.actionBuildHouse.SetActive(true);
                    UIManager.Instance.actionBuildTower.SetActive(true);
                }
                break;
            case LandmarkID.Capital:
                text = "- Capital";
                break;
            case LandmarkID.Church:
                text = "- Church";
                if (player.country.id == _province.country.id)
                {
                    UIManager.Instance.actionDemolish.SetActive(true);
                }
                break;
            case LandmarkID.Forest:
                text = "- Forest";
                break;
            case LandmarkID.House:
                text = "- House";
                if (player.country.id == _province.country.id)
                {
                    UIManager.Instance.actionDemolish.SetActive(true);
                }
                break;
            case LandmarkID.Mountains:
                text = "- Mountains";
                break;
            case LandmarkID.Tower:
                text = "- Tower";
                if (player.country.id == _province.country.id)
                {
                    UIManager.Instance.actionDemolish.SetActive(true);
                }
                break;
        }

        return text;
    }

    public string GetArmyText(Province _province)
    {
        string text = "";

        switch (_province.army.id)
        {
            case ArmyID.None:
                text = "- No Army";
                break;
            case ArmyID.Green:
                text = "- Green's Army";
                break;
            case ArmyID.Purple:
                text = "- Purple's Army";
                break;
            case ArmyID.Red:
                text = "- Red's Army";
                break;
            case ArmyID.Yellow:
                text = "- Yellow's Army";
                break;
        }

        return text;
    }

    public bool ShowArmyMovementIndicatorUp(Vector2Int _tilePos)
    {
        // Try to check if there is a land in the up direction, if it error's, there is not
        try
        {
            Province upProvince = Game.Instance.map.provinces[_tilePos.x + _tilePos.y * Game.Instance.map.width + Game.Instance.map.width];
            bool show = upProvince.country.id != CountryID.None && (int)upProvince.army.id != (int)player.country.id;
            return show;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ShowArmyMovementIndicatorDown(Vector2Int _tilePos)
    {
        // Try to check if there is a land in the down direction, if it error's, there is not
        try
        {
            Province downProvince = Game.Instance.map.provinces[_tilePos.x + _tilePos.y * Game.Instance.map.width - Game.Instance.map.width];
            bool show = downProvince.country.id != CountryID.None && (int)downProvince.army.id != (int)player.country.id;
            return show;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ShowArmyMovementIndicatorLeft(Vector2Int _tilePos)
    {
        // Try to check if there is a land in the left direction, if it error's, there is not
        try
        {
            Province leftProvince = Game.Instance.map.provinces[_tilePos.x + _tilePos.y * Game.Instance.map.width - 1];
            bool show = leftProvince.country.id != CountryID.None && (int)leftProvince.army.id != (int)player.country.id;
            return show;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ShowArmyMovementIndicatorRight(Vector2Int _tilePos)
    {
        // Try to check if there is a land in the left direction, if it error's, there is not
        try
        {
            Province rightProvince = Game.Instance.map.provinces[_tilePos.x + _tilePos.y * Game.Instance.map.width + 1];
            bool show = rightProvince.country.id != CountryID.None && (int)rightProvince.army.id != (int)player.country.id;
            return show;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
