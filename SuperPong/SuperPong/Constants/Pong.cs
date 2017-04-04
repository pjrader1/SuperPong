﻿using System;
namespace SuperPong.Constants
{
	public class Pong
	{
		public static readonly float PLAYFIELD_WIDTH = 480;
		public static readonly float PLAYFIELD_HEIGHT = 300;

		public static readonly float EDGE_HEIGHT = 3f;

		public static readonly float PADDLE_WIDTH = 10;
		public static readonly float PADDLE_HEIGHT = 60;
		public static readonly float PADDLE_SPEED = 240;
		public static readonly float PADDLE_STARTING_X = PLAYFIELD_WIDTH / 2 - 2 * PADDLE_WIDTH;
		public static readonly float PADDLE_STARTING_Y = 0;

		public static readonly float BALL_WIDTH = 15;
		public static readonly float BALL_HEIGHT = 15;
		public static readonly float BALL_STARTING_ROTATION_DEGREES = 45;
		public static readonly float BALL_STARTING_VELOCITY = 200.0f;
		public static readonly float BALL_MAX_TRAVEL_ANGLE_DEGREES = 60.0f;

	}
}