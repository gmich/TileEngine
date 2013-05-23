using System;
using Microsoft.Xna.Framework;

namespace Carcassonne.Managers
{
    using TileEngine;
    using TileEngine.Input;

    public class CameraManager
    {

        #region Declarations

        private Vector2 initialPos;
        private Vector2 worldLocation;
        private Vector2 velocity;
        private Vector2 deSquareerating;
        private bool scrolling;
        private int scrollRate;
        public event ScaleChangedHandler ScaleChanged;

        #endregion

        #region Constructor

        public CameraManager(Vector2 worldLocation)
        {
            scrollRate = 2;
            this.worldLocation = worldLocation;
            deSquareerating = initialPos = velocity = Vector2.Zero;
            scrolling = false;
        }

        #endregion

        #region Properties

        public Vector2 WorldLocation
        {
            get
            {
                return worldLocation;
            }
            set
            {
                worldLocation.X = MathHelper.Clamp(value.X, Camera.ViewPortWidth / 2,
                                            TileGrid.MapWidth * TileGrid.TileWidth - Camera.ViewPortWidth / 2);
                worldLocation.Y = MathHelper.Clamp(value.Y, Camera.ViewPortHeight / 2,
                                            TileGrid.MapHeight * TileGrid.TileHeight - Camera.ViewPortHeight / 2);
            }
        }

        #endregion

        #region Helper Methods

        #region Scaling

        private void ScaleScreen(float newScale)
        {
            if (newScale != Camera.Scale)
            {
                if (newScale > Camera.Scale)
                {
                    TileGrid.TileWidth = TileGrid.TileHeight += scrollRate;
                }
                else
                {
                    TileGrid.TileWidth = TileGrid.TileHeight -= scrollRate;
                }

                WorldLocation *= (newScale / Camera.Scale);
                RepositionCamera();
                //Event
                if(ScaleChanged!=null)
                ScaleChanged(Camera.Scale, newScale);
                Camera.Scale = newScale;
            }
        }

        private void ScrollScalling()
        {
            float newValue = 0;

            if ((InputManager.IsWheelMovingUp() || InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                && InputManager.MouseInBounds)
                newValue = +(float)scrollRate / 100;
            else if ((InputManager.IsWheelMovingDown() || InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                && InputManager.MouseInBounds)
                newValue = -(float)scrollRate / 100;

            if (newValue != 0)
            {
                float newScale = (float)System.Math.Round((double)MathHelper.Clamp(Camera.Scale + newValue, 0.4f, 1.0f), 2);
                ScaleScreen(newScale);
            }
        }

        #endregion

        #region Scrolling

        private void ReduceVector(ref Vector2 vector)
        {
            float reduceAmount = 15.0f;
            float maxAcceleration = 2000f;
            if (vector.X > 0)
                vector.X = MathHelper.Clamp(vector.X - reduceAmount, 0, maxAcceleration);
            else
                vector.X = MathHelper.Clamp(vector.X + reduceAmount, -maxAcceleration, 0);

            if (vector.Y > 0)
                vector.Y = MathHelper.Clamp(vector.Y - reduceAmount, 0, maxAcceleration);
            else
                vector.Y = MathHelper.Clamp(vector.Y + reduceAmount, -maxAcceleration, 0);
        }

        public void RepositionCamera()
        {
            int screenLocX = (int)Camera.WorldToScreen(worldLocation).X;
            int screenLocY = (int)Camera.WorldToScreen(worldLocation).Y;

            if (screenLocY > Camera.ViewPortHeight / 2)
            {
                Camera.Move(new Vector2(0, screenLocY - Camera.ViewPortHeight / 2));
            }
            if (screenLocY < Camera.ViewPortHeight / 2)
            {
                Camera.Move(new Vector2(0, screenLocY - Camera.ViewPortHeight / 2));
            }

            if (screenLocX > Camera.ViewPortWidth / 2)
            {
                Camera.Move(new Vector2(screenLocX - Camera.ViewPortWidth / 2, 0));
            }

            if (screenLocX < Camera.ViewPortWidth / 2)
            {
                Camera.Move(new Vector2(screenLocX - Camera.ViewPortWidth / 2, 0));
            }
        }

        #endregion

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (InputManager.RightButtonIsClicked() && !scrolling && InputManager.MouseInBounds)
            {
                scrolling = true;
                initialPos = InputManager.MousePosition;
            }
            else if (InputManager.RightButtonIsClicked() && scrolling && InputManager.MouseInBounds)
            {
                velocity = initialPos - InputManager.MousePosition;
                initialPos = InputManager.MousePosition;
            }
            else if (!InputManager.RightButtonIsClicked() || !InputManager.MouseInBounds)
            {
                if (scrolling && InputManager.MouseInBounds)
                    deSquareerating = velocity*50;
                velocity = (float)gameTime.ElapsedGameTime.TotalSeconds * deSquareerating;
                ReduceVector(ref deSquareerating);
                scrolling = false;
            }

            if (velocity != Vector2.Zero)
            {
                WorldLocation += velocity;
                RepositionCamera();
            }
            ScrollScalling();
        }

        #endregion
    }
}