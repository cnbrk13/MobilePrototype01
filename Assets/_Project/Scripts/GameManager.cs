using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Variables

    [SerializeField] private float gridCellScale = 0.5f;
    [SerializeField] private UpgradeGrid upgradeGrid;

    [SerializeField] private int currency = 900;
    [SerializeField] private float baseShootSpeed = 1.2f;
    [SerializeField] private float shootSpeedPerUpgrade = 0.1f;


    [SerializeField] private int gridUpgradeCost = 50;
    [SerializeField] private int gridUpgradeCostIncrease = 100;


    [SerializeField] private int speedUpgradeCost = 50;
    [SerializeField] private int speedUpgradeCostIncrease = 100;

    [SerializeField] private GridProjectile gridProjPrefab;





    public float GridCellScale => gridCellScale;
    public float ShootSpeed => baseShootSpeed;
    public float TweeningCurrency => currencyTweening;
    public int GridUpgradeCost => gridUpgradeCost;
    public int SpeedUpgradeCost => speedUpgradeCost;
    public GridProjectile GridProjPrefab => gridProjPrefab;
    public UpgradeGrid UpgradeGrid => upgradeGrid;
    public float BaseShootSpeed => baseShootSpeed;


    public Action<int> CurrencyChanged;
    public Action<float> BaseShootSpeedChanged;



    private Tween currencyTween;
    private float currencyTweening = 0; // used to tween and display currency

    #endregion

    #region Unity Methods





    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //BaseShootSpeedChanged?.Invoke(baseShootSpeed);
        LoadData();
    }

    private void Update()
    {

    }

    #endregion

    #region Methods



    private void LoadData()
    {
        Debug.Log("Load Data");
        Debug.Log("Load Currency: " + GameData.Currency);
        Debug.Log("Load BaseShootSpeed: " + GameData.BaseShootSpeed);
        Debug.Log("Load GridUnlocks: " + GameData.GridUnlocks);

        SetCurrency(GameData.Currency);
        SetBaseShootSpeed(GameData.BaseShootSpeed);
        SetGridUnlocks(GameData.GridUnlocks);
    }

    private void SaveData()
    {
        Debug.Log("SaveData");
        Debug.Log("SaveData Currency: " + currency);
        Debug.Log("SaveData BaseShootSpeed: " + baseShootSpeed);
        Debug.Log("SaveData GridUnlocks: " + upgradeGrid.UnlockedCells);
        Debug.Log("SaveData CalculatedShootSpeed: " + upgradeGrid.CalculatedShootSpeed);

        GameData.Currency = currency;
        GameData.BaseShootSpeed = baseShootSpeed;
        GameData.GridUnlocks = upgradeGrid.UnlockedCells;
        GameData.CalculatedShootSpeed = upgradeGrid.CalculatedShootSpeed;
        GameData.HowManyAtOnce = upgradeGrid.TotalShootingFromCount;
    }

    public bool CanAfford(int amount)
    {
        return currency >= amount;
    }

    private void AddCurrency(int amount)
    {
        ResetCurrencyTween();

        var curr = currency;
        var target = curr + amount;
        currency = target;
        currencyTween = DOTween.To(() => currencyTweening, x => currencyTweening = x, target, 0.5f).SetUpdate(true)
            .OnUpdate(() => CurrencyChanged?.Invoke((int)currencyTweening))
            .OnComplete(() => CurrencyChanged?.Invoke((int)target));
    }


    private void SetCurrency(int newVal)
    {
        currency = newVal;
        CurrencyChanged?.Invoke(newVal);
    }

    private void SetBaseShootSpeed(float newVal)
    {
        baseShootSpeed = newVal;
        BaseShootSpeedChanged?.Invoke(baseShootSpeed);
    }

    public void TryPurchaseGridUpgrade()
    {
        if (CanAfford(gridUpgradeCost))
        {
            IncreaseGridSize();
        }
    }

    public void TryPurchaseSpeedUpgrade()
    {
        if (CanAfford(speedUpgradeCost))
        {
            AddCurrency(-speedUpgradeCost);
            speedUpgradeCost += speedUpgradeCostIncrease;
            SetBaseShootSpeed(baseShootSpeed + shootSpeedPerUpgrade);
        }
    }

    private void ResetCurrencyTween()
    {
        if (currencyTween != null && currencyTween.active)
        {
            currencyTween.Complete();
            currencyTween.Kill();
        }
    }

    private void IncreaseGridSize()
    {
        if (UpgradeGrid.CanIncreaseSize)
        {
            AddCurrency(-gridUpgradeCost);
            gridUpgradeCost += gridUpgradeCostIncrease;

            upgradeGrid.IncreaseGridSize();
        }
    }


    private void SetGridUnlocks(int gridUnlocks)
    {
        for (int i = 0; i < gridUnlocks; i++)
        {
            upgradeGrid.IncreaseGridSize();
        }
    }


    public void ChangeToBattleScene()
    {
        SaveData();
        SceneManager.LoadScene("GameplayScene");
    }

    #endregion

}
