using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }


    #region Variables
    [Tooltip("A reference to the player in scene")]
    [SerializeField] private Player player;
    [SerializeField] private List<Enemy> activeEnemies = new();


    [SerializeField] private LevelEndScreenUI levelEndScreen;

    public Player Player => player;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }




    #endregion

    #region Methods

    public void EnemyActivated(Enemy e)
    {
        activeEnemies.Add(e);
    }


    public void EnemyDied(Enemy e)
    {
        activeEnemies.Remove(e);

        // if this is the last enemy, resume player movement 
        if (activeEnemies.Count == 0)
        {
            player.MoveAnimator.ContinueMoveAnimation();
        }
    }

    public void LootChestUnlocked()
    {
        levelEndScreen.ShowSuccess();
    }

    public void PlayerDied()
    {
        levelEndScreen.ShowFail();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("UpgradeScene");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("UpgradeScene");
    }

    #endregion

}
