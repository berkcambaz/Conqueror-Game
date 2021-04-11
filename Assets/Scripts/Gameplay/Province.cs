using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public CountryID countryID;
    public Country occupiedBycountry;
    public Landmark landmark;
    public Army army;

    public Province(CountryID _countryID, LandmarkID _landmarkID)
    {
        countryID = _countryID;
        occupiedBycountry = new Country(CountryID.None);
        landmark = new Landmark(_landmarkID);
        army = new Army(ArmyID.None, -1);
    }
}
