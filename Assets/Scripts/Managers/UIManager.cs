using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject armyMovementIndicator;
    public GameObject armyMovementIndicatorUp;
    public GameObject armyMovementIndicatorDown;
    public GameObject armyMovementIndicatorLeft;
    public GameObject armyMovementIndicatorRight;
    public GameObject tileSelect;
    public GameObject tileShadow;

    public GameObject panelTop;
    public RectTransform rectTransformPanelTop;
    private Vector2 panelTopAnchoredPosition;
    private bool panelTopOpened = false;

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
            if (province == _province)  // If clicked to the same province, close the panel
            {
                DisablePanelBottom();
            }
            else    // If clicked to different province while panel was empty, update the panel
            {
                province = _province;

                // Move army
                GameplayManager.Instance.MoveArmy(ref province, tilePos, tilePosOld, mousePos, mousePosOld);

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
        actionMoveArmy.SetActive((int)GameplayManager.Instance.player.country.id == (int)province.armyID);
        actionRecruitArmy.SetActive(GameplayManager.Instance.player.country.id == province.countryID && province.armyID == ArmyID.None && province.landmarkID == LandmarkID.House);
        actionBuildChurch.SetActive(false);
        actionBuildHouse.SetActive(false);
        actionBuildTower.SetActive(false);
        actionDemolish.SetActive(false);

        textProvince.text = GameplayManager.Instance.GetProvinceText(province);
        textLandmark.text = GameplayManager.Instance.GetLandmarkText(province);
        textArmy.text = GameplayManager.Instance.GetArmyText(province);
    }

    public void ButtonEnableArmyMovementIndicator()
    {
        armyMovementIndicator.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        armyMovementIndicator.SetActive(!armyMovementIndicator.activeSelf);

        armyMovementIndicatorUp.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorUp(tilePos));
        armyMovementIndicatorDown.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorDown(tilePos));
        armyMovementIndicatorLeft.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorLeft(tilePos));
        armyMovementIndicatorRight.SetActive(GameplayManager.Instance.ShowArmyMovementIndicatorRight(tilePos));
    }

    public void DisableArmyMovementIndicator()
    {
        armyMovementIndicator.SetActive(false);
        armyMovementIndicatorUp.SetActive(false);
        armyMovementIndicatorDown.SetActive(false);
        armyMovementIndicatorLeft.SetActive(false);
        armyMovementIndicatorRight.SetActive(false);
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
}
