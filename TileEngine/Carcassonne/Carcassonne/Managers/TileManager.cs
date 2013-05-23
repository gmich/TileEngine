using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Carcassonne.Entities;

namespace Carcassonne.Managers
{
    using TileEngine.Entities.GameComponents;
    using TileEngine.Entities.MenuComponents;
    using TileEngine.Networking.Args;
    using TileEngine.Input;
    using TileEngine;

    public class TileManager
    {

        #region Declarations

        private List<MovingEntity> gridTiles;
        private List<MovingEntity> entities;
        private List<EntityInfo> entityInfo;
        private List<EntityTextures> entityTextures;
        private int counter;
        private enum TileState { MouseOver, Dragging, Free };
        private TileState tileState;
        private int? activeEntity, selectedEntityID;
        private const float layerStep = 0.001f;
        private IButton spawner;
        private TimedTitle title;
        private readonly DeckManager deckManager;
        private readonly BannerManager bannerManager;

        #endregion

        #region Event Declarations

        public event EventHandler<PlayArgs> PlayGame;
        public event EventHandler<UpdateEntityArgs> UpdateEntity;
        public event EventHandler<RequestEntityArgs> RequestTile;
        public event EventHandler<SignalEntityArgs> SnapTile;
        public event EventHandler<UpdateEntityArgs> ReleaseTile;
        public event EventHandler<ChangeEntityArgs> GetEntity;
        public event EventHandler<ChangeEntityArgs> DeleteEntity;
        public event EventHandler<RotateEntityArgs> RotateEntity;

        #endregion

        #region Constructor

        public TileManager(ContentManager Content,BannerManager bannerManager)
        {
            gridTiles = new List<MovingEntity>();
            entities = new List<MovingEntity>();
            entityInfo = new List<EntityInfo>();
            this.bannerManager = bannerManager;
            Font = Content.Load<SpriteFont>(@"Fonts\font");
            EntityFont = Content.Load<SpriteFont>(@"Fonts\entityFont");
            spawner = new ScoreButton(Content.Load<Texture2D>(@"Textures\Buttons\Spawner"),  Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(10, 10), " ", 0.1f);
            TileFrameTexture = Content.Load<Texture2D>(@"Textures\TileFrame");
            TextBackground = Content.Load<Texture2D>(@"Textures\Banners\TextBackground");
            EntityFrameTexture = Content.Load<Texture2D>(@"Textures\Soldiers\EntityFrame");

            AddEntityTextures(Content);
            tileState = TileState.Free;

            deckManager = new DeckManager(Content);
        }

        #endregion

        #region Properties

        private Texture2D TileFrameTexture
        {
            get;
            set;
        }

        private Texture2D TextBackground
        {
            get;
            set;
        }

        private Texture2D EntityFrameTexture
        {
            get;
            set;
        }
 
        private SpriteFont Font
        {
            get;
            set;
        }

        private SpriteFont EntityFont
        {
            get;
            set;
        }

        private Vector2 SoldierLocation(int x)
        {
            return new Vector2(Camera.ViewPortWidth - 220, 60 + x * 90);
        }

        private Rectangle SoldierRectangle(int x)
        {

            return new Rectangle(Camera.ViewPortWidth - 350, x * 90, 350,90);
        }

        private Vector2 LargeSoldierLocation(int x)
        {
            return new Vector2(Camera.ViewPortWidth - 165, 60 + x * 90);
        }

        private Rectangle LargeSoldierRectangle(int x)
        {
            return new Rectangle(Camera.ViewPortWidth - 350, x * 90, 350,90);
        }

        #endregion

        #region Texture Methods

