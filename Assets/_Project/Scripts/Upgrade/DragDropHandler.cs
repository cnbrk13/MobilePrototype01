using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropHandler : MonoBehaviour
{

    #region Variables

    [SerializeField] private float draggingZ = -1.1f;

    [Tooltip("How far to search for a valid gridCell when a piece is dropped")]
    [SerializeField] private float distanceTolerance = 1.5f;

    private bool dragging = false;
    private UpgradeItem toDrag;
    private float dist;
    private Vector3 offset;

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

        if (!dragging && Input.GetMouseButtonDown(0))
        {
            BeginDragging();
        }


        if (dragging)
        {
            ContinueDragging();
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            EndDragging();
        }

    }

    #endregion

    #region Methods

    private void EndDragging()
    {
        dragging = false;

        var grid = GameManager.Instance.UpgradeGrid;

        List<GridCell> closestCells = grid.ClosestCells(toDrag, distanceTolerance, 6);

        foreach (var c in closestCells)
        {
            bool canFit = grid.AreCellsFree(c, toDrag);
            bool isAdj = grid.IsAdjasent(c, toDrag.ItemSize);

            if (isAdj && canFit)
            {
                grid.Place(c, toDrag);
                //toDrag.transform.position = cellUnderneath.transform.position + Vector3.back * 0.4f;
                return;
            }
        }

        toDrag.RepositionToStart();
    }

    private void ContinueDragging()
    {
        var v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        v3 = Camera.main.ScreenToWorldPoint(v3);
        var final = v3 + offset;
        toDrag.transform.position = new Vector3(final.x, final.y, draggingZ);
    }


    private void BeginDragging()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            UpgradeItem i;
            hit.collider.gameObject.TryGetComponent<UpgradeItem>(out i);

            if (i != null && i.IsDraggable())
            {
                toDrag = i;
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                var v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                offset = toDrag.transform.position - v3;
                dragging = true;


                if (i.IsPlaced)
                {
                    i.Remove();
                }
            }
        }
    }



    #endregion

}
