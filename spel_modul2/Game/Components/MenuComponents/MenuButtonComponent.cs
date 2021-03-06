﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Components;

namespace Game.Components
{
    public delegate void ButtonAction();
    public enum MenuButtonType { PauseMainMenuButton, PauseOptionsMenuButton, MainMenuButton, MainOptionsMenuButton };

    public class MenuButtonComponent : IComponent
    {
        public bool IsActive { get; set; }
        public bool Ishighlighted { get; set; }
        public MenuButtonType Type { get; set; }
        public string NormalTexturePath { get; set; }
        public string HighlightTexturePath { get; set; }
        public ButtonAction Use { get; set; }
        public Texture2D NormalTexture { get; set; }
        public Texture2D HighlightTexture { get; set; }
        public Vector2 Position { get; set; }
        public RenderLayer Layer { get; set; }
        public int OrderPosition { get; set; }

        public MenuButtonComponent(MenuButtonType type, ButtonAction buttonAction, string normalTexturePath, string highlightTexturePath, Vector2 position, RenderLayer layer)
        {
            IsActive = false;
            Ishighlighted = false;
            Type = type;
            Use = buttonAction;
            NormalTexturePath = normalTexturePath;
            HighlightTexturePath = highlightTexturePath;
            Position = position;
            Layer = layer;
        }

        public MenuButtonComponent(MenuButtonType type, ButtonAction buttonAction, string normalTexturePath, string highlightTexturePath, Vector2 position, RenderLayer layer, int orderPosition)
        {
            IsActive = false;
            Ishighlighted = false;
            Type = type;
            Use = buttonAction;
            NormalTexturePath = normalTexturePath;
            HighlightTexturePath = highlightTexturePath;
            Position = position;
            Layer = layer;
            OrderPosition = orderPosition;
        }

        public object Clone()
        {
            MenuButtonComponent o = (MenuButtonComponent)MemberwiseClone();
            o.Type = Type;
            o.NormalTexturePath = string.Copy(NormalTexturePath);
            o.HighlightTexturePath = string.Copy(HighlightTexturePath);
            return o;
        }
    }
}