        private void AddEntityTextures(ContentManager Content)
        {
            entityTextures = new List<EntityTextures>();

            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\Soldier1"),Content.Load<Texture2D>(@"Textures\Soldiers\SoldierFrame"),Color.Red,typeof(Soldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\Soldier2"), Content.Load<Texture2D>(@"Textures\Soldiers\SoldierFrame"), Color.Blue, typeof(Soldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\Soldier3"), Content.Load<Texture2D>(@"Textures\Soldiers\SoldierFrame"), Color.Green, typeof(Soldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\Soldier4"), Content.Load<Texture2D>(@"Textures\Soldiers\SoldierFrame"), Color.Yellow, typeof(Soldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldier1"), Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldierFrame"), Color.Red, typeof(LargeSoldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldier2"), Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldierFrame"), Color.Blue, typeof(LargeSoldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldier3"), Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldierFrame"), Color.Green, typeof(LargeSoldier)));
            entityTextures.Add(new EntityTextures(Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldier4"), Content.Load<Texture2D>(@"Textures\Soldiers\LargeSoldierFrame"), Color.Yellow, typeof(LargeSoldier)));   
        }

        #endregion

        #region Event Calls

        public void OnPlayGame(bool play)
        {
            EventHandler<PlayArgs> playGame = PlayGame;

            if (playGame != null)
                playGame(PlayGame, new PlayArgs(play));
        }

        public void OnRotateEntity(int tileID, bool rotation, string name, int color)
        {
            EventHandler<RotateEntityArgs> rotateEntity = RotateEntity;

            if (rotateEntity != null)
                rotateEntity(RotateEntity, new RotateEntityArgs(tileID, rotation, name, color));
        }

        public void OnSnapTile(int tileID, string name, int color)
        {
            EventHandler<SignalEntityArgs> snapTile = SnapTile;

            if (snapTile != null)
                snapTile(SnapTile, new SignalEntityArgs(tileID, name, color));
        }

        public void OnReleaseTile(int tileID, string name, int color, Vector2 location, float scale)
        {
            EventHandler<UpdateEntityArgs> releaseTile = ReleaseTile;

            if (releaseTile != null)
                releaseTile(ReleaseTile, new UpdateEntityArgs(tileID, name, color, location, scale));
        }

        public void OnRequestTile(int tileID, int color,int counter)
        {
            EventHandler<RequestEntityArgs> requestTile = RequestTile;

            if (requestTile != null)
                requestTile(RequestTile, new RequestEntityArgs(tileID, color,counter));
        }

        public void OnUpdateEntity()
        {
            EventHandler<UpdateEntityArgs> updateEntity = UpdateEntity;

            int? x = GetSelectedEntityListID();
            if (updateEntity != null && x!=null)
                updateEntity(updateEntity, new UpdateEntityArgs(entities[(int)x].ID,bannerManager.PlayerName,TileGrid.ColorComparer(TileGrid.activeColor),entities[(int)x].ForcedLocation,Camera.Scale));
        }

        public void OnUpdateEntity(int id, string player, int color, Vector2 location, float scale)
        {
            EventHandler<UpdateEntityArgs> updateEntity = UpdateEntity;

            if (updateEntity != null)
                updateEntity(updateEntity, new UpdateEntityArgs(id, player, color, location, scale));
        }

        public void OnGetEntity(int entityID, int infoID, int color)
        {
            EventHandler<ChangeEntityArgs> getEntity = GetEntity;

            if (getEntity != null)
                getEntity(GetEntity, new ChangeEntityArgs(entityID, infoID, color));
        }

        public void OnRemoveEntity(int entityID, int infoID, int color)
        {
            EventHandler<ChangeEntityArgs> removeEntity = DeleteEntity;

            if (removeEntity != null)
                removeEntity(DeleteEntity, new ChangeEntityArgs(entityID, infoID, color));
        }

        #endregion

        #region Event Handling

        public void SetLocation(int id,string name, Color color, Vector2 location,float scale)
        {
            foreach (MovingEntity entity in entities)
            {
                if (entity.ID == id && !entity.OnGrid)
                {
                    entity.IsUsingScreenCoordinates = false;
                    entity.HasBeenSelected = true;
                    entity.PlayerDragging = name;
                    entity.BackgroundColor = color;
                    entity.ForcedLocation = location * (Camera.Scale / scale);
                    return;
                }
            }
        }

        public void RotateEntities(int tileID, bool rotation, string name, Color color)
        {
            foreach (MovingEntity entity in entities)
            {
                if (entity.ID == tileID)
                {
                    entity.PlayerDragging = name;
                    entity.BackgroundColor = color;
                    entity.RotateLogic(rotation);
                    return;
                }
            }
        }

        public void RemoveEntity(int entityID, int infoID)
        {
            for (int i = 0; i < entities.Count; i++)
                if (entities[i].ID == entityID)
                {
                    entityInfo[infoID].AliveEntities++;
                    ResetBannerEntity(entities[i], infoID);
                    break;
                }
        }

        public void GotEntity(int infoID)
        {
            entityInfo[infoID].AliveEntities--;
        }        

        public void SnapEntity(int id)
        {
            for(int i=0;i<entities.Count;i++)
            {
                if (entities[i].ID == id)
                {
                    entities[i].SnapToGrid();
                    //TODO: review this call
                    SetEntityLayers(i);
                    return;
                }
            }
        }

        //TODO: change release entity server args
        public void ReleaseEntity(int id, string name,Color color,Vector2 location,float scale)
        {
            foreach (MovingEntity entity in entities)
            {
                if (entity.ID == id)
                {
                    entity.PlayerDragging = name;
                    entity.BackgroundColor = color;
                    //entity.ForcedLocation = location * (Camera.Scale / scale);
                    entity.ReleaseSquare();
                    return;
                }
            }
        }

        #endregion

        #region New Game

        //TODO: update to support multiple expansions
        public void NewGame()
        {
            TileGrid.GameState = GameStates.Playing;

            title = new TimedTitle(Font, new Vector2(0, 30), "", false, Color.Black);
            counter = 0;
            tileState = TileState.Free;
            selectedEntityID = activeEntity = null;
            entities.Clear();
            entityInfo.Clear();

            for (int i = 0; i < bannerManager.LobbyBannerCount; i++)
            {
                AddEntityInfo(new LargeSoldier(), bannerManager.BannerColor(i + 1), 1, LargeSoldierLocation(i), LargeSoldierRectangle(i));
                AddEntityInfo(new Soldier(), bannerManager.BannerColor(i + 1), 7,SoldierLocation(i), SoldierRectangle(i));
            }
            
            spawner.ButtonLocation = SpawnerLocation(bannerManager.LobbyBannerCount);
            bannerManager.NewGame();
            AddEntityInfoItems();

            TileGrid.ResetGrid();
            deckManager.NewGame();
            ShuffleDecks();
        }

        private void ShuffleDecks()
        {
                string[] expansions = TileGrid.Expansions.Split('/');

                if (expansions.Length == 3)
                {
                    ShuffleDecks(true, true, true);
                }
                else if (expansions.Length == 1)
                {
                    ShuffleDecks(true, false, false);
                }
                else
                {
                    //TODO: UPDATE
                    ShuffleDecks(true, false, false);

                }
        }

        private void ShuffleDecks(bool baseDeck,bool river,bool cathedrals)
        {
            if (baseDeck && river && cathedrals)
            {
                deckManager.ShuffleRiver(0);
                deckManager.ShuffleBase(1);
            }
            else
            {
                deckManager.ShuffleRiver(0);
            }
        }

        private Vector2 SpawnerLocation(int x)
        {
            return new Vector2(55, x * 90 + 54);
        }

        #endregion

        #region Entity Handling Methods

        public void AdjustLocation(float oldScale, float newScale)
        {
            foreach (MovingEntity entity in entities)
                entity.AdjustLocation(oldScale, newScale);
        }

        public void AdjustLocation()
        {
            foreach (MovingEntity entity in entities)
                entity.AdjustLocation();

            foreach (EntityInfo entityinfo in entityInfo)
                entityinfo.AdjustLocation();
        }

        private void EntityFadeOnMouseOver(int x)
        {
            for (int y = 0; y < entities.Count; y++)
            {
                if (x != y)
                {
                    if (entities[y].Equals(entities[x]) && !entities[y].OnGrid)
                        entities[y].Transparency = 0.5f;
                }
            }
        }

        private void SetEntityLayers(int x)
        {
            for (int y = 0; y < entities.Count; y++)
            {
                if (x != y)
                {
                    if (entities[y].ForcedLayer <= entities[x].ForcedLayer && !entities[y].IsUsingScreenCoordinates)
                        entities[y].ForcedLayer += layerStep;
                }
            }
            if (entities[x].GetType() == typeof(Tile))
                entities[x].ForcedLayer = 0.6f - (entities.Count - 1) * layerStep;
            else
                entities[x].ForcedLayer = 0.4f - (entities.Count - 1) * layerStep;
        }

        private void ResetEntityAlphaLevel()
        {
            for (int y = 0; y < entities.Count; y++)
            {
                entities[y].IsSolid = true;
            }
        }

        private void HandleSelection()
        {
            if (selectedEntityID != null)
            {
                foreach (MovingEntity entity in entities)
                    if (entity.ID == selectedEntityID)
                    {
                        entity.IsSelected = true;
                        entity.HandleEntityFeatures();
                        if (entity.RotateTo != null)
                            OnRotateEntity(entity.ID, (bool)entity.RotateTo, bannerManager.PlayerName, TileGrid.ColorComparer(TileGrid.activeColor));
                        break;
                    }
                if (InputManager.LeftButtonIsClicked() && tileState == TileState.Free)
                {
                    selectedEntityID = null;
                }
            }
        }

        #endregion

        #region Special Entities

        private void CheckEntityForRemoval()
        {
            int id = (int)GetSelectedEntityListID();

            OnSnapTile(entities[id].ID, TileGrid.Player, TileGrid.ColorComparer(TileGrid.activeColor));

            for (int i = 0; i < entityInfo.Count; i++)
            {

                if (entityInfo[i].Equals(entities[id])
                    && entityInfo[i].DestructionBoundaries.Intersects(entities[id].CollisionRectangle))
                {
                    entityInfo[i].AliveEntities++;
                    ResetBannerEntity(entities[id], i);
                    OnRemoveEntity((int)selectedEntityID, i, TileGrid.ColorComparer(TileGrid.activeColor));
                    selectedEntityID = activeEntity = null;
                    break;
                }
            }
        }

        private int? GetSelectedEntityListID()
        {
            for (int i = 0; i < entities.Count; i++)
                if (entities[i].ID == selectedEntityID)
                {
                    return i;
                }
            return null;
        }

        private void CheckEntityForForcedRemoval()
        {
            if ((InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Delete) || InputManager.MouseMiddleButtonClicked()) && selectedEntityID != null)
            {
                for (int i = 0; i < entityInfo.Count; i++)
                {
                    int id = (int)GetSelectedEntityListID();

                    if (entityInfo[i].Equals(entities[id]))
                    {
                        {
                            entityInfo[i].AliveEntities++;
                            ResetBannerEntity(entities[id],i);
                            OnRemoveEntity((int)selectedEntityID, i, TileGrid.ColorComparer(TileGrid.activeColor));
                            selectedEntityID = null;
                            tileState = TileState.Free;
                            break;
                        }
                    }
                }
            }
        }

        private void ResetBannerEntity(MovingEntity entity,int entityinfo)
        {
            entity.ForcedLocation = entityInfo[entityinfo].SpawnLocation;
            entity.ResetAll();
        }

        private void CheckSelectedEntity()
        {
            int id = (int)GetSelectedEntityListID();

            OnReleaseTile(entities[id].ID, TileGrid.Player, TileGrid.ColorComparer(TileGrid.activeColor),entities[id].ForcedLocation,Camera.Scale);

            for (int i = 0; i < entityInfo.Count; i++)
            {
                if (entityInfo[i].Equals(entities[id])
                    && !entities[id].HasBeenSelected
                    && entityInfo[i].DestructionBoundaries.Intersects(entities[id].CollisionRectangle))
                {
                    OnGetEntity(id, i, TileGrid.ColorComparer(TileGrid.activeColor));
                    entityInfo[i].AliveEntities--;
                    break;
                }
            }
        }

        #endregion

        #region Add / Remove

        public void AddEntityInfo(MovingEntity entity, Color color, int maxNo, Vector2 location, Rectangle boundaries)
        {
            entityInfo.Add(new EntityInfo(EntityFrameTexture,Font, entity, color, maxNo, location, boundaries));
        }

        public void AddEntityInfoItems()
        {
            foreach (EntityInfo entity in entityInfo)
            {
                for (int i = 0; i < entity.MaxEntities; i++)
                    AddEntity(entity.EntityType, entity.SpawnLocation, entity.Color);
            }
        }

        private void TileSpawner()
        {
            if (spawner.IsClicked() && title.TimePassed)
                AddTile();
        }

        private void AddTile()
        {
            if (IsTileSpawnerFree() && deckManager.CountAll>0)
            {
                int tileID = deckManager.GetNextTile();
                entities.Add(new Tile(++counter, deckManager.GetTileTexture(tileID), TileFrameTexture, TextBackground, EntityFont, Color.Black, spawner.ButtonLocation - new Vector2(100, 0), 0.6f));// - counter * layerStep));
                OnRequestTile(tileID, TileGrid.ColorComparer(TileGrid.activeColor),counter);
            }
        }

        public void AddTile(int tileID,int tileCounter)
        {
            if (tileCounter != counter)
            {
                deckManager.ValidateDecks();
                entities.Add(new Tile(++counter, deckManager.GetTileTexture(tileID), TileFrameTexture, TextBackground, EntityFont, Color.Black, spawner.ButtonLocation - new Vector2(100, 0), 0.6f));// - counter * layerStep));
                deckManager.RemoveTile(tileID);
            }
            else
            {
                tileState = TileState.Free;
                UndoTiles(tileCounter);
                title.SetText("draw one tile at a time :p ", 2000f);
            }
        }

        private void UndoTiles(int tileCounter)
        {
            int tempCounter = counter;

            while (tempCounter >= tileCounter)
            {
                if (RemoveTileAt(counter))
                {
                    deckManager.UndoTileRemoval();
                }
                tempCounter--;
            }
        }

        private bool RemoveTileAt(int tileID)
        {
            for (int i = 0; i < entities.Count; i++)
                if (entities[i].ID == tileID)
                {
                    entities.RemoveAt(i);
                    return true;
                }
            return false;
        }

        private bool IsTileSpawnerFree()
        {
            foreach (MovingEntity entity in entities)
                if (entity.IsUsingScreenCoordinates && entity.GetType() == typeof(Tile))
                    return false;
            return true;
        }

        private void AddLargeSoldier(Vector2 location, Color color)
        {
            int x = TileGrid.ColorComparer(color);
            x = x + TileGrid.MaxColors - 1;
            entities.Add(new LargeSoldier(++counter, entityTextures[x].Texture, entityTextures[x].Frame, TextBackground, EntityFont, color, location, 0.4f));// - counter * layerStep));
        }

        private void AddSoldier(Vector2 location, Color color)
        {
            int x = TileGrid.ColorComparer(color);
            x -= 1;
            entities.Add(new Soldier(++counter, entityTextures[x].Texture, entityTextures[x].Frame, TextBackground, EntityFont, color, location, 0.4f));// - counter * layerStep));
        }
        
        private void AddEntity(Type entity, Vector2 location, Color color)
        {
            if (entity == typeof(Soldier))
                AddSoldier(location, color);
            else if (entity == typeof(LargeSoldier))
                AddLargeSoldier(location, color);
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            spawner.SetFont(deckManager.CountAll.ToString());
            spawner.Update(gameTime);
            title.Update(gameTime);
            TileSpawner();
            HandleSelection();
            CheckEntityForForcedRemoval();

            switch (tileState)
            {
                case TileState.Free:
                    for (int x = 0; x < entities.Count; x++)
                    {
                        if (entities[x].Update(gameTime))
                        {
                            activeEntity = x;
                            tileState = TileState.MouseOver;
                            EntityFadeOnMouseOver(x);
                            return;
                        }
                    }
                    break;
                case TileState.MouseOver:
                    if (!entities[(int)activeEntity].Update(gameTime))
                    {
                        tileState = TileState.Free;
                        ResetEntityAlphaLevel();
                    }
                    else
                    {
                        if (entities[(int)activeEntity] is Tile)
                        {
                            for (int x = 0; x < entities.Count; x++)
                            {
                                if (!(entities[x] is Tile) && entities[x].MouseOver)
                                {
                                    entities[(int)activeEntity].mouseOver = false;
                                    activeEntity = x;
                                    EntityFadeOnMouseOver(x);
                                    break;
                                }
                            }
                        }
                        if (entities[(int)activeEntity].IsDragging)
                        {
                            tileState = TileState.Dragging;
                            selectedEntityID = entities[(int)activeEntity].ID;
                            CheckSelectedEntity();
                            ResetEntityAlphaLevel();
                        }
                    }
                    break;
                case TileState.Dragging:
                    entities[(int)activeEntity].Update(gameTime);
                    if (!entities[(int)activeEntity].IsDragging || entities[(int)activeEntity].ShowString)
                    {
                        entities[(int)activeEntity].IsDragging = false;
                        tileState = TileState.Free;
                        SetEntityLayers((int)activeEntity);
                        //for setLocation event race condition
                        if( !entities[(int)activeEntity].ShowString)
                            CheckEntityForRemoval();
                        else
                            title.SetText(" one at a time :p ", 2000f);
                        entities.Sort(new LayerComparer());
                    }
                    else
                        OnUpdateEntity();

                    break;
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (EntityInfo entityinfo in entityInfo)
                entityinfo.Draw(spriteBatch);

            foreach (MovingEntity entity in entities)
                entity.Draw(spriteBatch);

            spawner.Draw(spriteBatch);
            title.Draw(spriteBatch);
        }

        #endregion

    }
}
