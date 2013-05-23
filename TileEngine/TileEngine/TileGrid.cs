using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities;

namespace TileEngine
{
    public static class TileGrid
    {
        #region Declarations

        public const int MapWidth = 100;
        public const int MapHeight = 100;
        public static int MaxColors = 4;
        public static Color activeColor;
        public static string Player;
        public static int tileWidth, tileHeight;
        public static string Expansions;
        public static GameStates GameState;

        static public MapSquare[,] mapSquares;

        #endregion
        
        #region Initialization

        static public void Initialize(ContentManager Content, string playerID)
        {
            Border = Content.Load<Texture2D>(@"Textures\TileGrid\Table");
            Square = Content.Load<Texture2D>(@"Textures\TileGrid\MapSquare");

            tileWidth=TileWidth = Square.Width;
            tileHeight=TileHeight = Square.Height;
            PlayerID = playerID;
            mapSquares = new MapSquare[MapWidth, MapHeight];
            Random rand = new Random();
            ResetGrid();
        }

        static public void ResetGrid()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (x == 0 || y == 0 || x == MapWidth - 1 || y == MapWidth - 1)
                        mapSquares[x, y] = new MapSquare(Border, true);
                    else
                        mapSquares[x, y] = new MapSquare(Square, false);
                }
            }
        }

        #endregion

        #region Properties

        public static Texture2D Border
        {
            get;
            set;
        }

        public static Texture2D Square
        {
            get;
            set;
        }

        public static int TileWidth
        {
            get;
            set;
        }

        public static int TileHeight
        {
            get
            {
                return (int)(tileHeight);
            }
            set
            {
                tileHeight = value;
            }
        }

        static string PlayerID
        {
            get;
            set;
        }

        #endregion

        #region Information about MapSquares

        static public int GetSquareByPixelX(int pixelX)
        {
            return pixelX / TileWidth;
        }

        static public int GetSquareByPixelY(int pixelY)
        {
            return pixelY / TileHeight;
        }

        static public Vector2 GetSquareByPixel(Vector2 pixelLocation)
        {
            return new Vector2(GetSquareByPixelX((int)pixelLocation.X),GetSquareByPixelY((int)pixelLocation.Y));
        }

        static public bool SquareIsOccupied(Vector2 pixelLocation)
        {
            Vector2 squareLocation = GetSquareByPixel(pixelLocation);
            return mapSquares[(int)squareLocation.X,(int)squareLocation.Y].Occupied;
        }

        static public void OccupySquareAt(Vector2 pixelLocation,bool value)
        {
            Vector2 squareLocation = GetSquareByPixel(pixelLocation);
            mapSquares[(int)squareLocation.X, (int)squareLocation.Y].Occupied = value;
        }

        static public Vector2 GetSquareCenter(int SquareX, int SquareY)
        {
            return new Vector2((SquareX * TileWidth) + (TileWidth / 2),(SquareY * TileHeight) + (TileHeight / 2));
        }

        static public Rectangle SquareWorldRectangle(int SquareX, int SquareY)
        {
            return new Rectangle(SquareX * TileWidth, SquareY * TileHeight, TileWidth, TileHeight);
        }

        static public Rectangle SquareWorldRectangle(Vector2 Square)
        {
            return SquareWorldRectangle((int)Square.X,(int)Square.Y);
        }

        static public Rectangle SquareScreenRectangle(int SquareX, int SquareY)
        {
            return Camera.WorldToScreen(SquareWorldRectangle(SquareX, SquareY));
        }
        
        static public Rectangle SquareSreenRectangle(Vector2 Square)
        {
            return SquareScreenRectangle((int)Square.X, (int)Square.Y);
        }
        
        static public Vector2 GetSquareLocation(Vector2 pixelLocation)
        {
            Vector2 location = GetSquareByPixel(pixelLocation);

            return new Vector2(location.X * TileWidth, location.Y * TileHeight);
        }

        #endregion

        #region Tile Helper

        static public Vector2 PositionInWorldBounds(Vector2 mousePos)
        {

            if (mousePos.Y < (float)TileHeight)
                mousePos.Y = (float)TileHeight;
            else if (mousePos.Y > ((float)TileHeight * (MapHeight - 2)))
                mousePos.Y = (float)TileHeight * (MapHeight - 2);

            if (mousePos.X < (float)TileWidth)
                mousePos.X = (float)TileWidth;
            else if (mousePos.X > ((float)TileWidth * (MapWidth - 2)))
                mousePos.X = (float)TileWidth * (MapWidth - 2);

            return mousePos;
        }

        #endregion

        #region Color Comparer

        static public Color ColorComparer(int colorID)
        {
            switch (colorID)
            {
                case 1:
                    return new Color(255,100,116);
                case 2:
                    return Color.CornflowerBlue;
                case 3:
                    return Color.LightGreen;
                case 4:
                    return Color.Yellow;
                default:
                    return Color.Black;
            }
        }

        static public int ColorComparer(Color colorID)
        {
            if (colorID == new Color(255,100,116))
                return 1;
            if (colorID == Color.CornflowerBlue)
                return 2;
            if (colorID == Color.LightGreen)
                return 3;
            if (colorID == Color.Yellow)
                return 4;
            else
                return 5;
        }

        static public Color GetSolidColor(Color colorID)
        {
            if (colorID == new Color(255, 100, 116))
                return Color.Red;
            if (colorID == Color.CornflowerBlue)
                return Color.Blue;
            if (colorID == Color.LightGreen)
                return Color.Green;
            if (colorID == Color.Yellow)
                return Color.Yellow;
            else
                return Color.Black;
        }

        #endregion

        #region Draw

        static public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetSquareByPixelX((int)Camera.Position.X);
            int endX = GetSquareByPixelX((int)Camera.Position.X + Camera.ViewPortWidth);

            int startY = GetSquareByPixelY((int)Camera.Position.Y);
            int endY = GetSquareByPixelY((int)Camera.Position.Y + Camera.ViewPortHeight);

            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MapWidth) && (y < MapHeight))
                        {
                            //TODO: remove after debugging
                            if (mapSquares[x, y].Occupied)
                                spriteBatch.Draw(mapSquares[x, y].Texture, SquareScreenRectangle(x, y), null, Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                            else
                                spriteBatch.Draw(mapSquares[x, y].Texture, SquareScreenRectangle(x, y), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);

                        }
                    }
                }
        }
                
        #endregion
    }
}
