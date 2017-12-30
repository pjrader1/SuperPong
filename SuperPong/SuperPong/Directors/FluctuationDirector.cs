﻿using System;
using Events;
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Events;
using SuperPong.Fluctuations;
using SuperPong.Processes;

namespace SuperPong.Directors
{
    public class FluctuationDirector : BaseDirector, IEventListener
    {
        readonly MTRandom _random;

        readonly Timer _fluctuationTimer = new Timer(0);
        int _fluctuationUnlockedLevel = 1;
        Type _lastFluctuation = null;

        public FluctuationDirector(IPongDirectorOwner owner) : base(owner)
        {
            _random = new MTRandom();
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<BallServeEvent>(this);
            EventManager.Instance.RegisterListener<FluctuationEndEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is BallServeEvent)
            {
                HandleBallServe(evt as BallServeEvent);
            }

            if (evt is FluctuationEndEvent)
            {
                HandleFluctuationEnd(evt as FluctuationEndEvent);
            }

            return false;
        }

        void AttachFluctuationSequence()
        {
            int availableFluctuationsCount = 0;
            for (int i = 0; i < _fluctuationUnlockedLevel; i++)
            {
                availableFluctuationsCount += Constants.Fluctuations.FLUCTUATIONS[i].Length;
            }

            Type[] fluctuations = new Type[availableFluctuationsCount];
            int k = 0;
            for (int i = 0; i < _fluctuationUnlockedLevel; i++)
            {
                for (int j = 0; j < Constants.Fluctuations.FLUCTUATIONS[i].Length; j++)
                {
                    fluctuations[k++] = Constants.Fluctuations.FLUCTUATIONS[i][j];
                }
            }

            Type fluctuationType = null;
            do
            {
                int index = _random.Next(0, fluctuations.Length - 1);
                fluctuationType = fluctuations[index];
            } while (fluctuationType == _lastFluctuation);

            WaitProcessKillOnEvent timerProcess = new WaitProcessKillOnEvent(_random.NextSingle() * (Constants.Fluctuations.TIMER_MAX - Constants.Fluctuations.TIMER_MIN) + Constants.Fluctuations.TIMER_MIN,
                                                                            typeof(GoalEvent));

            timerProcess.SetNext((Fluctuation)Activator.CreateInstance(fluctuationType, _owner));
            _processManager.Attach(timerProcess);
            _lastFluctuation = fluctuationType;

            _fluctuationUnlockedLevel = MathHelper.Min(++_fluctuationUnlockedLevel, Constants.Fluctuations.FLUCTUATIONS.Length);
        }

        // HANDLERS
        void HandleBallServe(BallServeEvent ballServeEvent)
        {
            _fluctuationUnlockedLevel = 1;
            AttachFluctuationSequence();
        }

        void HandleFluctuationEnd(FluctuationEndEvent fluctuationEndEvent)
        {
            if (fluctuationEndEvent.KillReason == Fluctuation.KillReason.NORMAL)
            {
                AttachFluctuationSequence();
            }
        }
    }
}
