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

using System.Collections.Generic;

namespace ECS
{
    public class Engine
    {
        readonly List<Entity> _entities = new List<Entity>();
        readonly ImmutableList<Entity> _immutableEntities;
        readonly Dictionary<Family, List<Entity>> _familyBags = new Dictionary<Family, List<Entity>>();
        readonly Dictionary<Family, ImmutableList<Entity>> _immutableFamilyBags = new Dictionary<Family, ImmutableList<Entity>>();

        public Engine()
        {
            _immutableEntities = new ImmutableList<Entity>(_entities);
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity(this);
            _entities.Add(entity);

            // We don't need to update bags, the entity does not have any components

            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            _entities.Remove(entity);
            UpdateFamilyBags(entity);
        }

        public ImmutableList<Entity> GetEntities()
        {
            return _immutableEntities;
        }

        public ImmutableList<Entity> GetEntitiesFor(Family family)
        {
            if (!_familyBags.ContainsKey(family))
            {
                InitFamilyBag(family);
            }

            return _immutableFamilyBags[family];
        }

        void InitFamilyBag(Family family)
        {
            List<Entity> bag = new List<Entity>();
            _familyBags.Add(family, bag);
            _immutableFamilyBags.Add(family, new ImmutableList<Entity>(_familyBags[family]));

            foreach (Entity entity in _entities)
            {
                if (family.Matches(entity))
                {
                    bag.Add(entity);
                }
            }
        }

        internal void UpdateFamilyBags(Entity entity)
        {
            foreach (Family family in _familyBags.Keys)
            {
                UpdateFamilyBag(family, entity);
            }
        }

        void UpdateFamilyBag(Family family, Entity entity)
        {
            List<Entity> bag = _familyBags[family];
            if (_entities.Contains(entity)) // Addition/update
            {
                if (family.Matches(entity) && !bag.Contains(entity))
                {
                    bag.Add(entity);
                }
                if (!family.Matches(entity) && bag.Contains(entity))
                {
                    bag.Remove(entity);
                }
            }
            else // Removal
            {
                bag.Remove(entity);
            }
        }

    }
}
