using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootChest : MonoBehaviour
{

    #region Variables
    [SerializeField] private int hitsToUnlock = 50;
    
    private TextMeshPro tmp;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        tmp.text = hitsToUnlock.ToString();
    }

    private void Update()
    {

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
        hitsToUnlock--;
        tmp.text = hitsToUnlock.ToString();

        if (hitsToUnlock == 0)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        Destroy(gameObject);
        LevelManager.Instance.LootChestUnlocked();
    }

    #endregion

}
