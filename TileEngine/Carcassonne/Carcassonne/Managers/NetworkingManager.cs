using System;
using TileEngine;
using TileEngine.Entities.MenuComponents;
using TileEngine.Networking;
using TileEngine.Networking.Messages;
using TileEngine.Networking.Args;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne.Managers
{
    public class NetworkingManager : IDisposable
    {
        private INetworkManager networkManager;
        private readonly BannerManager bannerManager;
        private readonly TileManager tileManager;
        private readonly AnimationManager animationManager;
        private Title title;
        private SpriteFont font;
        private float timeSinceMessage;
        private bool showMessage;

        #region Event Declarations

        public event EventHandler<BannerArgs> PlayerDisconnected;
        public event EventHandler<TitleArgs> InitiateTitle;
        
        #endregion

        #region Constructor

        public NetworkingManager(BannerManager bannerManager, TileManager tileManager,AnimationManager animationManager,SpriteFont font)
        {
            timeSinceMessage = 5.0f;
            showMessage = IsDisconnected = false;
            this.tileManager = tileManager;
            this.font = font;
            this.bannerManager = bannerManager;
            this.animationManager = animationManager;
            InitializeEvents();
        }

        #endregion

        #region Connect

        public void Connect(INetworkManager networkManager, string IP)
        {
            IsDisconnected = showMessage = false;
            SetTitle(" ");

            this.networkManager = networkManager;
            try
            {
                this.networkManager.Connect("Carcassonne", IP);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsDisconnected = true;
            }
        }

        public void Disconnect()
        {
            OnPlayerDisconnect(TileGrid.Player, TileGrid.ColorComparer(TileGrid.activeColor), bannerManager.LobbyBannerPos);

            bannerManager.RemoveLobbyBanners();

            if (TileGrid.GameState == GameStates.QuitGame)
                bannerManager.RemoveGameBanners();

            networkManager.Disconnect();
            IsDisconnected = true;
        }

        #endregion

        #region Initialize Events

        public void InitializeEvents()
        {
            bannerManager.RequestBanner += (sender, e) => networkManager.SendMessage(new RequestBannerMessage(e.Name, e.Color, e.IsUsingAvatar, e.Pos));
            bannerManager.PlayerBanner += (sender, e) => networkManager.SendMessage(new PlayerBannerMessage(e.Name, e.Color, e.IsUsingAvatar, e.Pos));
            bannerManager.AddBanner += (sender, e) => networkManager.SendMessage(new AddBannerMessage(e.Name, e.Color, e.IsUsingAvatar, e.Pos));
            bannerManager.ColorSwitch += (sender, e) => networkManager.SendMessage(new ColorSwitchMessage(e.Name, e.Color, e.OldColor, e.Pos));
            PlayerDisconnected += (sender, e) => networkManager.SendMessage(new PlayerDisconnectedMessage(e.Name, e.Color, e.IsUsingAvatar, e.Pos));
            InitiateTitle += (sender, e) => networkManager.SendMessage(new TitleMessage(e.Title, e.Show));
            tileManager.PlayGame += (sender, e) => networkManager.SendMessage(new InGameMessage(e.Play));
            tileManager.UpdateEntity += (sender, e) => networkManager.SendMessage(new UpdateEntityMessage(e.ID, e.Name, e.Color, e.Location, e.Scale));
            tileManager.RequestTile += (sender, e) => networkManager.SendMessage(new RequestTileMessage(e.TileID, e.Color, e.Counter));
            tileManager.SnapTile += (sender, e) => networkManager.SendMessage(new SnapTileMessage(e.ID, e.Name, e.Color));
            tileManager.ReleaseTile += (sender, e) => networkManager.SendMessage(new ReleaseTileMessage(e.ID, e.Name, e.Color, e.Location, e.Scale));
            tileManager.DeleteEntity += (sender, e) => networkManager.SendMessage(new RemoveEntityMessage(e.EntityID, e.InfoID, e.SenderColor));
            tileManager.GetEntity += (sender, e) => networkManager.SendMessage(new GetEntityMessage(e.EntityID, e.InfoID, e.SenderColor));
            tileManager.RotateEntity += (sender, e) => networkManager.SendMessage(new RotateEntityMessage(e.ID, e.Clockwise, e.Name, e.Color));
            bannerManager.ScoreUpdate += (sender, e) => networkManager.SendMessage(new UpdateScoreMessage(e.BannerID, e.Score, e.Color));
            animationManager.PointingAnimation += (sender, e) => networkManager.SendMessage(new AddPointingAnimationMessage(e.Location, e.Color,e.Scale));
        }

        #endregion

        #region Properties

        public bool IsDisconnected
        {
            get;
            private set;
        }

        private bool IsHost
        {
            get
            {
                return this.networkManager is ServerNetworkManager;
            }
        }

        private bool ShowMessage
        {
            get
            {
                return (timeSinceMessage < 4.0f || showMessage);
            }
            set
            {
                showMessage = value;
            }
        }

        #endregion

        #region Helper Methods

        private void SetTitle(string msg)
        {
            timeSinceMessage = 0.0f;
            title = new CenteredTitle(font, new Vector2(0, 140), msg, false, Color.Black);
        }
        
        #endregion

        #region Event Calls

        public void OnPlayerDisconnect(string name,int color,int pos)
        {
            EventHandler<BannerArgs> playerDisconnected = PlayerDisconnected;

            if (playerDisconnected != null)
                playerDisconnected(PlayerDisconnected, new BannerArgs(name, color, false,pos));
        }

        public void OnInitiateTitle(string title, bool show)
        {
            EventHandler<TitleArgs> initiateTitle = InitiateTitle;

            if (initiateTitle != null)
                initiateTitle(InitiateTitle, new TitleArgs(title, show));
        }

        #endregion

        #region Event Handling

        private void HandleAddPointingAnimationMessage(NetIncomingMessage im)
        {
            var message = new AddPointingAnimationMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                animationManager.AddPointingAnimation(message.Location * (Camera.Scale / message.Scale), TileGrid.ColorComparer(message.Color));

            if (this.IsHost)
                animationManager.OnAddPointingAnimation(message.Location, message.Color, message.Scale);
        }

        private void HandleUpdateScoreMessage(NetIncomingMessage im)
        {
            var message = new UpdateScoreMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                bannerManager.UpdateScore(message.BannerID, message.Score);

            if (this.IsHost)
                bannerManager.OnUpdateScore(message.BannerID, message.Score, message.Color);
        }

        private void HandleRotateEntityMessage(NetIncomingMessage im)
        {
            var message = new RotateEntityMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.RotateEntities(message.ID, message.Clockwise, message.Name, TileGrid.ColorComparer(message.Color));

            if (this.IsHost)
                tileManager.OnRotateEntity(message.ID, message.Clockwise, message.Name, message.Color);
        }

        private void HandleUpdateEntityMessage(NetIncomingMessage im)
        {
            var message = new UpdateEntityMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.SetLocation(message.ID, message.Name, TileGrid.ColorComparer(message.Color), message.Location, message.Scale);

            if (this.IsHost)
                tileManager.OnUpdateEntity(message.ID, message.Name, message.Color, message.Location, message.Scale);
        }

        private void HandleReleaseTileMessage(NetIncomingMessage im)
        {
            var message = new ReleaseTileMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.ReleaseEntity(message.ID, message.Name, TileGrid.ColorComparer(message.Color),message.Location,message.Scale);

            if (this.IsHost)
                tileManager.OnReleaseTile(message.ID, message.Name, message.Color,message.Location,message.Scale);
        }

        private void HandleGetEntityMessage(NetIncomingMessage im)
        {

            var message = new GetEntityMessage(im);

            if (message.SenderColor != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.GotEntity(message.InfoID);

            if (this.IsHost)
                tileManager.OnGetEntity(message.EntityID, message.InfoID, message.SenderColor);
        }

        private void HandleRemoveEntityMessage(NetIncomingMessage im)
        {

            var message = new RemoveEntityMessage(im);

            if (message.SenderColor != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.RemoveEntity(message.EntityID, message.InfoID);

            if (this.IsHost)
                tileManager.OnRemoveEntity(message.EntityID, message.InfoID, message.SenderColor);
        }

        private void HandleSnapTileMessage(NetIncomingMessage im)
        {
            var message = new SnapTileMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.SnapEntity(message.ID);

            if (this.IsHost)
                tileManager.OnSnapTile(message.ID, message.Name, message.Color);
        }

        private void HandleRequestTileMessage(NetIncomingMessage im)
        {
            var message = new RequestTileMessage(im);

            if (message.Color != TileGrid.ColorComparer(TileGrid.activeColor))
                tileManager.AddTile(message.TileID, message.Counter);

            if (this.IsHost)
                tileManager.OnRequestTile(message.TileID, message.Color, message.Counter);
        }

        private void HandleTitleMessage(NetIncomingMessage im)
        {
            var message = new TitleMessage(im);

            if (!this.IsHost)
            {
                ShowMessage = message.Show;
                SetTitle(message.Title);
                TileGrid.Expansions = message.Title;
            }  
        }

        private void HandlePlayGameMessage(NetIncomingMessage im)
        {
            var message = new InGameMessage(im);

            if (!this.IsHost)
            {
                if (message.Play)
                    tileManager.NewGame();
                else if (!message.Play && TileGrid.GameState == GameStates.Lobby)
                    Disconnect();
            }
        }
            

        private void HandlePlayerDisconnectedMessage(NetIncomingMessage im)
        {
            var message = new PlayerDisconnectedMessage(im);

            if (TileGrid.GameState == GameStates.Lobby)
            {
                bannerManager.RemoveLobbyBanner(message.Pos);
                if (this.IsHost)
                    OnPlayerDisconnect(message.Name, message.Color, message.Pos);
            }
        }

        private void HandleRequestBannerMessage(NetIncomingMessage im)
        {
            var message = new RequestBannerMessage(im);

            if (!(TileGrid.GameState == GameStates.Playing))
            {
                bannerManager.AddLobbyBanner(message.Name, TileGrid.ColorComparer(message.Color), message.IsUsingAvatar);
                bannerManager.OnPlayerBanner();
                if (this.IsHost)
                    bannerManager.OnSendAllBanners();
            }
        }

        private void HandleAddBannerMessage(NetIncomingMessage im)
        {     
            var message = new AddBannerMessage(im);

            bannerManager.AddLobbyBanner(message.Name, TileGrid.ColorComparer(message.Color), message.IsUsingAvatar,message.Pos);
        }

        private void HandlePlayerBannerMessage(NetIncomingMessage im)
        {
            var message = new PlayerBannerMessage(im);

            bannerManager.AddBannerID(message.Pos);
        }

        private void HandleColorSwitchMessage(NetIncomingMessage im)
        {
            var message = new ColorSwitchMessage(im);

            bannerManager.SetColor(TileGrid.ColorComparer(message.Color),message.Pos);

            if (this.IsHost)
                bannerManager.OnColorSwitch(message.Name, message.Color, message.OldColor,message.Pos);
        }

        #endregion

        private void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.networkManager.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(im.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                SetTitle(" ");
                                if (!this.IsHost)
                                {
                                    Console.WriteLine("Connected to {0}");
                                    bannerManager.SavePlayerInfo();
                                    bannerManager.OnRequestBanner(bannerManager.PlayerAvatarName, TileGrid.ColorComparer(TileGrid.activeColor), bannerManager.IsUsingAvatar);
                                }
                                else
                                {
                                    if (!(TileGrid.GameState == GameStates.Playing))
                                        OnInitiateTitle(TileGrid.Expansions, true);
                                    else
                                    {
                                        OnInitiateTitle("Disconnected: Game already in progress", true);
                                        tileManager.OnPlayGame(false);
                                    }
                                    Console.WriteLine("{0} Connected");
                                }
                                break;
                            case NetConnectionStatus.Disconnected:
                                if (!this.IsHost)
                                {
                                    bannerManager.RemoveLobbyBanners();
                                }
                                Console.WriteLine(
                                    this.IsHost ? "{0} Disconnected" : "Disconnected from {0}");
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                NetOutgoingMessage hailMessage = this.networkManager.CreateMessage();
                                im.SenderConnection.Approve(hailMessage);
                                break;
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        var gameMessageType = (GameMessageTypes)im.ReadByte();
                        switch (gameMessageType)
                        {
                            case GameMessageTypes.RequestBannerState:
                                this.HandleRequestBannerMessage(im);
                                break;
                            case GameMessageTypes.AddBannerState:
                                this.HandleAddBannerMessage(im);
                                break;
                            case GameMessageTypes.ColorSwitchState:
                                this.HandleColorSwitchMessage(im);
                                break;
                            case GameMessageTypes.PlayerBannerState:
                                this.HandlePlayerBannerMessage(im);
                                break;
                            case GameMessageTypes.PlayerDisconnectedState:
                                this.HandlePlayerDisconnectedMessage(im);
                                break;
                            case GameMessageTypes.SetTitleState:
                                this.HandleTitleMessage(im);
                                break;
                            case GameMessageTypes.InGameState:
                                this.HandlePlayGameMessage(im);
                                break;
                            case GameMessageTypes.UpdateEntityState:
                                this.HandleUpdateEntityMessage(im);
                                break;
                            case GameMessageTypes.RequestTileState:
                                this.HandleRequestTileMessage(im);
                                break;
                            case GameMessageTypes.SnapTileState:
                                this.HandleSnapTileMessage(im);
                                break;
                            case GameMessageTypes.ReleaseTileState:
                                this.HandleReleaseTileMessage(im);
                                break;
                            case GameMessageTypes.GetEntityState:
                                this.HandleGetEntityMessage(im);
                                break;
                            case GameMessageTypes.RemoveEntityState:
                                this.HandleRemoveEntityMessage(im);
                                break;
                            case GameMessageTypes.RotateEntityState:
                                this.HandleRotateEntityMessage(im);
                                break;
                            case GameMessageTypes.UpdateScoreState:
                                this.HandleUpdateScoreMessage(im);
                                break;
                            case GameMessageTypes.AddPointingAnimationState:
                                this.HandleAddPointingAnimationMessage(im);
                                break;
                        }
                        break;
                }
                this.networkManager.Recycle(im);
            }
        }

        #region Implement IDisposable

        public void Dispose()
        {
            Disconnect();
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            timeSinceMessage = MathHelper.Min(timeSinceMessage + (float)gameTime.ElapsedGameTime.Milliseconds, 2000.0f);
            ProcessNetworkMessages();
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ShowMessage)
                title.Draw(spriteBatch);
        }

        #endregion
    }
}
