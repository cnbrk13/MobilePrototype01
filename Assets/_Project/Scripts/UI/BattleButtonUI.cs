using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButtonUI : MonoBehaviour
{

    #region Variables
    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    #endregion

    #region Methods

    public void OnBattleClicked()
    {
        ChangeScene();
    }

    private void ChangeScene()
    {
        //SceneManager.LoadScene("UpgradeScene");
        GameManager.Instance.ChangeToBattleScene();
    }

    #endregion

}
