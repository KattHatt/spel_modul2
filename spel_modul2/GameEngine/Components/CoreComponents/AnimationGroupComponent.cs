﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Managers;

namespace GameEngine.Components
{
    public class AnimationGroupComponent : IComponent
    {
        public string SpritesheetFilename { get; set; }
        public Texture2D Spritesheet { get; set; }
        public Point SheetSize { get; set; }
        public Tuple<Point, Point>[] Animations { get; set; }
        public Point FrameSize { get; set; }
        public int FrameDuration { get; set; }
        public bool IsPaused { get; set; }
        private int activeAnimation;
        public int ActiveAnimation
        {
            get { return activeAnimation; }
            set
            {
                activeAnimation = value;
                GroupFrame = new Point(0, 0);
                CurrentFrame = GroupFrame + Animations[activeAnimation].Item1;
            }
        }
        public int LastFrameDeltaTime { get; set; }
        public Point GroupFrame;
        public Point CurrentFrame;
        public Rectangle SourceRectangle { get; set; }
        public Point Offset;
        public RenderLayer Layer;

        public AnimationGroupComponent(string spritesheetFilename, Point sheetSize, int frameDuration, params Tuple<Point, Point>[] animations) : this(spritesheetFilename, sheetSize, frameDuration, RenderLayer.Layer1, animations) { }

        public AnimationGroupComponent(string spritesheetFilename, Point sheetSize, int frameDuration, RenderLayer layer, params Tuple<Point, Point>[] animations)
        {
            ResourceManager rm = ResourceManager.GetInstance();

            SpritesheetFilename = spritesheetFilename;
            SheetSize = sheetSize;
            Animations = animations;
            FrameDuration = frameDuration;
            Layer = layer;

            Spritesheet = rm.GetResource<Texture2D>(spritesheetFilename);
        }
    }
}