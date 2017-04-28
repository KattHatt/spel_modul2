﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameEngine
{
    class AISystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
            updateAIMovements();
        }

        void updateAIMovements()
        {
            var cm = ComponentManager.GetInstance();

            foreach (KeyValuePair<int, IComponent> pair in cm.GetComponentsOfType<AIComponent>())
            {
                MoveComponent moveComp = ComponentManager.GetInstance().GetComponentForEntity<MoveComponent>(pair.Key);
                if (moveComp != null)
                {
                    PositionComponent posComp = ComponentManager.GetInstance().GetComponentForEntity<PositionComponent>(pair.Key);
                    if (posComp != null)
                    {
                        Vector2 nextMovement = new Vector2(((AIComponent)pair.Value).Destination.X - posComp.position.X, ((AIComponent)pair.Value).Destination.Y - posComp.position.Y);
                        float distance = (float)Math.Sqrt(nextMovement.X * nextMovement.X + nextMovement.Y * nextMovement.Y);
                        if (distance > 5)
                        {
                            Vector2 direction = new Vector2(nextMovement.X / distance, nextMovement.Y / distance);
                            moveComp.Velocity = direction;
                        }
                        else
                            moveComp.Velocity = new Vector2(0, 0);
                    }
                }
            }
        }
    }
}
