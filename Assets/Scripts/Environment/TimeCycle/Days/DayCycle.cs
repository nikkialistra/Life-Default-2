﻿using Environment.TimeCycle.Seasons;
using Environment.TimeCycle.Ticking;
using UI.Game.GameLook.Components;
using UnityEngine;
using Zenject;

namespace Environment.TimeCycle.Days
{
    public class DayCycle : MonoBehaviour, ITicking
    {
        [Range(0, 23)]
        [SerializeField] private int _hours;
        
        private int _ticks;
        
        private SeasonCycle _seasonCycle;
        private TickingRegulator _tickingRegulator;
        private TimeWeatherView _timeWeatherView;

        [Inject]
        public void Construct(SeasonCycle seasonCycle, TickingRegulator tickingRegulator, TimeWeatherView timeWeatherView)
        {
            _seasonCycle = seasonCycle;
            _tickingRegulator = tickingRegulator;
            _timeWeatherView = timeWeatherView;
            
            _tickingRegulator.AddToTickables(this);
        }

        private void Start()
        {
            UpdateView();
        }

        public void Tick()
        {
            _ticks++;

            if (_ticks == 25)
            {
                _ticks = 0;
                UpdateHours();
            }
        }

        private void UpdateHours()
        {
            _hours++;

            if (_hours == 24)
            {
                _hours = 0;
                _seasonCycle.NextDay();
            }

            UpdateView();
        }

        private void UpdateView()
        {
            _timeWeatherView.ChangeHours(_hours);
        }
    }
}
