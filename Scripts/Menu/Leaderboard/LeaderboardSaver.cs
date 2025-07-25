using System.Collections.Generic;
using UnityEngine;
using YG;
using static YG.YG2;

public class LeaderboardSaver : MonoBehaviour
{
    private Dictionary<Sprite, LeaderboardYG> _activeLeaderboards;

    private void Awake()
    {
        _activeLeaderboards = new();
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