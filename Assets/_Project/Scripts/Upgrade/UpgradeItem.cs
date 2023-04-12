using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeItem : MonoBehaviour
{

    #region Variables

    [Tooltip("Width and Height in grid cells")]
    [SerializeField] private Vector2 size;


    [SerializeField] private int plusBonus;
    [SerializeField] private int timesBonus;

    public Vector2 ItemSize => size;
    public bool IsPlaced { get; private set; } = false;

    public int PlusBonus => plusBonus;
    public int TimesBonus => timesBonus;

    private List<GridCell> occupiedCells = new List<GridCell>();
    private Vector3 startPos;

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 3);
    }

    #endregion

    #region Methods

    // returns the closest cell to UpgradeItem transform center
    public GridCell GetCellUnderneath()
    {
        RaycastHit hit;

        var cast = Physics.Raycast(transform.position, Vector3.forward, out hit);

        if (cast)
        {
            GridCell cell;
            hit.collider.gameObject.TryGetComponent<GridCell>(out cell);

            if (cell != null)
            {
                return cell;
            }
        }

        return null;
    }


    public void RepositionToStart()
    {
        transform.position = startPos;
    }

    public void SetOccupiedCells(List<GridCell> cells)
    {
        IsPlaced = true;
        occupiedCells = cells;
    }


    public void Remove()
    {
        IsPlaced = false;
        GameManager.Instance.UpgradeGrid.RemoveItem(this);
    }

    /// <summary>
    /// Only draggable if !placed or mostright on the grid
    /// </summary>
    /// <returns></returns>
    public bool IsDraggable()
    {
        if (!IsPlaced)
        {
            return true;
        }

        var grid = GameManager.Instance.UpgradeGrid;

        var mostRightXCoordOccupiedByItem = occupiedCells.Max(c => c.col);
        List<GridCell> mostRightCellsOccupiedByItem = occupiedCells.Where(c => c.col == mostRightXCoordOccupiedByItem).ToList();

        return mostRightCellsOccupiedByItem.TrueForAll(c => grid.IsCellMostright(c));
    }

    /// <summary>
    /// returns the cells occupied by the item
    /// </summary>
    /// <returns></returns>
    public List<GridCell> GetOccupiedCells()
    {
        return occupiedCells;
    }




    #endregion

}
