using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeGrid : MonoBehaviour
{

    #region Variables

    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private float placedItemsZOffset = 0.6f;

    [Tooltip("How often should cells with items spawn projectiles")]
    [SerializeField] private float cellShootInterval;

    public int columns;
    public int rows;
    public Action ItemAddedOrRemoved;

    private List<GridCell> cells = new List<GridCell>();
    private List<UpgradeItem> placedItems = new List<UpgradeItem>();

    private int unlockedCells = 0;
    private readonly int maxUnlocked = 25;

    public bool CanIncreaseSize => unlockedCells < maxUnlocked;

    public float CalculatedShootSpeed => CalculateShootingSpeed();
    public float CellShootInterval => cellShootInterval;

    // returns cells count that are currently shooting (or most right)
    public int TotalShootingFromCount => cells.Count(c => ShouldShootFromCell(c));


    public int UnlockedCells => unlockedCells;

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        //CreateGrid();
        FillCellsList();
    }

    private void Update()
    {

    }

    private void OnValidate()
    {
    }

    #endregion

    #region Methods

    private float CalculateShootingSpeed()
    {
        var placed = GetPlacedItems();

        int totalPlus = 1;
        int totalTimes = 1;

        foreach (var i in placed)
        {
            totalPlus += i.PlusBonus;
            totalTimes += i.TimesBonus;
        }

        return (float)Math.Round(GameManager.Instance.BaseShootSpeed * totalPlus * totalTimes, 2);
    }

    public List<UpgradeItem> GetPlacedItems()
    {
        return placedItems;
    }

    [ContextMenu("RecreateGrid")]
    private void CreateGrid()
    {
        DestroyGrid();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GridCell c = Instantiate(cellPrefab);
                c.transform.SetParent(transform);
                c.Reposition(i, j, rows, columns);
                cells.Add(c);
            }
        }
    }




    private void DestroyGrid()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        cells.Clear();
    }


    private void FillCellsList()
    {
        foreach (Transform child in transform)
        {
            var cell = child.GetComponent<GridCell>();
            cells.Add(cell);
        }
    }



    internal void IncreaseGridSize()
    {
        cells.First(c => c.unlockOrder == unlockedCells).SetActive();
        unlockedCells++;
    }

    internal int FilledCellsToTheLeft(GridCell gridCell)
    {
        return cells.Count(c => !c.IsEmpty && c.row == gridCell.row && c.col < gridCell.col && c.col >= 0);
    }


    /// <summary>
    /// returns true if the cells at every coordinate of item will fit are IsActive and IsEmpty
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AreCellsFree(GridCell cell, UpgradeItem item)
    {
        return AreCellsFree(cell, item.ItemSize);
    }


    /// <summary>
    /// returns true if the cell at a coordinate is IsActive and IsEmpty
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    private bool IsCellFree(int col, int row)
    {
        var targetCell = GetCell(col, row);

        if (targetCell == null || !targetCell.IsActive || !targetCell.IsEmpty)
        {
            return false;
        }

        return true;
    }


    private bool AreCellsFree(GridCell cell, Vector2 size)
    {
        int col = cell.col;
        int row = cell.row;

        if (size.x == 1 && size.y == 1)
        {
            return IsCellFree(col, row);
        }

        if (size.x == 2 && size.y == 1)
        {
            return IsCellFree(col, row) && IsCellFree(col + 1, row);
        }

        if (size.x == 1 && size.y == 2)
        {
            return IsCellFree(col, row) && IsCellFree(col, row - 1);
        }

        if (size.x == 1 && size.y == 3)
        {
            return IsCellFree(col, row) && IsCellFree(col, row + 1) && IsCellFree(col, row - 1);
        }

        return false;
    }

    /// <summary>
    /// returns true if there is an upgrade
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool IsAdjasent(GridCell cell, Vector2 size)
    {
        int col = cell.col;
        int row = cell.row;

        if (size.x == 1 && size.y == 1)
        {
            var c = GetCell(col - 1, row);

            // if not empty, means we are adjasent to an existing one
            return (c != null && !c.IsEmpty);
        }

        if (size.x == 2 && size.y == 1)
        {
            var c = GetCell(col - 1, row);

            return (c != null && !c.IsEmpty);
        }

        if (size.x == 1 && size.y == 2)
        {
            var c = GetCell(col - 1, row);
            var c2 = GetCell(col - 1, row - 1);

            return (c != null && !c.IsEmpty) || (c2 != null && !c2.IsEmpty);
        }

        if (size.x == 1 && size.y == 3)
        {
            var c = GetCell(col - 1, row);
            var c2 = GetCell(col - 1, row - 1);
            var c3 = GetCell(col - 1, row + 1);

            return (c != null && !c.IsEmpty) || (c2 != null && !c2.IsEmpty) || (c3 != null && !c3.IsEmpty);
        }

        return false;
    }


    public List<GridCell> ClosestCells(UpgradeItem item, float maxDist, int howMany)
    {
        var closest = cells.Where(c => Vector3.Distance(item.transform.position, c.transform.position) < maxDist)
            .OrderBy(c => Vector3.Distance(item.transform.position, c.transform.position))
            .Take(howMany).ToList();

        return closest;
    }


    public bool IsCellMostright(GridCell c)
    {
        var cellToTheRight = GetCell(c.col + 1, c.row);

        if (cellToTheRight == null)
        {
            return true;
        }

        if (!cellToTheRight.IsActive)
        {
            return true;
        }

        if (cellToTheRight.IsEmpty)
        {
            return true;
        }

        return false;
    }


    private GridCell GetCell(Vector2 coord)
    {
        return GetCell((int)coord.x, (int)coord.y);
    }

    private GridCell GetCell(int x, int y)
    {
        return cells.FirstOrDefault(c => c.col == x && c.row == y);
    }

    private void SetCellAtCoordFull(int x, int y)
    {
        var c = GetCell(x, y);
        c.SetFull();
    }

    private void SetCellAtCoordEmpty(int x, int y)
    {
        var c = GetCell(x, y);
        c.SetEmpty();
    }

    public void Place(GridCell cell, UpgradeItem item)
    {
        var occupiedCells = new List<GridCell>();
        occupiedCells.Add(cell);

        cell.SetFull();

        int col = cell.col;
        int row = cell.row;

        var size = item.ItemSize;

        var zOffset = Vector3.back * placedItemsZOffset;

        if (size.x == 1 && size.y == 1)
        {
            SetCellAtCoordFull(col, row);
            item.transform.position = cell.transform.position + zOffset;
        }

        if (size.x == 2 && size.y == 1)
        {
            SetCellAtCoordFull(col, row);
            SetCellAtCoordFull(col + 1, row);

            var c1 = GetCell(col, row);
            var c2 = GetCell(col + 1, row);

            var pos = (c1.transform.position + c2.transform.position) / 2f;

            item.transform.position = pos + zOffset;

            occupiedCells.Add(c2);
        }

        if (size.x == 1 && size.y == 2)
        {
            SetCellAtCoordFull(col, row);
            SetCellAtCoordFull(col, row - 1);

            var c1 = GetCell(col, row);
            var c2 = GetCell(col, row - 1);

            var pos = (c1.transform.position + c2.transform.position) / 2f;

            item.transform.position = pos + zOffset;

            occupiedCells.Add(c2);
        }

        if (size.x == 1 && size.y == 3)
        {
            SetCellAtCoordFull(col, row);
            SetCellAtCoordFull(col, row + 1);
            SetCellAtCoordFull(col, row - 1);

            var c1 = GetCell(col, row);
            var c2 = GetCell(col, row + 1);
            var c3 = GetCell(col, row - 1);

            var pos = (c1.transform.position + c2.transform.position + c3.transform.position) / 3f;

            item.transform.position = pos + zOffset;

            occupiedCells.Add(c2);
            occupiedCells.Add(c3);
        }

        item.SetOccupiedCells(occupiedCells);
        placedItems.Add(item);
        ItemAddedOrRemoved?.Invoke();




        CheckWhichCellsShouldShoot();

    }


    private void CheckWhichCellsShouldShoot()
    {
        foreach (var c in cells)
        {
            if (ShouldShootFromCell(c))
            {
                c.StartShooting();
            }
            else
            {
                c.StopShooting();
            }
        }
    }


    /// <summary>
    /// Sets cells the item is sitting on free
    /// </summary>
    /// <param name="upgradeItem"></param>
    public void RemoveItem(UpgradeItem upgradeItem)
    {
        foreach (var c in upgradeItem.GetOccupiedCells())
        {
            c.SetEmpty();
            c.StopShooting();
        }

        placedItems.Remove(upgradeItem);
        ItemAddedOrRemoved?.Invoke();

        CheckWhichCellsShouldShoot();
    }


    public bool ShouldShootFromCell(GridCell cell)
    {
        if (cell.IsEmpty)
        {
            return false;
        }

        if (cell.col < 0)
        {
            return false;
        }

        return IsCellMostright(cell);
    }




    #endregion

}
