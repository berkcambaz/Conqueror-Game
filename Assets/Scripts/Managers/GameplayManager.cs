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

    public void MoveArmy(ref Province _province, Vector2Int _tilePos, Vector2Int _tilePosOld, Vector2Int _mousePos, Vector2Int _mousePosOld)
    {
        if ((int)_province.armyID != (int)player.country.id)
        {
            Vector2Int diff = _tilePos - _tilePosOld;
            ref Province oldProvince = ref Game.Instance.map.provinces[_tilePosOld.x + _tilePosOld.y * Game.Instance.map.width];
            if (diff.x == 1 && UIManager.Instance.armyMovementIndicatorRight.activeSelf
                || diff.x == -1 && UIManager.Instance.armyMovementIndicatorLeft.activeSelf
                || diff.y == 1 && UIManager.Instance.armyMovementIndicatorUp.activeSelf
                || diff.y == -1 && UIManager.Instance.armyMovementIndicatorDown.activeSelf)
            {
                if ((int)oldProvince.armyID == (int)player.country.id)
                {
                    if (_province.armyID == ArmyID.None)    // If new province is empty, move army to new province
                    {
                        // Move army to new province
                        Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseArmy[(int)oldProvince.armyID]);
                        _province.armyID = oldProvince.armyID;

                        // Remove army from old province
                        Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePosOld, null);
                        oldProvince.armyID = ArmyID.None;
                    }
                    else if ((int)_province.armyID != (int)player.country.id)   // If new province has army other than player's, fight
                    {

                    }
                }
            }
        }
    }

    public string GetProvinceText(Province _province)
    {
        string text = "";

        switch (_province.countryID)
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

        switch (_province.occupiedBycountryID)
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

                if (player.country.id == _province.countryID)
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
                if (player.country.id == _province.countryID)
                {
                    UIManager.Instance.actionDemolish.SetActive(true);
                }
                break;
            case LandmarkID.Forest:
                text = "- Forest";
                break;
            case LandmarkID.House:
                text = "- House";
                if (player.country.id == _province.countryID)
                {
                    UIManager.Instance.actionDemolish.SetActive(true);
                }
                break;
            case LandmarkID.Mountains:
                text = "- Mountains";
                break;
            case LandmarkID.Tower:
                text = "- Tower";
                if (player.country.id == _province.countryID)
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

        switch (_province.armyID)
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
            bool show = upProvince.countryID != CountryID.None && (int)upProvince.armyID != (int)player.country.id;
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
            bool show = downProvince.countryID != CountryID.None && (int)downProvince.armyID != (int)player.country.id;
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
            bool show = leftProvince.countryID != CountryID.None && (int)leftProvince.armyID != (int)GameplayManager.Instance.player.country.id;
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
            bool show = rightProvince.countryID != CountryID.None && (int)rightProvince.armyID != (int)GameplayManager.Instance.player.country.id;
            return show;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
