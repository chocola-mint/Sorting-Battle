using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.Core
{
    /// <summary>
    /// A class that manages a player's score.
    /// </summary>
    public class GameScoreState
    {
        public int totalScore { get; private set; } = 0;
        public int combo { get; private set; } = 0;
        public int effectiveCombo => Mathf.Min(combo, maxEffectiveCombo);
        public int comboScoreBuffer { get; private set; } = 0;
        public struct ScoreIncreaseInfo
        {
            public int increase;
            public int removeCount;
            public int combo;
        }
        public event System.Action<int> onComboReset;
        public event System.Action<ScoreIncreaseInfo> onScoreIncrease;
        public struct Config
        {
            public int minimumRemoveCount, baseRemoveScore, maxEffectiveCombo, removeLengthBonus;
            public float comboScoreStep;
        }
        private int minimumRemoveCount, baseRemoveScore, maxEffectiveCombo, removeLengthBonus;
        private float comboScoreStep;
        public GameScoreState(Config config)
        {
            this.minimumRemoveCount = config.minimumRemoveCount;
            this.baseRemoveScore = config.baseRemoveScore;
            this.maxEffectiveCombo = config.maxEffectiveCombo;
            this.removeLengthBonus = config.removeLengthBonus;
            this.comboScoreStep = config.comboScoreStep;
        }
        public void OnRemove(int removeCount)
        {
            if(removeCount < minimumRemoveCount)
            {
                // Failed remove. Maybe reset combo?
                ResetCombo();
            }
            else
            {
                // Successful remove.
                RegisterCombo(removeCount);
            }
        }
        private void RegisterCombo(int removeCount)
        {
            ++combo;
            int plusScore = Mathf.CeilToInt(effectiveCombo / comboScoreStep) * (baseRemoveScore + (removeCount - minimumRemoveCount) * removeLengthBonus);
            totalScore += plusScore;
            comboScoreBuffer += plusScore;
            onScoreIncrease?.Invoke(new(){
                increase = plusScore,
                removeCount = removeCount,
                combo = combo,
                });
        }

        public void ResetCombo()
        {
            combo = 0;
            onComboReset?.Invoke(comboScoreBuffer);
            comboScoreBuffer = 0;
        }
    }
}
