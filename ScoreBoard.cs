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

        #region Timer
        private float scoreTimer;
        public float ScoreTimer { get { return scoreTimer; } set { scoreTimer = value; } }
        public float TotalSecondsElapsed;
        #endregion

        #region Init & Current Score
        private int initialScore; // Starting Score for Level/Challenge
        private int currentScore;
        public int InitialScore { get { return initialScore; } } // Could use a Setter for each new level.
        public int CurrentScore { get { return currentScore; } set { currentScore += (int)NetScorePerSecond; } }
        #endregion

        #region Cost
        private int additiveScore; //Total Gain from all in-Game sources (Things should be in 'Gain-per-Minute' form)
        private int subtractiveScore; //Total Cost of all in-Game sources (Things should be in 'Cost-per-Minute' form)
        private float scoreGainedPerSec;
        private float scoreLostPerSec;
        public int AdditiveScore { get { return additiveScore; } set { additiveScore = value; } }  
        public float ScoreGainedPerSec { get { return (float)additiveScore / 60f; } set { scoreGainedPerSec = (float)additiveScore / 60f; } }
        public int SubtractiveScore { get { return subtractiveScore; } set { subtractiveScore = value; } }
        public float ScoreLostPerSec { get { return (float)subtractiveScore / 60f; } set { scoreLostPerSec = (float)subtractiveScore / 60f; } }
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
        public int HighestScoreAchieved { get { return highestScoreAchieved; } set { if (currentScore >= highestScoreAchieved) { highestScoreAchieved = currentScore} } }
        #endregion


        public void ScoreBoardMethod(float deltaTime) {


            ScoreTimer += deltaTime;
            if (ScoreTimer >= 1f) {
                ScoreTimer = 0f;
                CurrentScore++;
                HighestScoreAchieved++;
                ScoreGainedPerSec++;
                ScoreLostPerSec++;
                TotalScoreAccumulated++;
                TotalScoreLost++;
                TotalSecondsElapsed++;
            }

            AverageScorePerSecond = (CurrentScore - InitialScore) / TotalSecondsElapsed;
            NetScorePerSecond = ScoreGainedPerSec - ScoreLostPerSec;




        }


    }
}