using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerSecondTextUI : MonoBehaviour
{

    #region Variables
    private TextMeshProUGUI tmp;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.BaseShootSpeedChanged += UpdateText;
        GameManager.Instance.UpgradeGrid.ItemAddedOrRemoved += UpdateText;
    }

    private void Update()
    {

    }

    #endregion

    #region Methods

    private void UpdateText(float newVal)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        tmp.text = GameManager.Instance.UpgradeGrid.CalculatedShootSpeed * 2 + "/s";
    }


    #endregion

}
