﻿using Game.Components;
using GameEngine.Managers;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game.Systems
{
    public class LevelSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            foreach (var entity in cm.GetComponentsOfType<LevelComponent>())
            {
                LevelComponent levelComponent = (LevelComponent)entity.Value;
                if (cm.HasEntityComponent<StatsComponent>(entity.Key))
                {
                    StatsComponent statComponent = cm.GetComponentForEntity<StatsComponent>(entity.Key);
                    //See if there is any experience to gain
                    if (levelComponent.LevelLoss)
                    {
                        levelComponent.TotalExperience = 0;
                        levelComponent.CurrentLevel--;
                        levelComponent.LevelLoss = false;
                        statComponent.RemoveStats += 3;
                    }

                    if (levelComponent.ExperienceGains.Count > 0)
                    {
                        foreach (int entityKilled in levelComponent.ExperienceGains)
                        {
                            int xpGained = CalculateKillExperience(entity.Key, entityKilled);
                            levelComponent.TotalExperience += xpGained;
                            int oldLevel = levelComponent.CurrentLevel;
                            int newLevel = LevelCalculator(levelComponent.TotalExperience);
                            levelComponent.CurrentLevel = newLevel;
                            

                            // See if entity leveled up
                            if (oldLevel < newLevel)
                            {
                                int num = newLevel - oldLevel;
                                statComponent.SpendableStats += 3 * num;
                            }
                            levelComponent.Experience = levelComponent.TotalExperience - ExperienceCalculator(newLevel - 1);
                        }
                        levelComponent.ExperienceGains = new List<int>();
                    }
                }
            }
        }

        private int CalculateKillExperience(int entity, int entityKilled)
        {
            ComponentManager cm = ComponentManager.GetInstance();
            LevelComponent entityToGainExpComponent = cm.GetComponentForEntity<LevelComponent>(entity);
            int entityKilledLvl = cm.GetComponentForEntity<LevelComponent>(entityKilled).CurrentLevel;
            int entitylvl = entityToGainExpComponent.CurrentLevel;
            int xpGained = 4 * (entityKilledLvl);
            if (xpGained <= 0)
                xpGained = 0;

            return xpGained;
        }

        private int LevelCalculator(int experience)
        {
            if (experience <= 0) return 0;
            else if (experience <= 83) return 1;
            else if (experience <= 174) return 2;
            else if (experience <= 266) return 3;
            else if (experience <= 389) return 4;
            else if (experience <= 572) return 5;
            else if (experience <= 939) return 6;
            else if (experience <= 1306) return 7;
            else if (experience <= 1673) return 8;
            else if (experience <= 2407) return 9;
            else return 10;
        }

        private int ExperienceCalculator(int level)
        {
            if (level <= 0) return 0;
            else if (level <= 1) return 83;
            else if (level <= 2) return 174;
            else if (level <= 3) return 266;
            else if (level <= 4) return 389;
            else if (level <= 5) return 572;
            else if (level <= 6) return 939;
            else if (level <= 7) return 1306;
            else if (level <= 8) return 1673;
            else if (level <= 9) return 2407;
            else return 10;
        }
    }
}
