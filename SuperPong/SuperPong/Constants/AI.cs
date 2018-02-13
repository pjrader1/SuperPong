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

namespace SuperPong.Constants
{
    public class AI
    {
        public static readonly float ACTIVE_DISTANCE = Constants.Pong.PLAYFIELD_WIDTH / 2.5f;
        public static readonly float TARGET_SENSITIVITY = Constants.Pong.PADDLE_HEIGHT / 4;
    }
}
