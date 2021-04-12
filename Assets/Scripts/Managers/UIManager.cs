using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Net;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject armyMovementIndicator;
    public GameObject armyMovementIndicatorCenter;
    public GameObject armyMovementIndicatorUp;
    public GameObject armyMovementIndicatorDown;
    public GameObject armyMovementIndicatorLeft;
    public GameObject armyMovementIndicatorRight;
    public GameObject tileSelect;
    public GameObject tileShadow;

    public Canvas canvasMenu;
    public Canvas canvasHost;
    public InputField inputFieldHostPort;
    public Button buttonHost;
    public Canvas canvasJoin;
    public InputField inputFieldJoinIPAddress;
    public InputField inputFieldJoinPort;
    public Button buttonJoin;

    public Canvas canvasGameUI;

    public GameObject panelTop;
    public Text textGold;
    public Text textGoldEarned;
    public Text textArmyCount;
    public Text textManpower;
    public RectTransform rectTransformPanelTop;
    private Vector2 panelTopAnchoredPosition;
    private bool panelTopOpened = false;

    public GameObject endTurn;

    public GameObject panelBottom;
    public Text textProvince;
    public Text textLandmark;
    public Text textArmy;
    public GameObject actionMoveArmy;
    public GameObject actionRecruitArmy;
    public GameObject actionBuildChurch;
    public GameObject actionBuildHouse;
    public GameObject actionBuildTower;
    public GameObject actionDemolish;
    public Image[] imageArmy;

    private Province province;
    private Vector2Int mousePos;
    private Vector2Int mousePosOld;
    private Vector2Int tilePos;
    private Vector2Int tilePosOld;

    public void Init()
    {
        Instance = this;

        panelTopAnchoredPosition = rectTransformPanelTop.anchoredPosition;
    }

    public void EnablePanelBottom(Province _province, Vector2Int _mousePos, Vector2Int _tilePos)
    {
        mousePosOld = mousePos;
        tilePosOld = tilePos;

        mousePos = _mousePos;
        tilePos = _tilePos;

        // If panel was not activated, activate it
        if (!panelBottom.activeSelf)
        {
            province = _province;

            // Show the bottom panel
            panelBottom.SetActive(true);
            tileShadow.SetActive(true);

            // Display bottom panel with province
            DisplayPanelBottom();
        }
        else    // If panel was already activated
        {
            if (province == _province)  // If clicked to the same province
            {
                if (armyMovementIndicatorCenter.activeSelf) // If center indicator was active, occupy the province
                {
                    province.army.Occupy(ref province, tilePos, mousePos);

                    // Update top panel and display bottom panel with province
                    UpdateTopPanel();
                    DisplayPanelBottom();
                    DisableArmyMovementIndicator();
                }
                else    // Else, close the bottom panel
                {
                    DisablePanelBottom();
                }
            }
            else    // If clicked to different province while panel was empty, update the panel
            {
                province = _province;

                // Move army
                province.army.Move(ref province, tilePos, tilePosOld, mousePos, mousePosOld);

                // Display bottom panel with province
                DisplayPanelBottom();
                DisableArmyMovementIndicator();
            }
        }
    }

    public void DisablePanelBottom()
    {
        // Hide the bottom panel
        panelBottom.SetActive(false);
        tileShadow.SetActive(false);
        DisableArmyMovementIndicator();
    }

    private void DisplayPanelBottom()
    {
        actionMoveArmy.SetActive((int)GameplayManager.Instance.player.country.id == (int)province.army.id);
        actionRecruitArmy.SetActive(GameplayManager.Instance.player.country.id == province.countryID && province.army.id == ArmyID.None && province.landmark.id == LandmarkID.House);
        actionBuildChurch.SetActive(false);
        actionBuildHouse.SetActive(false);
        actionBuildTower.SetActive(false);
        actionDemolish.SetActive(false);

        textProvince.text = GameplayManager.Instance.GetProvinceText(province);
        textLandmark.text = GameplayManager.Instance.GetLandmarkText(province);
        textArmy.text = GameplayManager.Instance.GetArmyText(province);
    }

    public void ButtonTogglePanelTop()
    {
        panelTopOpened = !panelTopOpened;

        StopCoroutine("TogglePanelTop");
        StartCoroutine("TogglePanelTop");
    }

    private IEnumerator TogglePanelTop()
    {
        Vector2 position;
        if (panelTopOpened)
            position = Vector2.zero;
        else
            position = panelTopAnchoredPosition;

        while (rectTransformPanelTop.anchoredPosition != position)
        {
            rectTransformPanelTop.anchoredPosition = Vector2.Lerp(rectTransformPanelTop.anchoredPosition, position, Time.deltaTime * 5f);
            yield return null;
        }
    }

    public void UpdateTopPanel()
    {
        textGold.text = GameplayManager.Instance.player.country.gold.ToString();
        textGoldEarned.text = GameplayManager.Instance.player.country.goldEachRound.ToString();
        textArmyCount.text = GameplayManager.Instance.player.country.armyCount.ToString();
        textManpower.text = GameplayManager.Instance.player.country.manpower.ToString();
    }

    public void ButtonEnableArmyMovementIndicator()
    {
        if (province.army.lastActionRound == GameplayManager.Instance.round || !GameplayManager.Instance.player.GetTurn())
            return;

        armyMovementIndicator.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        armyMovementIndicator.SetActive(!armyMovementIndicator.activeSelf);

        armyMovementIndicatorCenter.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorCenter(tilePos));
        armyMovementIndicatorUp.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorUp(tilePos));
        armyMovementIndicatorDown.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorDown(tilePos));
        armyMovementIndicatorLeft.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorLeft(tilePos));
        armyMovementIndicatorRight.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorRight(tilePos));
    }

    public void DisableArmyMovementIndicator()
    {
        armyMovementIndicator.SetActive(false);
        armyMovementIndicatorCenter.SetActive(false);
        armyMovementIndicatorUp.SetActive(false);
        armyMovementIndicatorDown.SetActive(false);
        armyMovementIndicatorLeft.SetActive(false);
        armyMovementIndicatorRight.SetActive(false);
    }

    public void ButtonRecruitArmy()
    {
        bool armyPurchased = Game.Instance.countries[(int)province.countryID].BuyArmy(ref province, mousePos);
        if (armyPurchased)
        {
            UpdateTopPanel();
            DisplayPanelBottom();
        }
    }

    public void ButtonBuildChurch()
    {
        bool churchPurchased = Game.Instance.countries[(int)province.countryID].BuyLandmark(LandmarkID.Church, ref province, mousePos);
        if (churchPurchased)
        {
            UpdateTopPanel();
            DisplayPanelBottom();
        }
    }

    public void ButtonBuildHouse()
    {
        bool housePurchased = Game.Instance.countries[(int)province.countryID].BuyLandmark(LandmarkID.House, ref province, mousePos);
        if (housePurchased)
        {
            UpdateTopPanel();
            DisplayPanelBottom();
        }
    }

    public void ButtonBuildTower()
    {
        bool towerPurchased = Game.Instance.countries[(int)province.countryID].BuyLandmark(LandmarkID.Tower, ref province, mousePos);
        if (towerPurchased)
        {
            UpdateTopPanel();
            DisplayPanelBottom();
        }
    }

    public void ButtonDemolish()
    {
        bool demolished = Game.Instance.countries[(int)province.countryID].RemoveLandmark(ref province, mousePos);
        if (demolished)
        {
            UpdateTopPanel();
            DisplayPanelBottom();
        }
    }

    public void ButtonEndTurn()
    {
        GameplayManager.Instance.player.SetTurn(false);
    }

    public void ButtonHostMenu()
    {
        canvasMenu.gameObject.SetActive(false);
        canvasHost.gameObject.SetActive(true);
    }

    public void ButtonJoinMenu()
    {
        canvasMenu.gameObject.SetActive(false);
        canvasJoin.gameObject.SetActive(true);
    }

    public void ButtonGoBack()
    {
        canvasMenu.gameObject.SetActive(true);
        canvasHost.gameObject.SetActive(false);
        canvasJoin.gameObject.SetActive(false);
    }

    public void ButtonHost()
    {
        buttonHost.interactable = false;

        int port;
        try
        {
            port = int.Parse(inputFieldHostPort.text);
        }
        catch (Exception)
        {
            buttonHost.interactable = true;
            return;
        }

        if (Server.Server.Start(4, port))
        {
            // Disable host menu and enable game UI and map
            canvasHost.gameObject.SetActive(false);
            canvasGameUI.gameObject.SetActive(true);
            Game.Instance.map.gameObject.SetActive(true);
        }

        buttonHost.interactable = true;
    }

    public void ButtonJoin()
    {
        buttonJoin.interactable = false;

        IPAddress ipAddress;
        int port;

        try
        {
            ipAddress = IPAddress.Parse(inputFieldJoinIPAddress.text);
            port = int.Parse(inputFieldJoinPort.text);
        }
        catch (Exception)
        {

            buttonJoin.interactable = true;
            return;
        }


        Client.Client.Connect(ipAddress, port);
    }
}
