using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test02 : MonoBehaviour
{

    #region Variables

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        Debug.Log("test02 start");
        StartCoroutine(PrintWithDelay());
    }

    private void Update()
    {

    }

    #endregion

    #region Methods


    private IEnumerator PrintWithDelay()
    {

        yield return new WaitForSeconds(1f);
        yield return null;

    }

    #endregion

}
