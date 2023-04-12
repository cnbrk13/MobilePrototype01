using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    #region Variables


    [Tooltip("Which checkpoint activates enemies")]
    [SerializeField] private int activatedByCheckpoint = -1;


    [Tooltip("Speed to move towards player once activated")]
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private int startingHealth = 4;

    private int currentHealth;
    private Animator animator;
    private Rigidbody rigidbody;
    private bool isActivated = false;
    private Vector3 desiredVel = Vector3.zero;
    private Slider slider;


    #endregion

    #region Unity Methods

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator =  GetComponentInChildren<Animator>();
        slider = GetComponentInChildren<Slider>();
        currentHealth = startingHealth;
    }

    private void Start()
    {
        LevelManager.Instance.Player.MoveAnimator.CheckpointReached += OnCheckpointReached;
        slider.value = 1;
        animator.SetFloat("CycleOffsetVal", UnityEngine.Random.value);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            CalculateDesiredVel();
            rigidbody.velocity = desiredVel;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerProjectile p = other.gameObject.GetComponent<PlayerProjectile>();

        if (p != null)
        {
            TookHitFrom(p);
        }
    }






    #endregion

    #region Methods


    private void TookHitFrom(PlayerProjectile p)
    {
        p.Explode();
        currentHealth--;

        slider.value = currentHealth / (float)startingHealth;

        if (currentHealth == 0)
        {
            Die();
        }
    }


    private void OnCheckpointReached(int checkpointNo)
    {
        if (checkpointNo == activatedByCheckpoint)
        {
            Activate();
        }
    }

    private Vector3 MoveTo(Vector3 from, Vector3 to, float speed)
    {
        var dir = (to - from).normalized;
        //transform.LookAt(to);
        return dir * speed;
    }

    private void CalculateDesiredVel()
    {
        var dest = LevelManager.Instance.Player.transform.position;
        desiredVel = MoveTo(transform.position, dest, moveSpeed);
    }

    private void Activate()
    {
        if (this != null)
        {
            LevelManager.Instance.EnemyActivated(this);
            isActivated = true;
        }
    }

    private void Die()
    {
        LevelManager.Instance.EnemyDied(this);
        Destroy(gameObject);
    }

    #endregion

}
