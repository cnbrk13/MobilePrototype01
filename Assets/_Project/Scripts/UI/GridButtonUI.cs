using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridButtonUI : MonoBehaviour
{

    #region Variables
    [SerializeField] private TextMeshProUGUI costTMP;
    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        UpdateCostTMP();
    }

    private void Update()
    {

    }

    #endregion

    #region Methods

    public void OnGridIncreaseClicked()
    {
        GameManager.Instance.TryPurchaseGridUpgrade();
        UpdateCostTMP();
    }


    private void UpdateCostTMP()
    {
        costTMP.text = GameManager.Instance.GridUpgradeCost.ToString();
    }


    #endregion

}
