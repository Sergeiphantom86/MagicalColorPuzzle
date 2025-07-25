using UnityEngine;
using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        public SavesYG()
        {
            MusicVolume = 0.8f;
            SoundVolume = 0.8f;
            CountStars = 0;
            IsMusicEnabled = true;
            NpcStates = new Dictionary<string, int>();
            PuzzleBestTimes = new Dictionary<string, float>();
            ActiveLeaderboards = new Dictionary<Sprite, LeaderboardYG> { };
        }

        public Dictionary<string, int> NpcStates;
        public Dictionary<string, float> PuzzleBestTimes;
        public Dictionary<Sprite, LeaderboardYG> ActiveLeaderboards;

        public int PuzzleKeys
        {
            get => NpcStates.TryGetValue("puzzle_keys", out int value) ? value : 0;
            set => NpcStates["puzzle_keys"] = value;
        }

        public float MusicVolume;
        public float SoundVolume;
        public bool IsMusicEnabled;
        public bool Initialized = false;
        public int CountStars;
        public int QuestID;

        private Sprite _sprite;
        public Sprite Sprite => _sprite;

        internal void SetSprite(Sprite sprite)
        {
            if (sprite == null) return;
            if (_sprite != null) return;

            _sprite = sprite;
        }

        public void ResetSprite()
        {
            _sprite = null;
        }

        public void SetVolume(string name, float volume)
        {
            if (nameof(MusicVolume) == name)
            {
                MusicVolume = volume;
            }
            else
            {
                SoundVolume = volume;
            }
        }
    }
}