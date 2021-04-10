using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public UIManager uiManager;
    public SpriteManager spriteManager;
    public GameplayManager gameplayManager;

    public Camera cam;

    public Map map;

    public Country[] countries;

    void Awake()
    {
        Instance = this;
        QualitySettings.vSyncCount = 1;

        // Initialize managers
        uiManager.Init();
        spriteManager.Init();
        gameplayManager.Init();
    }

    void Update()
    {

    }
}
