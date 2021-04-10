using UnityEngine;

public class Army
{
    public ArmyID id;

    public Army(ArmyID _id)
    {
        id = _id;
    }

    public void Move(ref Province _province, Vector2Int _tilePos, Vector2Int _tilePosOld, Vector2Int _mousePos, Vector2Int _mousePosOld)
    {
        if ((int)_province.army.id != (int)GameplayManager.Instance.player.country.id)  // If there is no army or an army other than player's army in the province
        {
            Vector2Int diff = _tilePos - _tilePosOld;
            ref Province oldProvince = ref Game.Instance.map.provinces[_tilePosOld.x + _tilePosOld.y * Game.Instance.map.width];
            if (diff.x == 1 && diff.y == 0 && UIManager.Instance.armyMovementIndicatorRight.activeSelf
                || diff.x == -1 && diff.y == 0 && UIManager.Instance.armyMovementIndicatorLeft.activeSelf
                || diff.x == 0 && diff.y == 1 && UIManager.Instance.armyMovementIndicatorUp.activeSelf
                || diff.x == 0 && diff.y == -1 && UIManager.Instance.armyMovementIndicatorDown.activeSelf)
            {
                if ((int)oldProvince.army.id == (int)GameplayManager.Instance.player.country.id)
                {
                    if (_province.army.id == ArmyID.None)    // If new province is empty, move army to new province
                    {
                        // Move army to new province
                        Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseArmy[(int)oldProvince.army.id]);
                        _province.army.id = oldProvince.army.id;

                        // Remove army from old province
                        Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePosOld, null);
                        oldProvince.army.id = ArmyID.None;
                    }
                    else if ((int)_province.army.id != (int)GameplayManager.Instance.player.country.id)   // If new province has army other than player's, fight
                    {
                        // Subtract 1 to exclude attacker army
                        int roll = Dice.Roll() - 1;

                        // Add bonuses and penalties to the roll
                        roll += GetArmyBonus(_province, oldProvince, _tilePos);
                        roll += oldProvince.landmark.GetBonus(true);
                        roll += _province.landmark.GetPenalty(true);

                        // If roll is higher than 5, clear the target province's army, and move ally army to target province
                        if (roll > 5)
                        {
                            _province.army.id = ArmyID.None;
                            Move(ref _province, _tilePos, _tilePosOld, _mousePos, _mousePosOld);
                        }

                        Debug.Log("Attack roll: " + roll);
                    }
                }
            }
        }
    }

    public void Occupy(ref Province _province, Vector2Int _tilePos, Vector2Int _mousePos)
    {
        int roll = Dice.Roll();

        // Add bonuses and penalties to the roll
        roll += GetArmyBonus(_province, _province, _tilePos);
        roll += _province.landmark.GetBonus(true);
        roll += _province.landmark.GetPenalty(true);

        if (roll > 5)
        {
            if (_province.occupiedBycountry.id != (CountryID)_province.army.id)
            {
                _province.occupiedBycountry.id = (CountryID)_province.army.id;

                Game.Instance.map.tilemapProvince.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseProvince[(int)_province.army.id + (int)ArmyID.Count]);
            }
            else
            {
                ArmyID armyId = _province.army.id;
                _province.countryID = (CountryID)armyId;
                _province.occupiedBycountry.id = CountryID.None;

                Game.Instance.map.tilemapProvince.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseProvince[(int)_province.army.id]);
            }
        }

        Debug.Log("Occupy roll: " + roll);
    }

    private int GetArmyBonus(Province _province, Province _oldProvince, Vector2Int _tilePos)
    {
        ArmyID enemyId = (ArmyID)_province.countryID;
        ArmyID allyId = _oldProvince.army.id;
        ArmyID landArmyId = ArmyID.None;
        int bonus = 0;

        if (!(_tilePos.x + 1 > Game.Instance.map.width - 1))
        {
            landArmyId = Game.Instance.map.provinces[(_tilePos.x + 1) + _tilePos.y * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.x - 1 < 0))
        {
            landArmyId = Game.Instance.map.provinces[(_tilePos.x - 1) + _tilePos.y * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.y + 1 > Game.Instance.map.height - 1))
        {
            landArmyId = Game.Instance.map.provinces[_tilePos.x + (_tilePos.y + 1) * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.y - 1 < 0))
        {
            landArmyId = Game.Instance.map.provinces[_tilePos.x + (_tilePos.y - 1) * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.x + 1 > Game.Instance.map.width - 1) && !(_tilePos.y + 1 > Game.Instance.map.height - 1))
        {
            landArmyId = Game.Instance.map.provinces[(_tilePos.x + 1) + (_tilePos.y + 1) * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.x - 1 < 0) && !(_tilePos.y + 1 > Game.Instance.map.height - 1))
        {
            landArmyId = Game.Instance.map.provinces[(_tilePos.x - 1) + (_tilePos.y + 1) * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.x + 1 > Game.Instance.map.width - 1) && !(_tilePos.y - 1 < 0))
        {
            landArmyId = Game.Instance.map.provinces[(_tilePos.x + 1) + (_tilePos.y - 1) * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }
        if (!(_tilePos.x - 1 < 0) && !(_tilePos.y - 1 < 0))
        {
            landArmyId = Game.Instance.map.provinces[(_tilePos.x - 1) + (_tilePos.y - 1) * Game.Instance.map.width].army.id;
            if (landArmyId == enemyId)
                bonus += -1;
            else if (landArmyId == allyId)
                bonus += 1;
        }

        return bonus;
    }
}

public enum ArmyID
{
    None = -1,
    Green,
    Purple,
    Red,
    Yellow,
    Count
}