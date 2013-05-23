using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Carcassonne.Managers;
using TileEngine;
using TileEngine.Input;
using TileEngine.Diagnostics;
using TileEngine.Networking;

namespace Carcassonne
{
    public delegate void ScaleChangedHandler(float oldScale, float newScale);
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CameraManager cameraManager;
        ResolutionManager resolutionManager;
        TileManager tileManager;
        MenuManager menuManager;
        BannerManager bannerManager;
        FpsMonitor fpsMonitor;
        NetworkingManager networkingManager;
        AnimationManager animationManager;
        ScreenshotManager screenshotManager;

        public Game1()
        {
            TileGrid.GameState = GameStates.Menu;

            this.graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(OnWindowClientSizeChanged);
            this.graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            this.Window.Title = "Carcassonne BETA";

            TileGrid.Initialize(Content, "Kokos");

            #region Camera / Resolution 

            Camera.Position = new Vector2(TileGrid.MapWidth / 2 * TileGrid.TileWidth, TileGrid.MapHeight / 2 * TileGrid.TileHeight);
            Camera.ViewPortWidth = this.GraphicsDevice.Viewport.Width;
            Camera.ViewPortHeight = this.GraphicsDevice.Viewport.Height;
            cameraManager = new CameraManager(new Vector2(Camera.Position.X + Camera.ViewPortWidth / 2, Camera.Position.Y + Camera.ViewPortHeight / 2));
            this.resolutionManager = new ResolutionManager(ref cameraManager);
            this.resolutionManager.Initialize(ref this.graphics,this.graphics.IsFullScreen);

            #endregion

            cameraManager.ScaleChanged += new ScaleChangedHandler(OnScaleChange);
            InputManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            bannerManager = new BannerManager(Content, GraphicsDevice);
            animationManager = new AnimationManager(Content); 
            tileManager = new TileManager(Content, bannerManager);
            networkingManager = new NetworkingManager(bannerManager, tileManager,animationManager,Content.Load<SpriteFont>(@"Fonts\font"));
            fpsMonitor = new FpsMonitor(Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(10, 10));
            menuManager = new MenuManager(Content, bannerManager, networkingManager);
            screenshotManager = new ScreenshotManager(GraphicsDevice, Content.Load<SpriteFont>(@"Fonts\font"));
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || menuManager.ExitGame)
            {
                this.Exit();
            }

            if (InputManager.IsKeyReleased(Keys.Escape) && InputManager.MouseInBounds)
            {
                if (TileGrid.GameState == GameStates.Playing)
                {
                    menuManager.QuitGame();
                    TileGrid.GameState = GameStates.QuitGame;
                }
                else if (TileGrid.GameState == GameStates.QuitGame)
                    TileGrid.GameState = GameStates.Playing;
            }

            InputManager.Update(gameTime);
            resolutionManager.Update(gameTime);

            switch (TileGrid.GameState)
            {
                case GameStates.Menu:
                    menuManager.Update(gameTime);
                    break;
                case GameStates.Lobby:
                    networkingManager.Update(gameTime);
                    menuManager.Update(gameTime);
                    break;
                case GameStates.StartNewGame:
                    tileManager.OnPlayGame(true);
                    networkingManager.Update(gameTime);
                    tileManager.NewGame();
                    break;
                case GameStates.QuitGame:
                    menuManager.Update(gameTime);
                    networkingManager.Update(gameTime);
                    break;
                case GameStates.Playing:
                    cameraManager.Update(gameTime);
                    tileManager.Update(gameTime);
                    networkingManager.Update(gameTime);
                    animationManager.Update(gameTime);
                    break;
            }
            screenshotManager.Update(gameTime);
            bannerManager.Update(gameTime);
            fpsMonitor.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            switch (TileGrid.GameState)
            {
                case GameStates.Menu:
                    menuManager.Draw(spriteBatch);
                    bannerManager.Draw(spriteBatch, true);
                    break;
                case GameStates.Lobby:
                    menuManager.Draw(spriteBatch);
                    networkingManager.Draw(spriteBatch);
                    bannerManager.Draw(spriteBatch, true);
                    break;
                case GameStates.QuitGame:
                    menuManager.Draw(spriteBatch);
                    tileManager.Draw(spriteBatch);
                    TileGrid.Draw(spriteBatch);
                    bannerManager.Draw(spriteBatch, false);
                    animationManager.Draw(spriteBatch);
                    break;
                case GameStates.Playing:
                    tileManager.Draw(spriteBatch);
                    TileGrid.Draw(spriteBatch);
                    bannerManager.Draw(spriteBatch, false);
                    animationManager.Draw(spriteBatch);
                    break;
            }
            fpsMonitor.Draw(spriteBatch);

            spriteBatch.End();

            if (screenshotManager.NewShot)
                screenshotManager.SaveScreenshot();

            base.Draw(gameTime);
        }

        #region Events

        private void OnWindowClientSizeChanged(object sender, System.EventArgs e)
        {
            this.resolutionManager.SetResolution(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            tileManager.AdjustLocation();
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            bannerManager.SavePlayerInfo();
            base.OnExiting(sender, args);
        }

        public void OnScaleChange(float oldScale, float newScale)
        {
            tileManager.AdjustLocation(oldScale, newScale);
        }

        #endregion

    }
}
