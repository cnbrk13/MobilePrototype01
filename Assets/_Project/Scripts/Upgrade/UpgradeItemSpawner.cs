using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItemSpawner : MonoBehaviour
{

    #region Variables

    [SerializeField] private Vector3 startPos;
    [SerializeField] private float gapSize = 1.35f;


    [SerializeField] List<UpgradeItem> prefabs;


    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        SpawnRandom();
    }

    private void Update()
    {

    }

    #endregion

    #region Methods

    private void SpawnRandom()
    {
        for (int i = 0; i < 4; i++)
        {
            var randomPrefab = prefabs[Random.Range(0, prefabs.Count)];
            var item = Instantiate(randomPrefab);
            item.transform.position = startPos + i * gapSize * Vector3.right;
        }
    }

    #endregion

}
