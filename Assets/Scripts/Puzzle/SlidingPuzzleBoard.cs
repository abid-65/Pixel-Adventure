using UnityEngine;

public class SlidingPuzzleBoard : MonoBehaviour
{
    [SerializeField] private PuzzleLevelData levelData;
    [SerializeField] private Transform[] tilePositions;
    [SerializeField] private Transform[] tiles;

    private int size;
    private int[] board;
    private int currentLevelIndex;

    void Start()
    {
        if (levelData != null)
            LoadLevel(levelData);
    }

    public void LoadLevel(PuzzleLevelData data)
    {
        levelData = data;
        currentLevelIndex = data.levelIndex;
        size = data.size;
        board = (int[])data.tiles.Clone();
        UpdateTilePositions();
    }

    private void UpdateTilePositions()
    {
        for (int i = 0; i < board.Length; i++)
        {
            int tileNumber = board[i];
            if (tileNumber > 0 && tileNumber - 1 < tiles.Length)
            {
                tiles[tileNumber - 1].position = tilePositions[i].position;
            }
        }
    }

    private bool IsAdjacent(int a, int b)
    {
        int x1 = a % size; int y1 = a / size;
        int x2 = b % size; int y2 = b / size;
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2) == 1;
    }

    public void Slide(int tileNumber)
    {
        int tileIndex = System.Array.IndexOf(board, tileNumber);
        int emptyIndex = System.Array.IndexOf(board, 0);
        if (IsAdjacent(tileIndex, emptyIndex))
        {
            board[emptyIndex] = tileNumber;
            board[tileIndex] = 0;
            UpdateTilePositions();
            if (CheckForVictory())
            {
                LevelManager.Instance.MarkPuzzleLevelComplete(currentLevelIndex);
            }
        }
    }

    private bool CheckForVictory()
    {
        for (int i = 0; i < board.Length - 1; i++)
        {
            if (board[i] != i + 1) return false;
        }
        return board[board.Length - 1] == 0;
    }
}
