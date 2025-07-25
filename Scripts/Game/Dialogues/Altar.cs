using UnityEngine;
using YG;

public class Altar : MonoBehaviour
{
    private const string PuzzleScene = nameof(PuzzleScene);

    [SerializeField] private GameSaveSystem _gameSaveSystem;
    private string _currentPuzzleId;

    public void GoToAnotherScene(Player player)
    {
        Sprite puzzleSprite = player.GetSprite();
        _currentPuzzleId = $"puzzle_{puzzleSprite.name}";

        YG2.saves.SetSprite(puzzleSprite);
        player.ResetSprite();

        // ��������� ID ����� ��� ������������ ���������� ����������
        PuzzleDataTransfer.CurrentPuzzleId = _currentPuzzleId;

        SceneLoader.Instance.LoadSceneWithSplash(PuzzleScene);
    }

    // ���������� ��� �������� �� ����� �����
    public void HandlePuzzleResult(float time)
    {
        if (string.IsNullOrEmpty(_currentPuzzleId)) return;

        // ��������� ������ ���������, ���� �����
        if (YG2.saves.PuzzleBestTimes.ContainsKey(_currentPuzzleId))
        {
            if (time < YG2.saves.PuzzleBestTimes[_currentPuzzleId])
            {
                YG2.saves.PuzzleBestTimes[_currentPuzzleId] = time;
            }
        }
        else
        {
            YG2.saves.PuzzleBestTimes.Add(_currentPuzzleId, time);
        }

        // ���������� ID
        _currentPuzzleId = null;
    }
}