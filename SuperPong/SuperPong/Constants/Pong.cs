﻿/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

namespace SuperPong.Constants
{
    public class Pong
    {
        public static readonly byte RENDER_GROUP = Render.GROUP_TWO;

        public static readonly Vector2 BUFFER_RENDER_POSITION = new Vector2(0, 25);

        public static readonly float PLAYFIELD_HEIGHT = 450;
        public static readonly float PLAYFIELD_WIDTH = PLAYFIELD_HEIGHT * 1.5f;

        public static readonly float GOAL_WIDTH = 3f;
        public static readonly float GOAL_HEIGHT = PLAYFIELD_HEIGHT;

        public static readonly float EDGE_WIDTH = PLAYFIELD_WIDTH;
        public static readonly float EDGE_HEIGHT = 3f;

        public static readonly float FIELD_BACKGROUND_TEXTURE_ASPECT_RATIO = 0.374376039933f;
        public static readonly float FIELD_BACKGROUND_HEIGHT = PLAYFIELD_HEIGHT;
        public static readonly float FIELD_BACKGROUND_WIDTH = FIELD_BACKGROUND_HEIGHT * FIELD_BACKGROUND_TEXTURE_ASPECT_RATIO;

        public static readonly int LIVES_LEFT_POSITION_X = -250;
        public static readonly int LIVES_RIGHT_POSITION_X = 215;
        public static readonly int LIVES_ICON_LEFT_POSITION_X = LIVES_LEFT_POSITION_X + 35;
        public static readonly int LIVES_ICON_RIGHT_POSITION_X = LIVES_RIGHT_POSITION_X + 35;
        public static readonly int LIVES_POSITION_Y = 265;
        public static readonly int LIVES_COUNT = 5;

        public static readonly float PADDLE_WIDTH = 10;
        public static readonly float PADDLE_HEIGHT = 60;
        public static readonly float PADDLE_SPEED = 400;
        public static readonly float PADDLE_STARTING_X = PLAYFIELD_WIDTH / 2 - 2.5f * PADDLE_WIDTH;
        public static readonly float PADDLE_STARTING_Y = 0;
        public static readonly float PADDLE_BOUNCE_MAX = 45;
        public static readonly float PADDLE_BOUNCE_MIN = -PADDLE_BOUNCE_MAX;

        public static readonly float BALL_WIDTH = 15;
        public static readonly float BALL_HEIGHT = 15;
        public static readonly float BALL_PLAYER1_STARTING_ROTATION_DEGREES = 135;
        public static readonly float BALL_PLAYER2_STARTING_ROTATION_DEGREES = 45;
        public static readonly float BALL_STARTING_VELOCITY = 350.0f;
        public static readonly float BALL_MAX_TRAVEL_ANGLE_DEGREES = 60.0f;
        public static readonly float BALL_SPEED_INCREASE = 5.0f;

        public static readonly string INTRO_READY_CONTENT = "Ready?";
        public static readonly string INTRO_GO_CONTENT = "Go!";

        public static readonly string GAME_OVER_CONTENT_SUFFIX = " Wins!";

        public static readonly string PAUSED_CONTENT = "Paused";
    }
}
