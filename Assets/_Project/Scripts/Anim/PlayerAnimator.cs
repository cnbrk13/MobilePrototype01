using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    #region Variables
    private Animator animator;

    private readonly int moveAnimStartTrigger = Animator.StringToHash("StartMove");

    public Action<int> CheckpointReached;

    private int checkpointNr = 0;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueMoveAnimation();
        }


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StartMoving();
        }
    }

    #endregion

    #region Methods

    public void StartMoving()
    {
        animator.SetTrigger(moveAnimStartTrigger);
    }



    /// <summary>
    /// Called on animation events (on checkpoints)
    /// </summary>
    public void AnimEventHit()
    {
        OnCheckpointReached();
    }

    private void OnCheckpointReached()
    {
        PauseMoveAnimation();
        CheckpointReached?.Invoke(checkpointNr);
        checkpointNr++;
    }

    public void PauseMoveAnimation()
    {
        animator.speed = 0;
    }

    public void ContinueMoveAnimation()
    {
        animator.speed = 1;
    }

    #endregion

}
