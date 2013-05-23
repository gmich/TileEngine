using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Carcassonne.Menu;
using TileEngine;
using TileEngine.Networking;

namespace Carcassonne.Managers
{
    public class MenuManager
    {
        #region Menu Declarations

        readonly BannerManager bannerManager;
        readonly NetworkingManager networkingManager;
        IMenu activeMenu;
        MainMenu mainMenu;
        Profile profile;
        Play play;
        JoinLobby joinLobby;
        CreateLobby createLobby;
        ExpansionLobby expansionLobby;
        QuitGame quitGame;
        About about;

        #endregion

        public MenuManager(ContentManager Content, BannerManager bannerManager,NetworkingManager networkingManager)
        {
            this.bannerManager = bannerManager;
            this.networkingManager = networkingManager;
            BackgroundTexture = Content.Load<Texture2D>(@"menuBackground");
            mainMenu = new MainMenu(Content);
            profile = new Profile(Content);
            play = new Play(Content);
            joinLobby = new JoinLobby(Content);
            createLobby = new CreateLobby(Content);
            about = new About(Content);
            expansionLobby = new ExpansionLobby(Content);
            quitGame = new QuitGame(Content);

            MenuState = MenuStates.MainMenu;
            activeMenu = mainMenu;
            ExitGame = false;
        }

        #region Properties

        public bool ExitGame
        {
            get;
            private set;
        }

        public bool Ready
        {
            get;
            private set;
        }

        public MenuStates MenuState
        {
            get;
            private set;
        }

        private Texture2D BackgroundTexture
        {
            get;
            set;
        }

        private Vector2 BackgroundLocation
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth / 2 - BackgroundTexture.Width / 2 , 0);
            }

        }

        #endregion

        #region Helper Methods

        private bool CreateClient()
        {
            string ip = (string)activeMenu.GetInformation();
            DNSConverter dns = new DNSConverter(ip);
            if (dns.IP != "0")
            {
                networkingManager.Connect(new ClientNetworkManager(), dns.IP);
                JoinLobby();
                return true;
            }
            return false;
        }

        private void SwitchColor()
        {
            bannerManager.SetColor(TileGrid.ColorComparer((int)activeMenu.GetInformation()));

            if (activeMenu is CreateLobby || activeMenu is JoinLobby)
                bannerManager.OnColorSwitch(TileGrid.Player, (int)activeMenu.GetInformation(), TileGrid.ColorComparer(TileGrid.activeColor),bannerManager.LobbyBannerPos);
        }

        private void CloseConnection()
        {
            networkingManager.Disconnect();
        }

        private void CreateServer()
        {
            networkingManager.Connect(new ServerNetworkManager(), " ");
        }

        private void JoinLobby()
        {
            activeMenu = joinLobby;
            activeMenu.LoadFromDatabase();
        }

        public void QuitGame()
        {
            activeMenu = quitGame;
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            switch (MenuState)
            {
                case MenuStates.IDLE:
                    break;
                case MenuStates.MainMenu:
                    activeMenu = mainMenu;
                    break;
                case MenuStates.Profile:
                    bannerManager.SavePlayerInfo();
                    activeMenu = profile;
                    activeMenu.LoadFromDatabase();
                    break;
                case MenuStates.Play:
                    TileGrid.GameState = GameStates.Menu;
                    activeMenu = play;
                    break;
                case MenuStates.ExpansionLobby:
                    activeMenu = expansionLobby;
                    break;
                case MenuStates.CreateLobby:
                    TileGrid.GameState = GameStates.Lobby;
                    bannerManager.SavePlayerInfo();
                    bannerManager.AddPlayerLobbyBanner();
                    createLobby.SetTitle((string)activeMenu.GetInformation());
                    activeMenu = createLobby;
                    activeMenu.LoadFromDatabase();
                    CreateServer();
                    break;
                case MenuStates.JoinLobby:
                    TileGrid.GameState = GameStates.Lobby;
                    CreateClient();
                    break;
                case MenuStates.About:
                    activeMenu = about;
                    break;
                case MenuStates.ProfileNameUpdated:
                    bannerManager.SetName((string)activeMenu.GetInformation());
                    break;
                case MenuStates.UseSteamAPI:
                    bannerManager.LoadGameAvatar();
                    break;         
                case MenuStates.Color:
                    SwitchColor();
                    break;
                case MenuStates.Ready:
                    Ready = (bool) activeMenu.GetInformation();
                    break;
                case MenuStates.Disconnected:
                    CloseConnection();
                    activeMenu = mainMenu;
                    TileGrid.GameState = GameStates.Menu;
                    break;
                case MenuStates.Start:
                    if (bannerManager.CanStartGame())
                        TileGrid.GameState = GameStates.StartNewGame;
                    break;
                case MenuStates.Exit:
                    ExitGame = true;
                    break;
            }
            MenuState = activeMenu.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            activeMenu.Draw(spriteBatch);

            if (activeMenu != quitGame)
                spriteBatch.Draw(BackgroundTexture, BackgroundLocation, null, Color.White * 0.3f, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.9f);
        }
    }
}
