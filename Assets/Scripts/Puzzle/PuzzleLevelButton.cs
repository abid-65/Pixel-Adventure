using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PuzzleLevelButton : MonoBehaviour
{
    [SerializeField] private PuzzleLevelData levelData;
    [SerializeField] private SlidingPuzzleBoard board;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void Start()
    {
        btn.interactable = LevelManager.Instance.GetPuzzleLevelStatus(levelData.levelIndex) != LevelStatus.Locked;
    }

    private void OnClick()
    {
        LevelStatus status = LevelManager.Instance.GetPuzzleLevelStatus(levelData.levelIndex);
        if (status != LevelStatus.Locked)
        {
            board.LoadLevel(levelData);
        }
    }
}
