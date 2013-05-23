using System;
using Microsoft.Xna.Framework;

namespace TileEngine
{
   public static class Camera
    {

        #region Declarations

        private static Vector2 viewPortSize = Vector2.Zero;
        private static Vector2 position = Vector2.Zero;
        private static float scale = 1.0f;

        #endregion

        #region Properties

        public static float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public static Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(MathHelper.Clamp(value.X, 0, WorldRectangle.Width),
                    MathHelper.Clamp(value.Y, 0, WorldRectangle.Height));
            }
        }

        public static Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, TileGrid.MapWidth * TileGrid.TileWidth, TileGrid.MapHeight * TileGrid.TileHeight);
            }
        }

        public static int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set
            {
                viewPortSize.X = value;
            }
        }

        public static int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set
            {
                viewPortSize.Y = value;
            }
        }

        public static Rectangle ScreenRectangle
        {
            get
            {
                return new Rectangle(0, 0,ViewPortWidth, ViewPortHeight);
            }
        }

        public static Rectangle ViewPort
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, ViewPortWidth, ViewPortHeight);
            }
        }

        #endregion

        #region Public Methods
       
        public static void Move(Vector2 offset)
        {
            Position += offset;
        }
       
        public static bool inScreenBounds(Vector2 location)
        {
            return (location.X > Camera.Position.X && location.X < Camera.Position.X + Camera.ViewPortWidth
                 && location.Y > Camera.Position.Y && location.Y < Camera.Position.Y + Camera.ViewPortHeight);
        }

        public static Vector2 AdjustInWorldBounds(Vector2 location, float width, float height)
        {
            location.X = MathHelper.Clamp(location.X, position.X, position.X + Camera.ViewPortWidth - width);
            location.Y = MathHelper.Clamp(location.Y, position.Y, position.Y + Camera.ViewPortHeight - height);
            return location;
        }

        public static Vector2 AdjustInWorldBounds(Vector2 location, float width, float height,Vector2 origin)
        {
            location.X = MathHelper.Clamp(location.X, position.X+origin.X, position.X + Camera.ViewPortWidth - width+origin.X);
            location.Y = MathHelper.Clamp(location.Y, position.Y+origin.Y, position.Y + Camera.ViewPortHeight - height+origin.Y);
            return location;
        }

        public static Vector2 AdjustInScreenBounds(Vector2 position,float dimension)
        {
            position.X = MathHelper.Clamp(position.X, dimension / 2, Camera.ViewPortWidth - dimension / 2);
            position.Y = MathHelper.Clamp(position.Y, dimension / 2, Camera.ViewPortHeight - dimension / 2);
            return position;
        }

        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public static bool ObjectOnScreenBounds(Rectangle bounds)
        {
            return (ScreenRectangle.Intersects(bounds));
        }

        public static Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - position;
        }

        public static Rectangle WorldToScreen(Rectangle worldRectangle)
        {
            return new Rectangle(worldRectangle.Left - (int)position.X, worldRectangle.Top - (int)position.Y, worldRectangle.Width, worldRectangle.Height);
        }
       
        public static Vector2 VectorWorldToScreen(Vector2 worldLocation)
        {
            return new Vector2(worldLocation.X - Position.X, worldLocation.Y - Position.Y);
        }

        public static Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + position;
        }

        public static Rectangle ScreenToWorld(Rectangle screenRectangle)
        {
            return new Rectangle(screenRectangle.Left + (int)position.X, screenRectangle.Top + (int)position.Y, screenRectangle.Width, screenRectangle.Height);
        }

        #endregion

    }
}
