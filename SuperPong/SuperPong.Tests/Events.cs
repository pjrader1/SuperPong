﻿using System;
using Events;
using Events.Exceptions;
using NUnit.Framework;

namespace SuperPong.Tests
{
	[TestFixture]
	public class Events
	{
		[Test]
		public void ValidateExceptions()
		{
			Assert.Catch(typeof(TypeNotEventException), () =>
			{
				EventManager.Instance.RegisterListener(typeof(Generic1), new Listener1());
			});
			Assert.Catch(typeof(TypeNotEventException), () =>
			{
				EventManager.Instance.UnregisterListener(typeof(Generic1), new Listener1());
			});

			Assert.DoesNotThrow(() =>
			{
				EventManager.Instance.TriggerEvent(new Event1());
			});

			Listener1 listener = new Listener1();
			Assert.DoesNotThrow(() =>
			{
				EventManager.Instance.RegisterListener(typeof(Event1), listener);
			});
			Assert.DoesNotThrow(() =>
			{
				EventManager.Instance.UnregisterListener(typeof(Event1), listener);
			});

			EventManager.Instance.RegisterListener(typeof(Event1), listener);
			Assert.Throws(typeof(ListenerAlreadyExistsException), () =>
			{
				EventManager.Instance.RegisterListener(typeof(Event1), listener);
			});
		}

		[Test]
		public void TriggerEvent()
		{
			Listener1 listener1 = new Listener1();
			Listener2 listener2 = new Listener2();

			EventManager.Instance.RegisterListener<Event1>(listener1);

			Assert.False(EventManager.Instance.TriggerEvent(new Event1()));

			EventManager.Instance.RegisterListener<Event1>(listener2);
			Assert.True(EventManager.Instance.TriggerEvent(new Event1()));

			EventManager.Instance.UnregisterListener<Event1>(listener2);
			Assert.False(EventManager.Instance.TriggerEvent(new Event1()));

			EventManager.Instance.RegisterListener<Event1>(listener2);
			EventManager.Instance.UnregisterListener(listener2);
			Assert.False(EventManager.Instance.TriggerEvent(new Event1()));

			Assert.That(() =>
			{
				bool eventPropogated = false;

				EventManager.Instance.RegisterListener<Event1>(new PassListener((bool pass) =>
				{
					eventPropogated = true;
				}));
				EventManager.Instance.TriggerEvent(new Event1());

				return eventPropogated;
			});
		}
	}

	class Generic1
	{
	}

	class Event1 : IEvent
	{
	}

	class Listener1 : IEventListener
	{
		public bool Handle(IEvent evt)
		{
			return false;
		}
	}
	class Listener2 : IEventListener
	{
		public bool Handle(IEvent evt)
		{
			return true;
		}
	}

	class PassListener : IEventListener
	{
		Action<bool> _func;
		public PassListener(Action<bool> func)
		{
			_func = func;
		}

		public bool Handle(IEvent evt)
		{
			if (evt is Event1)
			{
				_func.Invoke(true);
				return true;
			}
			_func.Invoke(false);
			return false;
		}
	}
}