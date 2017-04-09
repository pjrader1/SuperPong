﻿using System;
using Events;
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Events;

namespace SuperPong.Systems
{
	public class BallMovementSystem : EntitySystem
	{
		Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _ballEntities;

		Family _paddleFamily = Family.All(typeof(PaddleComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _paddleEntities;

		Family _edgeFamily = Family.All(typeof(EdgeComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _edgeEntities;

		public BallMovementSystem(Engine engine) : base(engine)
		{
			_ballEntities = engine.GetEntitiesFor(_ballFamily);
			_paddleEntities = engine.GetEntitiesFor(_paddleFamily);
			_edgeEntities = engine.GetEntitiesFor(_edgeFamily);
		}

		public override void Update(float dt)
		{
			foreach (Entity ballEntity in _ballEntities)
			{
				processCollision(ballEntity);

				processMovement(ballEntity, dt);
			}
		}

		void processCollision(Entity ballEntity)
		{
			TransformComponent transformComp = ballEntity.GetComponent<TransformComponent>();
			BallComponent ballComp = ballEntity.GetComponent<BallComponent>();

			// Normalize ball angle
			while (ballComp.Direction < 0)
			{
				ballComp.Direction += 2 * MathHelper.Pi;
			}
			while (ballComp.Direction > 2 * MathHelper.Pi)
			{
				ballComp.Direction -= 2 * MathHelper.Pi;
			}

			BoundingRect ballAABB = new BoundingRect(transformComp.position.X - ballComp.Width / 2,
			                                        transformComp.position.Y - ballComp.Height / 2,
			                                        ballComp.Width,
			                                         ballComp.Height);
			// Paddles
			foreach (Entity paddleEntity in _paddleEntities)
			{
				PaddleComponent paddleComp = paddleEntity.GetComponent<PaddleComponent>();
				TransformComponent paddleTransformComp = paddleEntity.GetComponent<TransformComponent>();

				BoundingRect paddleAABB = new BoundingRect(paddleTransformComp.position.X - paddleComp.Width / 2,
				                                           paddleTransformComp.position.Y - paddleComp.Height / 2,
				                                           paddleComp.Width,
				                                           paddleComp.Height);

				if (ballAABB.Intersects(paddleAABB))
				{
					if (!paddleComp.IgnoreCollisions)
					{
						{
							Vector2 ballEdge = transformComp.position
							                                + new Vector2(ballComp.Width, ballComp.Height) * -paddleComp.Normal;
							Vector2 paddleEdge = paddleTransformComp.position
							                                        + new Vector2(paddleComp.Width, paddleComp.Height) * paddleComp.Normal;

							Vector2 bouncePosition = (ballEdge + paddleEdge) / 2;
							EventManager.Instance.TriggerEvent(new BallBounceEvent(ballEntity, paddleEntity, bouncePosition));
						}

						// Determine directional vector of ball
						Vector2 ballDirection = new Vector2((float)Math.Cos(ballComp.Direction),
															(float)Math.Sin(ballComp.Direction));

						// Determine reflection vector of ball
						Vector2 ballReflectionDir = getReflectionVector(ballDirection, paddleComp.Normal);

						// Set angle of new directional vector
						ballComp.Direction = (float)Math.Atan2(ballReflectionDir.Y,
																ballReflectionDir.X);

						// Make sure ball does not get stuck inside paddle
						paddleComp.IgnoreCollisions = true;
					}
				}
				else
				{
					paddleComp.IgnoreCollisions = false;
				}
			}
			// Edges
			foreach (Entity edgeEntity in _edgeEntities)
			{
				EdgeComponent edgeComp = edgeEntity.GetComponent<EdgeComponent>();
				TransformComponent edgeTransformComp = edgeEntity.GetComponent<TransformComponent>();

				BoundingRect edgeAABB = new BoundingRect(edgeTransformComp.position.X - Constants.Pong.PLAYFIELD_WIDTH / 2,
														 edgeTransformComp.position.Y - Constants.Pong.EDGE_HEIGHT / 2,
														 Constants.Pong.PLAYFIELD_WIDTH,
														 Constants.Pong.EDGE_HEIGHT);

				if (ballAABB.Intersects(edgeAABB))
				{
					{
						Vector2 ballEdge = transformComp.position
						                                + new Vector2(ballComp.Width, ballComp.Height) * -edgeComp.Normal;
						Vector2 paddleEdge = edgeTransformComp.position
						                                      + new Vector2(Constants.Pong.PLAYFIELD_WIDTH,
						                                                    Constants.Pong.EDGE_HEIGHT)
						                                      * edgeComp.Normal;

						Vector2 bouncePosition = (ballEdge + paddleEdge) / 2;
						EventManager.Instance.TriggerEvent(new BallBounceEvent(ballEntity, edgeEntity, bouncePosition));
					}

					// Determine directional vector of ball
					Vector2 ballDirection = new Vector2((float)Math.Cos(ballComp.Direction),
														(float)Math.Sin(ballComp.Direction));

					// Determine reflection vector of ball
					Vector2 ballReflectionDir = getReflectionVector(ballDirection, edgeComp.Normal);

					// Set angle of new directional vector
					ballComp.Direction = (float)Math.Atan2(ballReflectionDir.Y,
															ballReflectionDir.X);
				}
			}
		}

		void processMovement(Entity ballEntity, float dt)
		{
			TransformComponent transformComp = ballEntity.GetComponent<TransformComponent>();
			BallComponent ballComp = ballEntity.GetComponent<BallComponent>();

			transformComp.position.X += (float)Math.Cos(ballComp.Direction) * ballComp.Velocity * dt;
			transformComp.position.Y += (float)Math.Sin(ballComp.Direction) * ballComp.Velocity * dt;
		}

		Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
		{
			return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
		}

	}
}
