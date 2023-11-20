using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace decent
{
    public class SceneManager
    {
        private static SceneManager instance;
        public Vector2 Dimensions { private set; get; }
        public ContentManager Content { private set; get; }
        XmlManager<GameScene> XmlGameSceneManager;

        GameScene currentScene;
        public GraphicsDevice graphicsDevice;
        public SpriteBatch spriteBatch;

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();

                return instance;
            }
        }

        public SceneManager()
        {
            Dimensions = new Vector2(1920, 1080);
            currentScene = new SplashScene();
            XmlGameSceneManager = new XmlManager<GameScene>();
            XmlGameSceneManager.Type = currentScene.Type;
            currentScene = XmlGameSceneManager.Load("Content/Load/SplashScene.xml");
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentScene.LoadContent();
        }

        public void UnloadContent() 
        {
            currentScene.UnloadContent();
        }

        public void Update(GameTime gameTime) 
        { 
            currentScene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScene.Draw(spriteBatch);
        }

    }
}
