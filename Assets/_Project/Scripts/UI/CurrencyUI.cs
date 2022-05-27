using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{

    #region Variables
    [SerializeField] private TextMeshProUGUI cucrrencyTMP;
    #endregion

    #region Unity Methods

    private void Start()
    {
        GameManager.Instance.CurrencyChanged += UpdateUI;
    }

    #endregion


    private void UpdateUI(int newCurrency)
    {
        string currency = string.Format("{0:00000}", newCurrency);
        cucrrencyTMP.text = currency;
    }

}
