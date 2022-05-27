using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    #region Variables

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        Destroy(this, 1.5f);
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Arena")
        {
            Explode();
        }
    }


    #endregion

    #region Methods

    public void Explode()
    {
        Destroy(gameObject);
    }

    #endregion

}
