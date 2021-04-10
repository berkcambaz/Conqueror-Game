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
        if ((int)_province.army.id != (int)GameplayManager.Instance.player.country.id)
        {
            Vector2Int diff = _tilePos - _tilePosOld;
            ref Province oldProvince = ref Game.Instance.map.provinces[_tilePosOld.x + _tilePosOld.y * Game.Instance.map.width];
            if (diff.x == 1 && UIManager.Instance.armyMovementIndicatorRight.activeSelf
                || diff.x == -1 && UIManager.Instance.armyMovementIndicatorLeft.activeSelf
                || diff.y == 1 && UIManager.Instance.armyMovementIndicatorUp.activeSelf
                || diff.y == -1 && UIManager.Instance.armyMovementIndicatorDown.activeSelf)
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
                        ArmyID enemyId = _province.army.id;
                        ArmyID allyId = oldProvince.army.id;
                        ArmyID landArmyId = ArmyID.None;

                        // Subtract 1 to exclude attacker army
                        int roll = Dice.Roll() - 1;

                        // Add army bonuses
                        if (!(_tilePos.x + 1 > Game.Instance.map.width - 1))
                        {
                            landArmyId = Game.Instance.map.provinces[(_tilePos.x + 1) + _tilePos.y * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.x - 1 < 0))
                        {
                            landArmyId = Game.Instance.map.provinces[(_tilePos.x - 1) + _tilePos.y * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.y + 1 > Game.Instance.map.height - 1))
                        {
                            landArmyId = Game.Instance.map.provinces[_tilePos.x + (_tilePos.y + 1) * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.y - 1 < 0))
                        {
                            landArmyId = Game.Instance.map.provinces[_tilePos.x + (_tilePos.y - 1) * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.x + 1 > Game.Instance.map.width - 1) && !(_tilePos.y + 1 > Game.Instance.map.height - 1))
                        {
                            landArmyId = Game.Instance.map.provinces[(_tilePos.x + 1) + (_tilePos.y + 1) * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.x - 1 < 0) && !(_tilePos.y + 1 > Game.Instance.map.height - 1))
                        {
                            landArmyId = Game.Instance.map.provinces[(_tilePos.x - 1) + (_tilePos.y + 1) * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.x + 1 > Game.Instance.map.width - 1) && !(_tilePos.y - 1 < 0))
                        {
                            landArmyId = Game.Instance.map.provinces[(_tilePos.x + 1) + (_tilePos.y - 1) * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }
                        if (!(_tilePos.x - 1 < 0) && !(_tilePos.y - 1 < 0))
                        {
                            landArmyId = Game.Instance.map.provinces[(_tilePos.x - 1) + (_tilePos.y - 1) * Game.Instance.map.width].army.id;
                            if (landArmyId == enemyId)
                                roll += -1;
                            else if (landArmyId == allyId)
                                roll += 1;
                        }

                        // Add landmark bonuses for new province
                        switch (_province.landmarkID)
                        {
                            case LandmarkID.Mountains:
                                roll += -1;
                                break;
                            case LandmarkID.Tower:
                                roll += -1;
                                break;
                            default:
                                break;
                        }

                        // Add landmark bonuses for old province
                        switch (oldProvince.landmarkID)
                        {
                            case LandmarkID.Forest:
                                roll += 1;
                                break;
                            default:
                                break;
                        }

                        // If roll is higher than 5, clear the target province's army, and move ally army to target province
                        if (roll > 5)
                        {
                            _province.army.id = ArmyID.None;
                            Move(ref _province, _tilePos, _tilePosOld, _mousePos, _mousePosOld);
                        }

                    }
                }
            }
        }
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