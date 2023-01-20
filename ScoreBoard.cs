using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth;

namespace ScoreBoardUtil
{
    public class ScoreBoardClass
    {

        public ScoreBoardClass()
        {
            currentScore = 0;
            initialScore = 0;
            additiveScore = 0;
            subtractiveScore = 0;
            scoreGainedPerSec = 0;
            scoreLostPerSec = 0;
            netScorePerSecond = 0;
            averageScorePerSecond = 0;
            totalScoreAccumulated = 0;
            totalScoreLost = 0;
            highestScoreAchieved = 0;
        }

        #region Timer
        private float scoreTimer;
        public float ScoreTimer { get { return scoreTimer; } set { scoreTimer = value; } }
        public float TotalSecondsElapsed;
        #endregion

        #region Init & Current Score
        private int initialScore; // Starting Score for Level/Challenge
        private int currentScore;
        public int InitialScore { get { return initialScore; } set { initialScore = value; } } // Could use a Setter for each new level.
        public int CurrentScore { get { return currentScore; } set { currentScore = value; } }
        #endregion

        #region Cost
        private int additiveScore; //Total Gain from all in-Game sources (Things should be in 'Gain-per-Minute' form)
        private int subtractiveScore; //Total Cost of all in-Game sources (Things should be in 'Cost-per-Minute' form)
        private float scoreGainedPerSec;
        private float scoreLostPerSec;
        public int AdditiveScore { get { return additiveScore; } set { additiveScore = value; } }
        public float ScoreGainedPerSec { get { return scoreGainedPerSec; } set { scoreGainedPerSec = value; } }
        public int SubtractiveScore { get { return subtractiveScore; } set { subtractiveScore = value; } }
        public float ScoreLostPerSec { get { return scoreLostPerSec; } set { scoreLostPerSec = value; } }
        #endregion

        #region ScorePerSecond
        private float netScorePerSecond;
        private float averageScorePerSecond;
        public float NetScorePerSecond { get { return netScorePerSecond; } set { netScorePerSecond = value; } }
        public float AverageScorePerSecond { get { return averageScorePerSecond; } set { averageScorePerSecond = value; } }
        #endregion

        #region Totals
        private int totalScoreAccumulated;
        private int totalScoreLost;
        private int highestScoreAchieved;
        public int TotalScoreAccumulated { get { return totalScoreAccumulated; } set { totalScoreAccumulated = value; } }
        public int TotalScoreLost { get { return totalScoreLost; } set { totalScoreLost = value; } }
        public int HighestScoreAchieved { get { return highestScoreAchieved; } set { if (currentScore >= highestScoreAchieved) { highestScoreAchieved = currentScore; } } }
        #endregion


        public void ScoreBoardMethod(float deltaTime)
        {
            ScoreTimer += deltaTime;
            if (ScoreTimer >= 1f)
            {
                ScoreTimer = 0f;
                CurrentScore += (int)NetScorePerSecond;
                HighestScoreAchieved = Math.Max(HighestScoreAchieved, CurrentScore);
                TotalScoreAccumulated += (int)ScoreGainedPerSec;
                TotalScoreLost += (int)ScoreLostPerSec;
                TotalSecondsElapsed++;
            }
            AverageScorePerSecond = (CurrentScore - InitialScore) / TotalSecondsElapsed;
        }

    }
}