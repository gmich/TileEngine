using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Banner;
using TileEngine;
using TileEngine.Networking.Args;
using Carcassonne.Configuration;

namespace Carcassonne.Managers
{
    public class BannerManager
    {

        #region Declarations 

        #region Events

        public event EventHandler<BannerArgs> RequestBanner;
        public event EventHandler<BannerArgs> PlayerBanner;
        public event EventHandler<BannerArgs> AddBanner;
        public event EventHandler<ColorArgs> ColorSwitch;
        public event EventHandler<ScoreArgs> ScoreUpdate;

        #endregion

        List<IBanner> banners;
        List<int> bannerIDs;
        private readonly GraphicsDevice graphicsDevice;
        PlayerConfiguration configuration;

        #endregion

        #region Constructor

        public BannerManager(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            configuration = new PlayerConfiguration();

            banners = new List<IBanner>();
            bannerIDs = new List<int>();

            GameBannerTexture = Content.Load<Texture2D>(@"Textures\Banners\GameBanner");
            BannerTexture = Content.Load<Texture2D>(@"Textures\Banners\Banner");
            LobbyBannerTexture = Content.Load<Texture2D>(@"Textures\Banners\LobbyBanner");
            ButtonTexture = Content.Load<Texture2D>(@"Textures\Buttons\ScoreButton");
            Font = Content.Load<SpriteFont>(@"Fonts\font");
            SmallFont = Content.Load<SpriteFont>(@"Fonts\smallFont");
            ScoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");

            try
            {
                banners.Add(new MenuBanner(BannerTexture, Font, SmallFont, Vector2.Zero, configuration.Name, TileGrid.ColorComparer(configuration.ColorID)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                configuration.CreateDefaultXML();
                configuration.LoadFromXML();
                banners.Add(new MenuBanner(BannerTexture, Font, SmallFont, Vector2.Zero, configuration.Name, TileGrid.ColorComparer(configuration.ColorID)));
            }

            TileGrid.Player = configuration.Name;
            TileGrid.activeColor = TileGrid.ColorComparer(configuration.ColorID);
            bannerIDs.Add(0);
            if (configuration.LoadAvatar)
                LoadGameAvatar();

        }
        #endregion

        #region Event Calls

        public void OnColorSwitch(string name, int color, int oldColor, int pos)
        {
            EventHandler<ColorArgs> colorSwitch = ColorSwitch;
            if (colorSwitch != null)
                colorSwitch(ColorSwitch, new ColorArgs(name, color, oldColor,pos));
        }

        public void OnUpdateScore(int bannerID, int score, int color)
        {
            EventHandler<ScoreArgs> scoreUpdate = ScoreUpdate;
            if (scoreUpdate != null)
                scoreUpdate(ScoreUpdate, new ScoreArgs(bannerID, score, color));
        }

        public void OnRequestBanner(string name, int color, bool isUsingAvatar)
        {
            EventHandler<BannerArgs> requestBanner = RequestBanner;
            if (requestBanner != null)
                requestBanner(requestBanner, new BannerArgs(name, color, isUsingAvatar, 1));
        }

        public void OnPlayerBanner()
        {           
            EventHandler<BannerArgs> playerBanner = PlayerBanner;

            if(playerBanner != null)
                playerBanner(playerBanner,new BannerArgs(banners[banners.Count-1].PlayerAvatarName,0,true, banners.Count-1));
        }

        public void OnSendAllBanners()
        {
            EventHandler<BannerArgs> addBanner = AddBanner;

            for (int i = 0; i < banners.Count; i++)
            {
                if (addBanner != null)
                    if (banners[i] is LobbyBanner)
                        addBanner(addBanner, new BannerArgs(banners[i].PlayerAvatarName, TileGrid.ColorComparer(banners[i].Color), (banners[i].ShowAvatar || banners[i].AwaitAvatar), i));
            }

        }

        #endregion

        #region Event Handling

        public void UpdateScore(int bannerID, int score)
        {
            banners[bannerID].Score = score;
        }

        #endregion

        #region Properties

        Vector2 BannerLocation
        {
            get
            {
                //TODO: update
                return new Vector2(0, LobbyBannerTexture.Height * (banners.Count + 4) + 20);
            }
        }

        #endregion

        #region Textures and Fonts

        Texture2D LobbyBannerTexture
        {
            get;
            set;
        }

        Texture2D BannerTexture
        {
            get;
            set;
        }

        Texture2D GameBannerTexture
        {
            get;
            set;
        }

        Texture2D ButtonTexture
        {
            get;
            set;
        }

        SpriteFont Font
        {
            get;
            set;
        }

        SpriteFont ScoreFont
        {
            get;
            set;
        }

        SpriteFont SmallFont
        {
            get;
            set;
        }

        public int LobbyBannerPos
        {
            get
            {
                if(bannerIDs.Count>1)
                return bannerIDs[1];

                return 0;
            }
        }

        public int LobbyBannerCount
        {
            get
            {
                int count = 0;
                foreach (IBanner banner in banners)
                    if (banner is LobbyBanner)
                        count++;
                return count;
            }
        }

        public bool IsUsingAvatar
        {
            get
            {
                return banners[0].ShowAvatar;
            }
        }

        public string PlayerAvatarName
        {
            get
            {
                return banners[0].PlayerAvatarName;
            }
        }

        public string PlayerName
        {
            get
            {
                return banners[0].PlayerName;
            }
        }
         
        #endregion

        #region Add/Remove

        public void AddLobbyBanner(string name, Color color, bool loadAvatar)
        {
            LobbyBanner LBanner = new LobbyBanner(LobbyBannerTexture, false, Font, BannerLocation, name, color);

            banners.Add(LBanner);
            if (loadAvatar)
                banners[banners.Count - 1].LoadGameAvatar(graphicsDevice);
        }

        public void RemoveLobbyBanner(int pos)
        {
            banners.RemoveAt(pos);
            if (LobbyBannerPos > pos)
                bannerIDs[1]--;
            ResetLobbyBannerLocations();
        }

        public void AddLobbyBanner(string name, Color color, bool loadAvatar, int pos)
        {
            LobbyBanner LBanner = new LobbyBanner(LobbyBannerTexture, false, Font, BannerLocation, name, color);

            if (pos + 1 > banners.Count)
            {
                banners.Add(LBanner);
                if (loadAvatar)
                    banners[banners.Count - 1].LoadGameAvatar(graphicsDevice);
            }
        }

        public void AddPlayerLobbyBanner()
        {
            bannerIDs.Add(1);
            AddLobbyBanner(banners[0].PlayerAvatarName, banners[0].Color, banners[0].ShowAvatar);
        }

        public void RemoveLobbyBanners()
        {
            banners.RemoveAll(i => (i is LobbyBanner && !(i is GameBanner)));

            bannerIDs.Clear();
            bannerIDs.Add(0);
        }

        public void RemoveGameBanners()
        {
            banners.RemoveAll(i => (i is GameBanner));

            bannerIDs.Clear();
            bannerIDs.Add(0);
        }

        #endregion

        #region Configure Player Avatar

        public void SetName(string name)
        {
            banners[0].PlayerName = name;
            TileGrid.Player = name;
        }

        public void AddBannerID(int id)
        {
            if (bannerIDs.Count <= 1)
                bannerIDs.Add(id);
        }

        public void SetColor(Color color)
        {
            TileGrid.activeColor = color;
            foreach(int x in bannerIDs)
                banners[x].Color = color;
        }

        public void SetColor(Color color, int pos)
        {
                banners[pos].Color = color;
        }

        public void LoadGameAvatar()
        {
            banners[0].LoadGameAvatar(graphicsDevice);
        }

        #endregion

        #region Helper Methdods

        public bool CanStartGame()
        {
            for (int i = 1; i < banners.Count-1; i++)
            {
                for (int j = i + 1; j < banners.Count; j++)
                    if (banners[j].Color == banners[i].Color)
                        return false;
            }
            return true;
        }

        private void ResetLobbyBannerLocations()
        {
            for (int i = 1; i < banners.Count; i++)
                banners[i].Location = new Vector2(0, LobbyBannerTexture.Height * (i + 4) + 20);
        }

        public void SavePlayerInfo()
        {
            configuration.Name=banners[0].PlayerAvatarName;
            configuration.LoadAvatar = banners[0].ShowAvatar;
            configuration.ColorID = TileGrid.ColorComparer(banners[0].Color);
            configuration.SaveToXML();
        }

        public Color BannerColor(int pos)
        {
            return banners[pos].Color;
        }

        public Vector2 GameBannerLocation(int x)
        {
            return new Vector2(GameBannerTexture.Width, (x-1) * GameBannerTexture.Height);
        }

        #endregion

        #region Start New Game

        //TODO: update
        public void NewGame()
        {
            int bannersCount = banners.Count;
            for (int i = 1; i < bannersCount; i++)
            {
                banners.Add(new GameBanner(GameBannerTexture, banners[i].GetAvatar(),banners[i].ShowAvatar, SmallFont, Font,ScoreFont,
                            GameBannerLocation(i), banners[i].PlayerName, banners[i].Color, ButtonTexture));
            }

            RemoveLobbyBanners();
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            for(int i=0;i<banners.Count;i++)
            {
                banners[i].Update(gameTime);
                if (banners[i] is GameBanner && banners[i].Signal)
                    OnUpdateScore(i, banners[i].Score, TileGrid.ColorComparer(TileGrid.activeColor));
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, bool ShowMainBanner)
        {
            int x = 1;
            if (ShowMainBanner)
                x = 0;
            for (int i = x; i < banners.Count; i++)
                banners[i].Draw(spriteBatch);
        }

        #endregion
    }
}
