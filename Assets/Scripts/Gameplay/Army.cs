using UnityEngine;

public class Army
{
    public ArmyID id;
    public int lastActionRound;

    public Army(ArmyID _id, int _lastActionRound)
    {
        id = _id;
        lastActionRound = _lastActionRound;
    }

    public void Move(ref Province _province, Vector2Int _tilePos, Vector2Int _tilePosOld, Vector2Int _mousePos, Vector2Int _mousePosOld)
    {
        if ((int)_province.army.id != (int)GameplayManager.Instance.player.country.id)  // If there is no army or an army other than player's army in the province
        {
            Vector2Int diff = _tilePos - _tilePosOld;
            ref Province oldProvince = ref Game.Instance.map.provinces[_tilePosOld.x + _tilePosOld.y * Game.Instance.map.width];

            // The army has already done an action in this round or it's not player's turn, return
            if (oldProvince.army.lastActionRound == GameplayManager.Instance.round || !GameplayManager.Instance.player.GetTurn())
                return;

            if (diff.x == 1 && diff.y == 0 && UIManager.Instance.armyMovementIndicatorRight.activeSelf
                || diff.x == -1 && diff.y == 0 && UIManager.Instance.armyMovementIndicatorLeft.activeSelf
                || diff.x == 0 && diff.y == 1 && UIManager.Instance.armyMovementIndicatorUp.activeSelf
                || diff.x == 0 && diff.y == -1 && UIManager.Instance.armyMovementIndicatorDown.activeSelf)
            {
                if ((int)oldProvince.army.id == (int)GameplayManager.Instance.player.country.id)
                {
                    if (_province.army.id == ArmyID.None)    // If new province is empty, move army to new province
                    {
                        // Set the last action round to current round
                        oldProvince.army.lastActionRound = GameplayManager.Instance.round;

                        // Move army to new province
                        Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseArmy[(int)oldProvince.army.id]);
                        _province.army = new Army(oldProvince.army.id, oldProvince.army.lastActionRound);

                        // Remove army from old province
                        Game.Instance.map.tilemapArmy.SetTile((Vector3Int)_mousePosOld, null);
                        oldProvince.army = new Army(ArmyID.None, -1);
                    }
                    else if ((int)_province.army.id != (int)GameplayManager.Instance.player.country.id)   // If new province has army other than player's, fight
                    {
                        // Subtract 1 to exclude attacker army
                        int roll = Dice.Roll() - 1;

                        // Add bonuses and penalties to the roll
                        int armyBonus = GetArmyBonus(_province, oldProvince, _tilePos);
                        int landmarkBonus = oldProvince.landmark.GetBonus(true);
                        int landmarkPenalty = _province.landmark.GetPenalty(true);

                        roll += armyBonus + landmarkBonus + landmarkPenalty;

                        // If roll is higher than 5, clear the target province's army, and move ally army to target province
                        if (roll > 5)
                        {
                            // Subtract 1 from last action round to allow army to move to the province of the killed army
                            oldProvince.army.lastActionRound += -1;

                            _province.army.id = ArmyID.None;
                            Move(ref _province, _tilePos, _tilePosOld, _mousePos, _mousePosOld);
                        }

                        // Set the last action round to current round
                        oldProvince.army.lastActionRound = GameplayManager.Instance.round;

                        Debug.Log("Army bonus: " + armyBonus);
                        Debug.Log("Lanmark bonus: " + landmarkBonus);
                        Debug.Log("Landmark penalth: " + landmarkPenalty);
                        Debug.Log("Attack roll: " + roll);
                    }
                }
            }
        }
    }

    public void Occupy(ref Province _province, Vector2Int _tilePos, Vector2Int _mousePos)
    {
        // The army has already done an action in this round or it's not player's turn, return
        if (lastActionRound == GameplayManager.Instance.round || !GameplayManager.Instance.player.GetTurn())
            return;

        int roll = Dice.Roll();

        // Add bonuses and penalties to the roll
        int armyBonus = GetArmyBonus(_province, _province, _tilePos);
        int landmarkPenalty = _province.landmark.GetPenalty(true);

        roll += armyBonus + landmarkPenalty;

        if (roll > 5)
        {
            if (_province.occupiedBycountryID != (CountryID)_province.army.id)
            {
                _province.occupiedBycountryID = (CountryID)_province.army.id;

                Game.Instance.map.tilemapProvince.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseProvince[(int)_province.army.id + (int)ArmyID.Count]);
            }
            else
            {
                _province.countryID = (CountryID)_province.army.id;
                _province.occupiedBycountryID = CountryID.None;
                Game.Instance.countries[(int)_province.countryID].AddLandmark(_province.landmark.id, true);

                Game.Instance.map.tilemapProvince.SetTile((Vector3Int)_mousePos, Game.Instance.map.tilebaseProvince[(int)_province.army.id]);
            }

            // Set the last action round to current round
            lastActionRound = GameplayManager.Instance.round;
        }

        Debug.Log("Army bonus: " + armyBonus);
        Debug.Log("Landmark penalty: " + landmarkPenalty);
        Debug.Log("Occupy roll: " + roll);
    }

    private int GetArmyBonus(Province _province, Province _oldProvince, Vector2Int _tilePos)
    {
        ArmyID enemyId = _province.army.id;
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