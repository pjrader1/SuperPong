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

using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SuperPong.Components;

namespace SuperPong.Systems
{
    public class RenderSystem
    {
        readonly Engine _engine;

        public static readonly Vector2 FlipY = new Vector2(1, -1);
        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

        Family _spriteFamily = Family.All(typeof(SpriteComponent), typeof(TransformComponent)).Get();
        Family _fontFamily = Family.All(typeof(FontComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _spriteEntities;
        ImmutableList<Entity> _fontEntities;

        SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get
            {
                return _spriteBatch;
            }
        }

        public RenderSystem(GraphicsDevice graphics, Engine engine)
        {
            _engine = engine;
            _spriteEntities = engine.GetEntitiesFor(_spriteFamily);
            _fontEntities = engine.GetEntitiesFor(_fontFamily);

            _spriteBatch = new SpriteBatch(graphics);
        }

        public void DrawEntities(float dt, float betweenFrameAlpha)
        {
            DrawEntities(Constants.Render.GROUP_MASK_ALL, dt, betweenFrameAlpha);
        }

        public void DrawEntities(byte groupMask, float dt, float betweenFrameAlpha)
        {
            DrawEntities(Matrix.Identity, groupMask, dt, betweenFrameAlpha);
        }

        public void DrawEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred,
                               null,
                               SamplerState.PointClamp,
                               null,
                               null,
                               null,
                               transformMatrix);

            foreach (Entity entity in _spriteEntities)
            {
                SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
                if (spriteComp.Hidden
                    || (spriteComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 offsetPosition = (transformComp.Position - transformComp.LastPosition) * (1 - betweenFrameAlpha);
                offsetPosition *= -1;

                Vector2 scale = new Vector2(spriteComp.Bounds.X / spriteComp.Texture.Width,
                                            spriteComp.Bounds.Y / spriteComp.Texture.Height);
                Vector2 origin = new Vector2(spriteComp.Texture.Bounds.Width,
                                             spriteComp.Texture.Bounds.Height) * HalfHalf;

                _spriteBatch.Draw(spriteComp.Texture,
                                  (transformComp.Position + offsetPosition) * FlipY,
                                  null,
                                  Color.White,
                                  transformComp.Rotation,
                                  origin,
                                  scale,
                                  SpriteEffects.None,
                                  0);
            }

            foreach (Entity entity in _fontEntities)
            {
                FontComponent fontComp = entity.GetComponent<FontComponent>();
                if (fontComp.Hidden
                    || (fontComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 scale = Vector2.One;
                Vector2 origin = fontComp.Font.MeasureString(fontComp.Content) / 2;

                _spriteBatch.DrawString(fontComp.Font,
                                        fontComp.Content,
                                        transformComp.Position * FlipY,
                                        fontComp.Color,
                                        transformComp.Rotation,
                                        origin,
                                        scale,
                                        SpriteEffects.None,
                                        0);
            }

            _spriteBatch.End();
        }
    }
}
