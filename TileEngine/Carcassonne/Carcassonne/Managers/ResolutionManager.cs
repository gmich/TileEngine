using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne.Managers
{
    using TileEngine;
    using TileEngine.Input;

    public class ResolutionManager
    {
        #region Declarations

        private GraphicsDeviceManager graphicsDeviceManager;
        private int height, vheight;
        private int width, vwidth;
        private DisplayMode displayM;
        private bool isFullScreen;
        private readonly CameraManager cameraManager;

        #endregion

        #region Constructor

        public ResolutionManager(ref CameraManager cameraManager)
        {
            this.cameraManager = cameraManager;
        }

        #endregion

        #region Initialize

        public void Initialize(ref GraphicsDeviceManager device, bool newFullScreen)
        {
            this.width = device.PreferredBackBufferWidth;
            this.height = device.PreferredBackBufferHeight;
            this.graphicsDeviceManager = device;
            this.isFullScreen = newFullScreen;
            this.ApplyResolutionSettings();
        }

        #endregion

        #region Methods

        public void SetResolution(int newWidth, int newHeight)
        {
            Camera.ViewPortWidth = this.width = newWidth;
            Camera.ViewPortHeight = this.height = newHeight;
            this.ApplyResolutionSettings();
            cameraManager.WorldLocation = (Camera.Position + new Vector2(newWidth / 2, newHeight / 2));
            cameraManager.RepositionCamera();
        }

        private void ApplyResolutionSettings()
        {
                if (this.isFullScreen == false)
                {
                    if ((this.width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                        && (this.height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                    {
                        this.graphicsDeviceManager.PreferredBackBufferWidth = (int)MathHelper.Max(1, (float)this.width);
                        this.graphicsDeviceManager.PreferredBackBufferHeight = (int)MathHelper.Max(1, (float)this.height);
                        this.graphicsDeviceManager.IsFullScreen = this.isFullScreen;
                        this.graphicsDeviceManager.ApplyChanges();
                    }
                }
                else
                {
                    foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                    {
                        if ((dm.Width == this.width) && (dm.Height == this.height))
                        {
                            this.graphicsDeviceManager.PreferredBackBufferWidth = this.width;
                            this.graphicsDeviceManager.PreferredBackBufferHeight = this.height;
                            this.graphicsDeviceManager.IsFullScreen = this.isFullScreen;
                            this.graphicsDeviceManager.ApplyChanges();
                            break;
                        }
                    }
                }
                this.width = this.graphicsDeviceManager.PreferredBackBufferWidth;
                this.height = this.graphicsDeviceManager.PreferredBackBufferHeight;
        }

        private void FindHighestSupportedResolution()
        {
            displayM = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        }

        private void ToggleFullScreen()
        {
            if (!this.isFullScreen)
            {
                vwidth = width;
                vheight = height;
                this.isFullScreen = true;
                FindHighestSupportedResolution();
                SetResolution(displayM.Width, displayM.Height);
            }
            else
            {
                this.isFullScreen = false;
                SetResolution(vwidth, vheight);
            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F11))
            {
                ToggleFullScreen();
            }
        }

        #endregion
    }
}