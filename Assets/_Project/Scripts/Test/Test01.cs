using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test01 : MonoBehaviour
{

    #region Variables

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        StartCoroutine(ChangeSceneWithDelay());
    }

    private void Update()
    {

    }

    #endregion

    #region Methods


    private IEnumerator ChangeSceneWithDelay()
    {

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameplayScene");
        yield return null;

    }

    #endregion

}
