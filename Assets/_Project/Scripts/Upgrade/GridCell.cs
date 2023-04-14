using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    #region Variables

    [SerializeField] private bool isActive, isEmpty;
    [SerializeField] private Material activeMat, disabledMat;


    [Tooltip("Gap between cells")]
    public float margin = 0f;

    public bool IsActive => isActive;
    public bool IsEmpty => isEmpty;

    public int row, col;
    public int unlockOrder;


    private MeshRenderer rend;
    private Coroutine shootingCO;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
    }

    private void Update()
    {



    }

    #endregion

    #region Methods

    public void Reposition(int row, int col, int maxRows, int maxColumns)
    {
        var scale = GameManager.Instance.GridCellScale;
        transform.localScale = Vector3.one * scale;

        this.row = row;
        this.col = col;

        transform.position = new Vector3(col * scale + col * margin, -row * scale - row * margin, 0);
    }

    public void SetActive()
    {
        isActive = true;
        rend.material = activeMat;
    }

    public void SetFull()
    {
        isEmpty = false;
        rend.material = disabledMat;
    }

    public void SetEmpty()
    {
        isEmpty = true;
        rend.material = activeMat;
    }

    internal void StartShooting()
    {
        if (shootingCO != null)
        {
            return;
        }

        shootingCO = StartCoroutine(ShootingLoop());
    }

    internal void StopShooting()
    {
        if (shootingCO != null)
        {
            StopCoroutine(shootingCO);
            shootingCO = null;
        }
    }


    private IEnumerator ShootingLoop()
    {
        while (true)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(ShootInterval());
        }
    }


    private float ShootInterval()
    {
        int filledCellCount = GameManager.Instance.UpgradeGrid.FilledCellsToTheLeft(this);
        return GameManager.Instance.UpgradeGrid.CellShootInterval/(filledCellCount + 1);
    }

    private void SpawnProjectile()
    {
        GridProjectile proj = Instantiate(GameManager.Instance.GridProjPrefab);

        proj.transform.position = transform.position - new Vector3(0, 0, 0.5f);
    }

    #endregion

}
