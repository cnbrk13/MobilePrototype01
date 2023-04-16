using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    #region Variables

    private ParticleSystem explodePS;


    #endregion

    #region Unity Methods

    private void Awake()
    {
        explodePS = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        Destroy(this, 1.5f);
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Arena")
        {
            Explode();
        }
    }




    #endregion

    #region Methods

    public void Explode()
    {
        explodePS.transform.parent = null;
        explodePS.Play();
        Destroy(gameObject);
    }

    #endregion

}
