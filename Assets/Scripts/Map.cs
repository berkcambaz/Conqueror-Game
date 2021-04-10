using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Tilemap tilemapProvince;
    public TileBase[] tilebaseProvince;

    public Tilemap tilemapLandmarks;
    public TileBase[] tilebaseLandmarks;

    public Tilemap tilemapArmy;
    public TileBase[] tilebaseArmy;

    public Province[] provinces;
    private BoundsInt bounds;
    public int width;
    public int height;

    void Start()
    {
        // Reset all the countries
        Game.Instance.countries = new Country[(int)CountryID.Count];

        bounds = tilemapProvince.cellBounds;
        width = bounds.size.x;
        height = bounds.size.y;
        provinces = new Province[width * height];

        TileBase[] tilebaseLandAll = tilemapProvince.GetTilesBlock(bounds);
        TileBase[] tilebaseLandmarksAll = tilemapLandmarks.GetTilesBlock(bounds);
        TileBase[] tilebaseArmyAll = tilemapArmy.GetTilesBlock(bounds);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CountryID countryID = CountryID.None;
                LandmarkID landmarkID = LandmarkID.None;

                for (int i = 0; i < tilebaseProvince.Length; i++)
                {
                    if (tilebaseProvince[i] == tilebaseLandAll[x + y * width])
                    {
                        countryID = (CountryID)(i);

                        // If country is not initialized, initialize
                        if (Game.Instance.countries[(int)countryID] == null)
                            Game.Instance.countries[(int)countryID] = new Country(countryID);

                        break;
                    }
                }

                for (int i = 0; i < tilebaseLandmarks.Length; i++)
                {
                    if (tilebaseLandmarks[i] == tilebaseLandmarksAll[x + y * width])
                    {
                        landmarkID = (LandmarkID)(i % (int)LandmarkID.Count);
                        Game.Instance.countries[(int)countryID].AddLandmark(landmarkID);
                        break;
                    }
                }


                provinces[x + y * width] = new Province(countryID, landmarkID);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ArmyID armyID = ArmyID.None;

                for (int i = 0; i < tilebaseArmy.Length; i++)
                {
                    if (tilebaseArmy[i] == tilebaseArmyAll[x + y * width])
                    {
                        armyID = (ArmyID)(i % (int)ArmyID.Count);
                        Game.Instance.countries[(int)armyID].AddArmy();
                        provinces[x + y * width].armyID = armyID;
                        break;
                    }
                }
            }
        }

        // Initialize player as green country
        GameplayManager.Instance.player = new Player(Game.Instance.countries[(int)CountryID.Red]);
    }

    public void SelectTile(int _x, int _y)
    {
        Vector2Int mousePos = new Vector2Int(_x, _y);
        Vector2Int tilePos = new Vector2Int(_x + Mathf.Abs(bounds.min.x), _y + Mathf.Abs(bounds.min.y));

        if (!bounds.Contains(new Vector3Int(mousePos.x, mousePos.y, 0)))
        {
            UIManager.Instance.DisablePanelBottom();
            return;
        }

        try
        {
            if (provinces[tilePos.x + tilePos.y * width].countryID != CountryID.None)
            {
                UIManager.Instance.EnablePanelBottom(provinces[tilePos.x + tilePos.y * width], mousePos, tilePos);
                UIManager.Instance.tileShadow.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
            }
        }
        catch (Exception)
        {
            UIManager.Instance.DisablePanelBottom();
        }
    }
}
