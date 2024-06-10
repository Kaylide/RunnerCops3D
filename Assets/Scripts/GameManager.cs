using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject failPanel;
    public GameObject congratsPanel;
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonTapped() 
    {
        Debug.Log("Start button tapped!!!");
        MainPanel.SetActive(false);
        GameObject PlayerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        PlayerSpawnerController playerSpawner = PlayerSpawnerGO.GetComponent<PlayerSpawnerController>();
        playerSpawner.MovePlayer();
    }

    public void ShowFailPanel() 
    {
        failPanel.SetActive(true);
    }

    public void RestartButtonTapped()
    {
        LevelLoader.instance.GetLevel();
    }

    public void ShowCongratsPanel() 
    {
        congratsPanel.SetActive(true);
    }

    public void NextLevelButtonTapped() 
    {
        LevelLoader.instance.NextLevel();
    }
}
