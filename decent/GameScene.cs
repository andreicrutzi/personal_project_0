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
    public class GameScene
    {
        protected ContentManager Content;
        [XmlIgnore]
        public Type Type;
        public GameScene() 
        {
            Type = this.GetType();
        }

        public virtual void LoadContent()
        {
            Content = new ContentManager(
                SceneManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            Content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
