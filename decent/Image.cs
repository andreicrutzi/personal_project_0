using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace decent
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRectangle;
        [XmlIgnore]
        public Texture2D Texture;
        Vector2 Origin;
        ContentManager Content;
        RenderTarget2D RenderTarget;
        SpriteFont Font;

        public Image() 
        {
            Path = Text = String.Empty;
            FontName = "Arial";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRectangle = Rectangle.Empty;
        }

        public void LoadContent() 
        {
            Content = new ContentManager(SceneManager.Instance.Content.ServiceProvider, "Content");
            if(Path != String.Empty)
            {
                Texture = Content.Load<Texture2D>(Path);
            }

            Font = Content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;
            
            if(Texture != null)
            {
                dimensions.X += Texture.Width;
            }
            dimensions.X += Font.MeasureString(Text).X;

            if(Texture != null)
            {
                dimensions.Y = Math.Max(Texture.Height, Font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.Y = Font.MeasureString(Text).Y;
            }

            if(SourceRectangle == Rectangle.Empty)
            {
                SourceRectangle = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            RenderTarget = new RenderTarget2D(SceneManager.Instance.graphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            SceneManager.Instance.graphicsDevice.SetRenderTarget(RenderTarget);
            SceneManager.Instance.graphicsDevice.Clear(Color.Transparent);
            SceneManager.Instance.spriteBatch.Begin();
            if(Texture != null)
            {
                SceneManager.Instance.spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }
            SceneManager.Instance.spriteBatch.DrawString(Font, Text, Vector2.Zero, Color.White);
            SceneManager.Instance.spriteBatch.End();

            Texture = RenderTarget;
            SceneManager.Instance.graphicsDevice.SetRenderTarget(null);
        }

        public void UnloadContent()
        {
            Content.Unload();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            spriteBatch.Draw(Texture, Position + Origin, SourceRectangle, Color.White * Alpha,
                0.0f, Origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
