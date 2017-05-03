﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine
{
    class SoundLoaderSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
        }

        public void Load(ContentManager content)
        {
            var sounds = ComponentManager.GetInstance().GetComponentsOfType<SoundComponent>();
            foreach (SoundComponent soundComponent in sounds.Values)
            {
                soundComponent.AttackSound = content.Load<SoundEffect>(soundComponent.AttackFile).CreateInstance();
                soundComponent.WalkSound = content.Load<SoundEffect>(soundComponent.WalkFile).CreateInstance();
                soundComponent.DamageSound = content.Load<SoundEffect>(soundComponent.DamageFile).CreateInstance();
                soundComponent.WalkSound.IsLooped = true;
            }

            foreach (var entity in ComponentManager.GetInstance().GetComponentsOfType<SoundThemeComponent>())
            {
                SoundThemeComponent stc = (SoundThemeComponent)entity.Value;
                stc.Music = content.Load<SoundEffect>(stc.MusicFile).CreateInstance();
                stc.Music.Volume *= 0.1f;
            }
        }
    }
}