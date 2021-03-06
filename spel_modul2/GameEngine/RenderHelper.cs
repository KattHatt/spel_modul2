﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public enum RenderLayer { Tiles, Background1, Background2, Background3, Layer1, Layer2, Layer3, Layer4, Layer5, Foreground1, Foreground2, ForeGround3, GUI1, GUI2 , GUI3, Menubackground, MenuButton};

public class RenderHelper
{
    public GraphicsDevice graphicsDevice { get; }
    public SpriteBatch spriteBatch { get; }
    private float[] layerDepths;

    public RenderHelper(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        int length = Enum.GetValues(typeof(RenderLayer)).Length;
        layerDepths = new float[length];

        for (int i = 0; i < length; i++)
        {
            layerDepths[i] = (1f / length) * i;
        }

        this.graphicsDevice = graphicsDevice;
        this.spriteBatch = spriteBatch;

        rectangleTexture = new Texture2D(graphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White });
    }

    public float GetLayerDepth(RenderLayer layer)
    {
        return layerDepths[(int)layer];
    }

    public void Draw(Texture2D texture, Vector2 position, RenderLayer layer)
    {
        if (texture != null)
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, GetLayerDepth(layer));
    }

    public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color, RenderLayer layer)
    {
        if (texture != null)
            spriteBatch.Draw(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, GetLayerDepth(layer));
    }

    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, RenderLayer layer)
    {
        spriteBatch.DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, GetLayerDepth(layer));
    }

    private Texture2D rectangleTexture;

    public void DrawRectangle(Rectangle rectangle, int strokeWidth, Color color, RenderLayer layer)
    {
        spriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, strokeWidth), null, color, 0f, Vector2.Zero, SpriteEffects.None, GetLayerDepth(layer));
        spriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width + strokeWidth, strokeWidth), null, color, 0f, Vector2.Zero, SpriteEffects.None, GetLayerDepth(layer));
        spriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.Left, rectangle.Top, strokeWidth, rectangle.Height), null, color, 0f, Vector2.Zero, SpriteEffects.None, GetLayerDepth(layer));
        spriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.Right, rectangle.Top, strokeWidth, rectangle.Height), null, color, 0f, Vector2.Zero, SpriteEffects.None, GetLayerDepth(layer));
    }

    public void DrawFilledRectangle(Rectangle rectangle, Color color, RenderLayer layer)
    {
        spriteBatch.Draw(rectangleTexture, rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, GetLayerDepth(layer));
    }
}