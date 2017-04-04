﻿using Microsoft.Xna.Framework.Input;

namespace SuperPong.Input
{
	public class PrimaryKeyboardInputMethod : InputMethod
	{
		public override void Update(float dt)
		{
			KeyboardState currentState = Keyboard.GetState();

			_snapshot._buttons = 0;
			if (currentState.IsKeyDown(Keys.W))
			{
				_snapshot._buttons |= (byte)Buttons.Up;
			}
			if (currentState.IsKeyDown(Keys.S))
			{
				_snapshot._buttons |= (byte)Buttons.Down;
			}
		}
	}
}