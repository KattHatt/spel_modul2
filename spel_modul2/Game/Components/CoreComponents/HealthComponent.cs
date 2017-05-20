﻿using GameEngine.Components;
using System;

namespace Game.Components
{
    public class HealthComponent : IComponent
    {
        public bool IsAlive { get; set; } = true;
        public int Max { get; set; }
        public int Current { get; set; }
        public float DeathTimer { get; set; }

        public HealthComponent(int maxHealth)
        {
            Max = maxHealth;
            Current = Max;
            DeathTimer = 20.0f;
        }

        public HealthComponent(int maxHealth, float deathTimer)
        {
            Max = maxHealth;
            Current = Max;
            DeathTimer = deathTimer;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}