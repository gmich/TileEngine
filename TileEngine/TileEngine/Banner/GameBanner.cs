using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;

namespace TileEngine.Banner
{
    public class GameBanner : LobbyBanner
    {

        #region Declarations

        ScoreBanner scoreBanner;
        private Vector2 location;

        #endregion

        #region Constructor

        public GameBanner(Texture2D texture,Texture2D AvatarTexture, bool showAvatar, SpriteFont font,SpriteFont largeFont, SpriteFont scoreFont, Vector2 location, string name, Color color,Texture2D buttonTexture)
            : base(texture, showAvatar, largeFont, location, name, color)
        {
            this.AvatarTexture = AvatarTexture;
            scoreBanner = new ScoreBanner(buttonTexture, font, scoreFont, location - new Vector2(texture.Width-60,0), Color.Black);
        }

        #endregion

        #region Properties

        public override bool Signal
        {
            get
            {
                return scoreBanner.Updated;
            }
        }

        public override int Score
        {
            get
            {
                return scoreBanner.Score;
            }
            set
            {
                scoreBanner.Score = value;
            }
        }

        public override Vector2 Location
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth - location.X, location.Y);
            }
            set
            {
                location = value;
            }
        }

        protected override Vector2 FontLocation
        {
            get
            {
                return new Vector2(Location.X + Texture.Height + (fontSize.Y / 2), Location.Y+5);
            }
        }

        protected override Rectangle AvatarRectangle
        {
            get
            {
                int offSet = 4;
                return new Rectangle((int)Location.X + offSet, (int)Location.Y + offSet, Texture.Height - (offSet*2), Texture.Height - (offSet*2));
            }
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            scoreBanner.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            scoreBanner.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }

}