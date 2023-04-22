using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    #region Variables
    [SerializeField] private bool shootWhenMouseDown = false;
    [SerializeField] private PlayerProjectile proj;
    [SerializeField] private Transform nozzle;

    [SerializeField] private float projectileSpeed = 15f;

    [SerializeField] private Transform projectileHolder;
    [SerializeField] private float defaultShootingIntervalInMs = 500f;

    private Animator animator;
    private List<ParticleSystem> particles;

    
    private readonly int shootAnimStartTrigger = Animator.StringToHash("StartShoot");
    private readonly int shootAnimStopTrigger = Animator.StringToHash("StopShoot");


    private Coroutine shooting_Co;
    private bool isShooting = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
        particles = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    private void Start()
    {

    }


    private void Update()
    {

        if (shootWhenMouseDown)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartShooting();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopShooting();
            }
        }
        else
        {
            if (!isShooting)
            {
                StartShooting();
            }
        }


    }

    #endregion

    #region Methods

    public void StartShootAnim()
    {
        animator.SetTrigger(shootAnimStartTrigger);
    }

    public void StopShootAnim()
    {
        animator.SetTrigger(shootAnimStopTrigger);
    }

    public void StartShooting()
    {
        if (isShooting)
        {
            return;
        }

        isShooting = true;

        if (shooting_Co != null)
        {
            StopCoroutine(shooting_Co);
        }

        float intervalMilliseconds = defaultShootingIntervalInMs;

        intervalMilliseconds = (1000f / GameManager.Instance.UpgradeGrid.CalculatedShootSpeed) * 4f;

        shooting_Co = StartCoroutine(ShootingLoop(intervalMilliseconds));

        StartShootAnim();
    }


    private void StopShooting()
    {
        if (shooting_Co != null)
        {
            StopCoroutine(shooting_Co);
            shooting_Co = null;
        }

        isShooting = false;

        StopShootAnim();
    }


    private IEnumerator ShootingLoop(float interval)
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(interval/1000f);
        }
    }


    private List<PlayerProjectile> SpawnProjectiles(int howManyAtOnce)
    {
        var projectiles = new List<PlayerProjectile>();

        for (int i = 0; i < howManyAtOnce; i++)
        {
            projectiles.Add(Instantiate(proj));
        }

        return projectiles;
    }


    [ContextMenu("Shoot")]
    private void Shoot()
    {
        var projectiles = SpawnProjectiles(4);
        PositionProjectiles(projectiles);
        MoveProjectiles(projectiles);
        PlayParticles();
    }

    private void PlayParticles()
    {
        foreach (var p in particles)
        {
            p.Play();
        }
    }

    private void PositionProjectiles(List<PlayerProjectile> projectiles)
    {
        var holder = new GameObject("Projectiles").transform;
        holder.SetParent(projectileHolder);

        var nozzlePos = nozzle.position;

        Vector3 gap = 0.06f * Vector3.up;

        var startPos = ((projectiles.Count - 1) / 2 * gap);

        for (int i = 0; i < projectiles.Count; i++)
        {
            var p = projectiles[i];

            p.transform.position = startPos - gap * i;
            p.transform.SetParent(holder);
            p.transform.LookAt(LevelManager.Instance.Player.Body.transform.forward, Vector3.up);
        }

        // set parent and rotate the group
        holder.transform.position = nozzle.position;

    }


    private void MoveProjectiles(List<PlayerProjectile> projectiles)
    {
        foreach (var p in projectiles)
        {
            MoveProjectile2(p);
        }
    }


    public Camera Camera;

    private void MoveProjectile(PlayerProjectile p)
    {
        // Create a ray from the camera going through the middle of your screen
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Check whether your are pointing to something so as to adjust the direction
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000); // You may need to change this value according to your needs
                                              // Create the bullet and give it a velocity according to the target point computed before
        
        p.transform.LookAt(targetPoint, Vector3.up);
        p.GetComponent<Rigidbody>().velocity = (targetPoint - transform.position).normalized * projectileSpeed;
    }

    private void MoveProjectile2(PlayerProjectile p)
    {
        var player = LevelManager.Instance.Player;
        p.GetComponent<Rigidbody>().velocity = player.Body.transform.forward * projectileSpeed;
    }

    #endregion

}
