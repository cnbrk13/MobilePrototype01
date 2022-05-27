using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProjectile : MonoBehaviour
{

    #region Variables
    [SerializeField] private float speed;

    private Rigidbody rigidbody;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigidbody.velocity = new Vector3(speed, 0, 0);
        Destroy(this, 1.5f);
    }

    private void Update()
    {

    }

    #endregion

    #region Methods


    #endregion

}
