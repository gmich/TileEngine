using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Input;

namespace Carcassonne.Managers
{
    public class ScreenshotManager
    {
        #region Declarations

        GraphicsDevice graphicsDevice;
        static int counter;
        int width, height;
        SpriteFont font;

        #endregion

        #region Constructor

        public ScreenshotManager(GraphicsDevice graphicsDevice,SpriteFont font)
        {
            this.graphicsDevice = graphicsDevice;
            counter = width = height = 0;
            NewShot = false;
            this.font = font;
        }

        #endregion

        #region Properties

        public bool NewShot
        {
            get;
            private set;
        }

        private string Text
        {
            get
            {
                return ("Screenshot no" + counter + " taken");
            }
        }

        #endregion

        #region Methods


        private void InitializeScreenshot()
        {
            counter++;
            width = graphicsDevice.PresentationParameters.BackBufferWidth;
            height = graphicsDevice.PresentationParameters.BackBufferHeight;
        }

        public void SaveScreenshot()
        {
            int[] backBuffer = new int[width * height];
            graphicsDevice.GetBackBufferData(backBuffer);

            Texture2D texture = new Texture2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            Stream stream = File.OpenWrite(".\\Screenshots\\" + counter + ".jpg");

            texture.SaveAsJpeg(stream, width, height);
            stream.Dispose();

            texture.Dispose();
            NewShot = false;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                InitializeScreenshot();
                NewShot = true;
            }
        }

        #endregion

    }
}
