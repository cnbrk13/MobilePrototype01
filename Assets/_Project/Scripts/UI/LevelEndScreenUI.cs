using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndScreenUI : MonoBehaviour
{

    #region Variables
    [SerializeField] private GameObject failGroup;
    [SerializeField] private GameObject successGroup;
    [SerializeField] private Image bgImage;

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


    public void ShowFail()
    {
        failGroup.SetActive(true);
        bgImage.gameObject.SetActive(true);
    }

    public void ShowSuccess()
    {
        successGroup.SetActive(true);
        bgImage.gameObject.SetActive(true);
    }

    public void OnNextLevelClicked()
    {
        LevelManager.Instance.NextLevel();
    }


    public void OnRestartClicked()
    {
        LevelManager.Instance.RestartGame();
    }

    #endregion

}
