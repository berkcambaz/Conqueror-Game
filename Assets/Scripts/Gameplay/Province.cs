using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public CountryID countryID;
    public CountryID occupiedBycountryID;
    public Landmark landmark;
    public Army army;

    public Province(CountryID _countryID, LandmarkID _landmarkID)
    {
        countryID = _countryID;
        occupiedBycountryID = CountryID.None;
        landmark = new Landmark(_landmarkID);
        army = new Army(ArmyID.None, -1);
    }
}
