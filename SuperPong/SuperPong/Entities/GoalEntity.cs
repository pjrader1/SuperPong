﻿using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
	public static class GoalEntity
	{
		public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Vector2 normal)
		{
			Entity entity = engine.CreateEntity();

			entity.AddComponent(new TransformComponent(position));
			entity.AddComponent(new SpriteComponent(texture, new Vector2(Constants.Pong.GOAL_WIDTH,
			                                                             Constants.Pong.PLAYFIELD_HEIGHT)));
			entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;
			entity.AddComponent(new GoalComponent(normal));

			return entity;
		}
	}
}