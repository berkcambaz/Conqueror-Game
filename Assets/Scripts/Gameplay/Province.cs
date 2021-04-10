using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public Country country;
    public Country occupiedBycountry;
    public Landmark landmark;
    public Army army;

    public Province(ref Country _country, LandmarkID _landmarkID)
    {
        country = _country;
        occupiedBycountry = new Country(CountryID.None);
        landmark = new Landmark(_landmarkID);
        army = new Army(ArmyID.None);
    }
}
