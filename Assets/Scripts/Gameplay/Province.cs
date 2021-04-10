using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public CountryID countryID;
    public CountryID occupiedBycountryID;
    public LandmarkID landmarkID;
    public ArmyID armyID = ArmyID.None;

    public Province(CountryID _countryID, LandmarkID _landmarkID)
    {
        countryID = (CountryID)((int)_countryID % (int)CountryID.Count);
        occupiedBycountryID = (int)_countryID > (int)CountryID.Count - 1 ? (CountryID)((int)_countryID % (int)CountryID.Count) : CountryID.None;
        landmarkID = _landmarkID;
    }
}
