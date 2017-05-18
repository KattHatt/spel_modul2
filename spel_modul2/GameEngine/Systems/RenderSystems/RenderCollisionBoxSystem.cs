﻿using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameEngine.Systems
{
    class RenderCollisionBoxSystem : ISystem, IRenderSystem
    {
        void ISystem.Update(GameTime gameTime) {}

        public void Render(RenderHelper rh)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            Viewport viewport = Extensions.GetCurrentViewport(rh.graphicsDevice);

            Dictionary<int, IComponent> entities = cm.GetComponentsOfType<CollisionComponent>();
            foreach (var entity in entities.Keys)
            {
                CollisionComponent collisionComponent;
                PositionComponent positionComponent;

                if (cm.GetComponentsForEntity(entity, out collisionComponent, out positionComponent))
                {
                    Rectangle bb = collisionComponent.collisionBox;
                    bb.Offset(-bb.Width / 2, -bb.Height / 2);
                    bb.Offset(positionComponent.position);
                    rh.DrawRectangle(bb.WorldToScreen(ref viewport), 2, Color.Yellow, RenderLayer.Foreground1);
                }
            }
        }
    }
}
