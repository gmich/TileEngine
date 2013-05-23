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
    public class LobbyBanner : IBanner , IEquatable<IBanner>
    {

        #region Declarations

        protected Vector2 fontSize;
        string playerName, playerAvatarName;
        Color color;
        bool awaitAvatar;
        Vector2 location;
        IAnimation loadingAnimation;

        #endregion

        #region Constructor

        public LobbyBanner(Texture2D texture, bool showAvatar, SpriteFont font, Vector2 location, string name, Color color)
        {
            this.Texture = texture;
            this.Color = color;
            this.Location = location;
            this.Font = font;
            this.playerAvatarName=this.PlayerName = name;
            this.ShowAvatar=showAvatar;
            fontSize = Font.MeasureString(PlayerName);
            loadingAnimation = new CircularAnimation(font,15.0f);
        }

        #endregion

        #region Initialize

        public void Initialize(Texture2D texture, string text)
        {
            return;
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

        public virtual Vector2 Location
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

        protected virtual Vector2 FontLocation
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

        public virtual int Score
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

        public virtual bool Signal
        {
            get
            {
                return false;
            }
        }

        #region Information about Interface

        protected Texture2D Texture
        {
            get;
            set;
        }

        protected Texture2D AvatarTexture
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

        protected virtual Rectangle AvatarRectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Texture.Height, Texture.Height);
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

        public void LoadGameAvatar(GraphicsDevice graphicsDevice)
        {
            if (!AwaitAvatar && !ShowAvatar)
            {
                AvatarThread = new LoadAvatarThread(PlayerName, graphicsDevice);
                loadingAnimation.Text = " ";
                loadingAnimation.InitialLocation = new Vector2(0, -80);
                loadingAnimation.Location = Location;
                AwaitAvatar = true;
            }
        }

        public Texture2D GetAvatar()
        {
            return AvatarTexture;
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

        public virtual void Update(GameTime gameTime)
        {
            if (AwaitAvatar)
            {
                loadingAnimation.Location = new Vector2(Location.X+15, Location.Y+5);
                loadingAnimation.Update(gameTime);
                CheckAvatarThread();
            }
        }

        #endregion

        #region Draw

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (AwaitAvatar)
                loadingAnimation.Draw(spriteBatch);

            if (ShowAvatar)
                spriteBatch.Draw(AvatarTexture, AvatarRectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.08f);

            spriteBatch.Draw(Texture, BannerRectangle, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.09f);
            spriteBatch.DrawString(Font, PlayerName, FontLocation, Color.Black);
        }

        #endregion

        #region Implement IEquatable

        public bool Equals(IBanner bannerToTest)
        {
            return (Color == bannerToTest.Color && PlayerAvatarName == bannerToTest.PlayerAvatarName);
        }

        #endregion
    }
}
