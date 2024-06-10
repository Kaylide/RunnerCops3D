using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    private int maxLevel;
    private int currentLevel;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        maxLevel = 5; // Set the max level to 5
        DontDestroyOnLoad(this.gameObject); // Don't destroy this game object when loading a new scene
        GetLevel(); // Call the GetLevel method
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel() 
    {
        currentLevel++; // Increase the current level by 1
        if (currentLevel > maxLevel) 
        {
            currentLevel = 1; 
        } // If the current level is greater than the max level, set the current level to 1

        PlayerPrefs.SetInt("keyLevel", currentLevel); // Save the current level in PlayerPrefs
        LoadLevel(); // Call the LoadLevel method
    }

    public void GetLevel()
    {
        currentLevel = PlayerPrefs.GetInt("keyLevel", 1);
        LoadLevel();
    } // Get the current level from PlayerPrefs

    private void LoadLevel() 
    {
        string LevelName = "LevelScene" + currentLevel;
        SceneManager.LoadScene(LevelName);
    } // Load the level scene
}
