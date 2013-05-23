using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Networking.Avatar;
using TileEngine.Animations;
using TileEngine.Entities.GameComponents;
using TileEngine.Entities.MenuComponents;

namespace TileEngine.Banner
{
    public class MenuBanner : IBanner
    {

        #region Declarations

        Vector2 fontSize;
        string playerName, playerAvatarName;
        Color color;
        Vector2 location;
        IAnimation loadingAnimation;
        bool awaitAvatar;

        #endregion

        #region Constructor

        public MenuBanner(Texture2D texture, SpriteFont font, SpriteFont smallFont, Vector2 location, string name, Color color)
        {
            this.Texture = texture;
            this.Color = color;
            this.Location = location;
            this.Font = font;
            this.PlayerName = name;
            this.AwaitAvatar=this.ShowAvatar = false;
            loadingAnimation = new CircularAnimation(font, 15.0f);
        }

        #endregion

        #region Initialize

        public void Initialize(Texture2D texture, string text)
        {
            ;
        }

        #endregion

        #region Properties

        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                playerAvatarName = value;
                ShowAvatar = false;
                fontSize = Font.MeasureString(PlayerName);
            }
        }

        public string PlayerAvatarName
        {
            get
            {
                return playerAvatarName;
            }
            set
            {
                playerAvatarName = playerName;
                playerName = value;
            }
        }

        public Vector2 Location
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth / 2 - this.Texture.Width / 2, location.Y);
            }
            set
            {
                location = value;
            }
        }

        private Vector2 FontLocation
        {
            get
            {
                    return new Vector2(Location.X + Texture.Width / 2 - (fontSize.X / 2), Location.Y + Texture.Height / 2 - 2 - (fontSize.Y / 2));
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public bool ShowAvatar
        {
            get;
            set;
        }

        public int Score
        {
            get
            {
                return 0;
            }
            set
            {
                int score = value;
            }
        }

        public bool Signal
        {
            get
            {
                return false;
            }
        }

        #region Information about Interface

        Texture2D Texture
        {
            get;
            set;
        }

        Texture2D AvatarTexture
        {
            get;
            set;
        }

        SpriteFont Font
        {
            get;
            set;
        }

        private Rectangle BannerRectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Texture.Width, Texture.Height);
            }
        }

        private Rectangle AvatarRectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Texture.Height, Texture.Height);
            }
        }

        private Color NegativeColor
        {
            get
            {
                if (color == Color.Black)
                    return Color.White;
                else
                    return Color.Black;
            }
        }
        
        #endregion

        #endregion

        #region Public Methods

        private LoadAvatarThread AvatarThread
        {
            get;
            set;
        }

        public bool AwaitAvatar
        {
            get
            {
                return awaitAvatar;
            }
            private set
            {
                awaitAvatar = value;
            }
        }

        public Texture2D GetAvatar()
        {
            return AvatarTexture;
        }

        public void LoadGameAvatar(GraphicsDevice graphicsDevice)
        {
            if (!AwaitAvatar && !ShowAvatar)
            {
                AvatarThread = new LoadAvatarThread(PlayerName, graphicsDevice);
                loadingAnimation.Text = " ";
                loadingAnimation.InitialLocation = new Vector2(0, -80);
                loadingAnimation.Location = new Vector2(Location.X-20 , Location.Y -15);
                AwaitAvatar = true;
            }
        }

        public void UpdateScore(int newScore)
        {
            ;
        }

        public void UpdateQuantityAt(int pos,int quantity)
        {
            ;
        }

        private void CheckAvatarThread()
        {
            if (!AvatarThread.thread.IsAlive)
            {
                if (!AvatarThread.Avatar.HasFailed)
                {
                    AvatarTexture = AvatarThread.Avatar.Texture;
                    PlayerAvatarName = AvatarThread.Avatar.UsrName;
                    ShowAvatar = true;
                }
                else
                    ShowAvatar = false;
                AvatarThread.Dispose();
                AwaitAvatar = false;
            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (AwaitAvatar)
            {
                loadingAnimation.Location = new Vector2(Location.X +15, Location.Y+5);
                loadingAnimation.Update(gameTime);
                CheckAvatarThread();
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (AwaitAvatar)
                loadingAnimation.Draw(spriteBatch);

            if (ShowAvatar)
                spriteBatch.Draw(AvatarTexture, AvatarRectangle, null,Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.03f);

            spriteBatch.Draw(Texture, BannerRectangle, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.09f);
            spriteBatch.DrawString(Font, PlayerName, FontLocation, NegativeColor);
        }

        #endregion
    }
}
