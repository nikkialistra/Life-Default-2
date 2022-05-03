﻿using Units.Enums;
using Units.MinimaxFightBehavior.FightAct;
using static Units.MinimaxFightBehavior.FightAct.Fight.FightState;

namespace Units.MinimaxFightBehavior
{
    public class MinimaxScoreFunction
    {
        public static float Calculate(Fight fight, Player player)
        {
            if (player.Fraction == Fraction.Colonists)
            {
                return CalculateForFirstPlayer(fight);
            }
            else
            {
                return CalculateForSecondPlayer(fight);
            }
        }

        private static float CalculateForFirstPlayer(Fight fight)
        {
            return fight.State switch
            {
                FirstPlayerVictory => 1f,
                SecondPlayerVictory => -1f,
                _ => 0f
            };
        }
        
        private static float CalculateForSecondPlayer(Fight fight)
        {
            return fight.State switch
            {
                FirstPlayerVictory => -1f,
                SecondPlayerVictory => 1f,
                _ => 0f
            };
        }
    }
}
