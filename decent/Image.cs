using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public bool IsActive;
        public Rectangle SourceRectangle;
        [XmlIgnore]
        public Texture2D Texture;
        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;
        public String Effects;

        public FadeEffect FadeEffect;

        void SetEffect<T>(ref T effect)
        {
            if(effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            else 
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            effectList.Add(effect.GetType().ToString().Replace("decent.", ""), (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if(effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if(effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                effectList[effect].UnloadContent();
            }
        }

        public Image() 
        {
            Path = Text = Effects = String.Empty;
            FontName = "Fonts/Arial";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRectangle = Rectangle.Empty;
            effectList = new Dictionary<string, ImageEffect>();
        }

        public void LoadContent() 
        {
            content = new ContentManager(SceneManager.Instance.Content.ServiceProvider, "Content");
            if(Path != String.Empty)
            {
                Texture = content.Load<Texture2D>(Path);
            }

            font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;
            
            if(Texture != null)
            {
                dimensions.X += Texture.Width;
            }
            dimensions.X += font.MeasureString(Text).X;

            if(Texture != null)
            {
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.Y = font.MeasureString(Text).Y;
            }

            if(SourceRectangle == Rectangle.Empty)
            {
                SourceRectangle = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            renderTarget = new RenderTarget2D(SceneManager.Instance.graphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            SceneManager.Instance.graphicsDevice.SetRenderTarget(renderTarget);
            SceneManager.Instance.graphicsDevice.Clear(Color.Transparent);
            SceneManager.Instance.spriteBatch.Begin();
            if(Texture != null)
            {
                SceneManager.Instance.spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }
            SceneManager.Instance.spriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            SceneManager.Instance.spriteBatch.End();

            Texture = renderTarget;
            SceneManager.Instance.graphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);

            if(Effects != String.Empty)
            {
                String[] split = Effects.Split(':');
                foreach(String item in split)
                    ActivateEffect(item);
            }
        }

        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var effect in effectList)
            {
                if(effect.Value.IsActive)
                    effect.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            spriteBatch.Draw(Texture, Position + origin, SourceRectangle, Color.White * Alpha,
                0.0f, origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
