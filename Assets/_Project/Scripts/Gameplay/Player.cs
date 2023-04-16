using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Variables


    private PlayerAnimator playerAnimator;
    private PlayerWeapon weapon;

    public PlayerAnimator PlayerAnimator => playerAnimator;

    public PlayerWeapon Weapon => weapon;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Start()
    {
        playerAnimator.StartMoving();
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            LevelManager.Instance.PlayerDied();
        }
    }

    #endregion

    #region Methods


    #endregion

}
