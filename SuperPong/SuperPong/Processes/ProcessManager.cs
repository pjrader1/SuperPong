﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
	public class ProcessManager
	{
		List<Process> _processList = new List<Process>();

		public void Attach(Process process)
		{
			// Commands are special, since they can be ran in 0 ticks
			if (process is Command)
			{
				// Doesn't use time, so we can pass null
				process.Update(null);

				// Attach the next process, if there is one
				if (process.Next != null)
				{
					Attach(process.Next);
					process.SetNext(null);
				}

				return;
			}

			_processList.Add(process);
			process.SetActive(true);
		}

		void Detatch(Process process)
		{
			_processList.Remove(process);
		}

		public bool HasProcesses()
		{
			return _processList.Count > 0;
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < _processList.Count; i++)
			{
				Process curr = _processList[i];

				if (curr.IsDead())
				{
					if (curr.Next != null)
					{
						Attach(curr.Next);
						curr.SetNext(null);
					}
					Detatch(curr);
					i--;
					continue;
				}
				if (curr.IsActive() && !curr.IsPaused())
				{
					curr.Update(gameTime);
				}
			}
		}
	}
}