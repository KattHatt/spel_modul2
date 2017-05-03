﻿using Microsoft.Xna.Framework;

namespace GameEngine
{
    class LevelSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
        }

        public void Update(int entity, int entityKilled)
        {
            ComponentManager cm = ComponentManager.GetInstance();

            if(cm.GetComponentForEntity<PlayerComponent>(entity) != null)
                LevelCalculator(entity, CalculateKillExperience(entity, entityKilled));

            if(cm.GetComponentForEntity<PlayerComponent>(entityKilled) != null)
            {
                LevelComponent levelComponent = cm.GetComponentForEntity<LevelComponent>(entityKilled);
                int lvl = levelComponent.CurrentLevel;
                switch (lvl)
                {
                    case 1:  LevelCalculator(entityKilled, -45.0f);  break;
                    case 2:  LevelCalculator(entityKilled, -46.0f);  break;
                    case 3:  LevelCalculator(entityKilled, -61.0f);  break;
                    case 4:  LevelCalculator(entityKilled, -91.0f);  break;
                    case 5:  LevelCalculator(entityKilled, -183.0f); break;
                    case 6:  LevelCalculator(entityKilled, -183.0f); break;
                    case 7:  LevelCalculator(entityKilled, -185.0f); break;
                    case 8:  LevelCalculator(entityKilled, -367.0f); break;
                    case 9:  LevelCalculator(entityKilled, -400.0f); break;
                    case 10: LevelCalculator(entityKilled, -400.0f); break;
                }
            }
        }
        
        private float CalculateKillExperience(int entity, int entityKilled)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            LevelComponent entityToGainExpComponent = cm.GetComponentForEntity<LevelComponent>(entity);
            int entityKilledLvl = cm.GetComponentForEntity<LevelComponent>(entityKilled).CurrentLevel;
            int entitylvl = entityToGainExpComponent.CurrentLevel;
            float xpGained = 22.0f * entityKilledLvl - entitylvl;
            if (xpGained <= 0)
                xpGained = 2.0f;
            
            return xpGained;
        }

        private void LevelCalculator(int entity, float experience)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            LevelComponent levelComponent = cm.GetComponentForEntity<LevelComponent>(entity);
            StatsComponent statComponent = cm.GetComponentForEntity<StatsComponent>(entity);
            int currLevel = levelComponent.CurrentLevel;
            float currXp = levelComponent.Experience;

            levelComponent.Experience = currXp + experience;
            if(currXp + experience <= 0)
            {
                HealthComponent entityHealth = cm.GetComponentForEntity<HealthComponent>(entity);
                entityHealth.IsAlive = false;
                levelComponent.Experience = 0;
            }
            else if (currXp + experience >= 83.0f)
                levelComponent.CurrentLevel = 2;
            else if(currXp + experience >= 174.0f)
                levelComponent.CurrentLevel = 3;
            else if (currXp + experience >= 266.0f)
                levelComponent.CurrentLevel = 4;
            else if (currXp + experience >= 389.0f)
                levelComponent.CurrentLevel = 5;
            else if (currXp + experience >= 572.0f)
                levelComponent.CurrentLevel = 6;
            else if (currXp + experience >= 939.0f)
                levelComponent.CurrentLevel = 7;
            else if (currXp + experience >= 1306.0f)
                levelComponent.CurrentLevel = 8;
            else if (currXp + experience >= 1673.0f)
                levelComponent.CurrentLevel = 9;
            else if (currXp + experience >= 2407.0f)
                levelComponent.CurrentLevel = 10;

            //see if entity leveled up
            if(currLevel < levelComponent.CurrentLevel)
            {
                int num = levelComponent.CurrentLevel - currLevel;
                statComponent.StatPoints += num * 5;
             
            }
            //see if entity lost level
            if (currLevel > levelComponent.CurrentLevel)
            {
                int num = currLevel - levelComponent.CurrentLevel;
                statComponent.StatPoints -= num * 5;
            }
        }
    }
}
