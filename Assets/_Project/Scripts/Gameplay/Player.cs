using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Variables


    private PlayerMoveAnimator moveAnimator;
    private PlayerWeapon weapon;

    public PlayerMoveAnimator MoveAnimator => moveAnimator;

    public PlayerWeapon Weapon => weapon;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        moveAnimator = GetComponent<PlayerMoveAnimator>();
    }

    private void Start()
    {
        moveAnimator.StartMoving();
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
