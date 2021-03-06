using System;
using System.Collections;
using System.Collections.Generic;
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

    private Coroutine shooting_Co;
    private bool isShooting = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {

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

        if (GameData.CalculatedShootSpeed != 0)
        {
            intervalMilliseconds = 1000f / GameManager.Instance.UpgradeGrid.CalculatedShootSpeed;
        }

        shooting_Co = StartCoroutine(ShootingLoop(intervalMilliseconds));
    }


    private void StopShooting()
    {
        if (shooting_Co != null)
        {
            StopCoroutine(shooting_Co);
            shooting_Co = null;
        }

        isShooting = false;
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
        int howMany = GameData.HowManyAtOnce != -1 ? GameData.HowManyAtOnce : 5;

        var projectiles = SpawnProjectiles(howMany);
        PositionProjectiles(projectiles);
        MoveProjectiles(projectiles);
    }

    private void PositionProjectiles(List<PlayerProjectile> projectiles)
    {
        var parent = new GameObject("Projectiles");
        parent.transform.SetParent(projectileHolder);

        var nozzlePos = nozzle.position;
        float projSize = 0.025f;


        nozzlePos = Vector3.zero;

        Vector3 gap = projSize * Vector3.up;

        var startPos = nozzlePos + ((projectiles.Count - 1) / 2 * gap);

        for (int i = 0; i < projectiles.Count; i++)
        {
            var p = projectiles[i];

            p.transform.position = startPos - gap * i;
            p.transform.SetParent(parent.transform);
        }

        // set parent and rotate the group
        parent.transform.position = nozzle.position;
        parent.transform.rotation = Quaternion.AngleAxis(-45, parent.transform.forward);
    }


    private void MoveProjectiles(List<PlayerProjectile> projectiles)
    {
        foreach (var p in projectiles)
        {
            MoveProjectile(p);
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
        p.GetComponent<Rigidbody>().velocity = (targetPoint - transform.position).normalized * projectileSpeed;
    }

    #endregion

}
