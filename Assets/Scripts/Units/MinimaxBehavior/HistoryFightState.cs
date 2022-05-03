﻿using Units.Enums;

namespace Units.MinimaxBehavior
{
    public class HistoryFightState
    {
        public HistoryFightState(Fight.FightState state, Fraction whichMove, FightMove fightMove)
        {
            State = state;
            WhichMove = whichMove;
            FightMove = fightMove;
        }
        
        public Fight.FightState State { get; }
        public Fraction WhichMove { get; }
        
        public FightMove FightMove { get; }
        public FightMove ReverseFightMove => FightMove.GetReverseState(FightMove);
    }
}
