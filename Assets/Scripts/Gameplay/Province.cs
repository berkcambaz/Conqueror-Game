using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public Country country;
    public Country occupiedBycountry;
    public Landmark landmark;
    public Army army;

    public Province(CountryID _countryID, LandmarkID _landmarkID)
    {
        country = new Country((CountryID)((int)_countryID % (int)CountryID.Count));
        occupiedBycountry = new Country((int)_countryID > (int)CountryID.Count - 1 ? (CountryID)((int)_countryID % (int)CountryID.Count) : CountryID.None);
        landmark = new Landmark(_landmarkID);
        army = new Army(ArmyID.None);
    }
}
