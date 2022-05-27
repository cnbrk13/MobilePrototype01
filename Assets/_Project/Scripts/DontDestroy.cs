using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    #region Variables

    #endregion

    #region Unity Methods

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    #endregion

    #region Methods

    #endregion

}
