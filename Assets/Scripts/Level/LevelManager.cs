using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private LevelName[] Levels;
    [SerializeField] private int puzzleLevelCount = 200;
    
    void Start()
    {
        UnlockFirstLevel();
        UnlockFirstPuzzleLevel();
    }

    private void UnlockFirstLevel()
    {
        // the very first time the game starts, the level1 should be unlocked to play by default
        if (GetLevelStatus(Levels[0]) == LevelStatus.Locked)
        {
            SetLevelStatus(Levels[0], LevelStatus.Unlocked);
        }
    }

    public void MarkLevelComplete()
    {
        // finding the index of the active level from the array of Levels
        int currentSceneIndex = Array.FindIndex(Levels, level => level.ToString() == SceneManager.GetActiveScene().name);

        SetLevelStatus(Levels[currentSceneIndex], LevelStatus.Completed);

        // unlock the next level
        if (currentSceneIndex + 1 < Levels.Length)
        {
            SetLevelStatus(Levels[currentSceneIndex + 1], LevelStatus.Unlocked);
        }
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public int GetLatestUnlockedLevelNo()
    {
        int i = 0;
        
        foreach (LevelName level in Levels)
        {
            if (GetLevelStatus(level) == LevelStatus.Unlocked)
            {
                i = Array.IndexOf(Levels, level);
            }
        }

        return i;
    }
    
    public void GoToLatestUnlockedLevel()
    {
        SceneManager.LoadScene(GetLatestUnlockedLevelNo() + 1);
    }
    
    public LevelStatus GetLevelStatus(LevelName level)
    {
        LevelStatus levelStatus = (LevelStatus)PlayerPrefs.GetInt(level.ToString(), 0);
        
        return levelStatus;
    }

    public void SetLevelStatus(LevelName level, LevelStatus levelStatus)
    {
        PlayerPrefs.SetInt(level.ToString(), (int)levelStatus);
    }

    public void ResetLevelProgress()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            SetLevelStatus(Levels[i], i == 0 ? LevelStatus.Unlocked : LevelStatus.Locked);
        }
    }

    private void UnlockFirstPuzzleLevel()
    {
        if (GetPuzzleLevelStatus(1) == LevelStatus.Locked)
        {
            SetPuzzleLevelStatus(1, LevelStatus.Unlocked);
        }
    }

    public void MarkPuzzleLevelComplete(int index)
    {
        SetPuzzleLevelStatus(index, LevelStatus.Completed);
        if (index < puzzleLevelCount)
        {
            SetPuzzleLevelStatus(index + 1, LevelStatus.Unlocked);
        }
    }

    public LevelStatus GetPuzzleLevelStatus(int index)
    {
        return (LevelStatus)PlayerPrefs.GetInt($"PuzzleLevel{index}", 0);
    }

    public void SetPuzzleLevelStatus(int index, LevelStatus status)
    {
        PlayerPrefs.SetInt($"PuzzleLevel{index}", (int)status);
    }

    #region Debug Methods
    [ContextMenu("Unlock All Levels")]
    private void UnlockAllLevels()
    {
        foreach (LevelName level in Levels)
        {
            PlayerPrefs.SetInt(level.ToString(), (int)LevelStatus.Unlocked);
        }
    }
    #endregion
}