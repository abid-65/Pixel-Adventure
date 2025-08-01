using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/Level Data")]
public class PuzzleLevelData : ScriptableObject
{
    public int levelIndex;
    public int size = 3;
    public int[] tiles;
}
