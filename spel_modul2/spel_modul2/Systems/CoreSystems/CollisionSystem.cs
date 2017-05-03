﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameEngine
{
    class CollisionSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {

        }

        public static List<int> DetectAreaCollision(Rectangle area)
        {
            var cm = ComponentManager.GetInstance();
            List<int> foundEntities = new List<int>();
            foreach (KeyValuePair<int, IComponent> entity in cm.GetComponentsOfType<CollisionComponent>())
            {
                CollisionComponent collisionComponent = (CollisionComponent)entity.Value;
                Point entity2Pos = cm.GetComponentForEntity<PositionComponent>(entity.Key).position;
                Point correctedPos = new Point(entity2Pos.X - (collisionComponent.collisionBox.Width / 2), entity2Pos.Y - (collisionComponent.collisionBox.Height / 2));
                collisionComponent.collisionBox.Location = correctedPos;
                if (area.Intersects(collisionComponent.collisionBox))
                {
                    //Collision detected, add them to the list
                    foundEntities.Add(entity.Key);
                }
            }
            return foundEntities;
        }


        //Detect if characters collide
        public bool DetectMovementCollision(int entity, Point position)
        {
            var cm = ComponentManager.GetInstance();
            CollisionComponent collisionComponent = cm.GetComponentForEntity<CollisionComponent>(entity);
            Rectangle rectToCheck = new Rectangle(new Point(position.X - (collisionComponent.collisionBox.Width / 2), position.Y - (collisionComponent.collisionBox.Height / 2)), collisionComponent.collisionBox.Size);

            foreach (KeyValuePair<int, IComponent> entity2 in cm.GetComponentsOfType<CollisionComponent>())
            {
                if (entity2.Key != entity)
                {
                    CollisionComponent collisionComponent2 = (CollisionComponent)entity2.Value;
                    Point entity2Pos = cm.GetComponentForEntity<PositionComponent>(entity2.Key).position;
                    Point correctedPos = new Point(entity2Pos.X - (collisionComponent2.collisionBox.Width / 2), entity2Pos.Y - (collisionComponent2.collisionBox.Height / 2));
                    collisionComponent2.collisionBox.Location = correctedPos;
                    if (rectToCheck.Intersects(collisionComponent2.collisionBox))
                    {
                        //Collision detected
                        return true;
                    }
                }
            }
            return false;
        }
    }
}