﻿using System;
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Directors;

namespace SuperPong.Fluctuations
{
    public class CameraRollFluctuation : Fluctuation
    {
        enum State
        {
            Rotating,
            Ending
        }
        State _state = State.Rotating;

        readonly PongCamera _camera;
        float _elapsedTime;
        float _exitTime;

        public CameraRollFluctuation(IPongDirectorOwner owner) : base(owner)
        {
            _camera = owner.PongCamera;
        }

        protected override void OnKill()
        {
            _camera.Rotation = Quaternion.Identity;

            base.OnKill();
        }

        public override void SoftEnd()
        {
            _state = State.Ending;
        }

        protected override void OnTogglePause()
        {

        }

        protected override void OnUpdate(float dt)
        {
            switch (_state)
            {
                case State.Rotating:
                    {
                        _elapsedTime += dt;

                        float rot = _elapsedTime
                            * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                            * MathHelper.TwoPi;
                        _camera.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                                                                          rot);
                    }
                    break;
                case State.Ending:
                    {
                        _exitTime += dt;

                        float rot = _elapsedTime
                            * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                            * MathHelper.TwoPi;

                        while (rot >= MathHelper.TwoPi)
                        {
                            rot -= MathHelper.TwoPi;
                        }

                        float alpha = _exitTime / Constants.Fluctuations.CAMERA_ROLL_EXIT_TIME;
                        float crot = rot * (1 - Easings.QuinticEaseInOut(alpha));
                        _camera.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                                                                          crot);
                    }
                    break;
            }

            if (_elapsedTime >= Constants.Fluctuations.CAMERA_ROLL_STEADY_TIME
               || _exitTime >= Constants.Fluctuations.CAMERA_ROLL_EXIT_TIME)
            {
                Kill();
            }
        }
    }
}