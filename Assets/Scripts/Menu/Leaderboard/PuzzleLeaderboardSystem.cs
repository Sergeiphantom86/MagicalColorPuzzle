using System.Collections.Generic;
using UnityEngine;
using YG;
using static YG.YG2;

public class PuzzleLeaderboardSystem : MonoBehaviour
{
    public List<PuzzleLeaderboard> _leaderboards;
    public LeaderboardYG _leaderboardPrefab;
    public Transform _leaderboardParent;

    private Dictionary<Sprite, LeaderboardYG> _activeLeaderboards = new();

    void Start()
    {
        foreach (var lb in _leaderboards)
        {
            CreateLeaderboardForPuzzle(lb);
        }
    }

    void CreateLeaderboardForPuzzle(PuzzleLeaderboard config)
    {
        LeaderboardYG leaderboardYG = Instantiate(_leaderboardPrefab, _leaderboardParent);
        leaderboardYG.nameLB = config.technicalName;
        leaderboardYG.gameObject.SetActive(false);

        _activeLeaderboards.Add(config.puzzleSprite, leaderboardYG);
    }

    public void ShowLeaderboardForPuzzle(Sprite selectedPuzzle)
    {
        if (_activeLeaderboards.TryGetValue(selectedPuzzle, out LeaderboardYG leaderboardYG))
        {
            foreach (var board in _activeLeaderboards.Values)
            {
                board.gameObject.SetActive(false);
            }

            leaderboardYG.gameObject.SetActive(true);
            leaderboardYG.UpdateLB();
        }
    }

    public void ReportPuzzleTime(Sprite puzzle, float completionTime)
    {
        if (_activeLeaderboards.TryGetValue(puzzle, out LeaderboardYG lb))
        {
            var saves = YG2.saves;

            string puzzleKey = puzzle.name;

            if (saves.PuzzleBestTimes == null)
            {
                saves.PuzzleBestTimes = new Dictionary<string, float>();
            }

            bool hasRecord = saves.PuzzleBestTimes.TryGetValue(puzzleKey, out float currentBest);

            if (hasRecord == false || completionTime < currentBest)
            {
                saves.PuzzleBestTimes[puzzleKey] = completionTime;

                SaveProgress();

                SetLBTimeConvert(lb.nameLB, completionTime);

                Debug.Log($"Новый рекорд для {puzzleKey}: {completionTime} сек");
            }
            else
            {
                Debug.Log($"Текущий результат {completionTime} не побил рекорд {currentBest} для {puzzleKey}");
            }
        }
    }

    public float GetBestTimeForPuzzle(Sprite puzzle)
    {
        var saves = YG2.saves;
        string puzzleKey = puzzle.name;

        if (saves.PuzzleBestTimes != null &&
            saves.PuzzleBestTimes.TryGetValue(puzzleKey, out float bestTime))
        {
            return bestTime;
        }

        return float.MaxValue;
    }
}