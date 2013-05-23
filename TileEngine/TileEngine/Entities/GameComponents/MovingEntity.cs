using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Input;
using TileEngine;

namespace TileEngine.Entities.GameComponents
{
    public abstract class MovingEntity : IEquatable<MovingEntity>
    {
        #region Declarations

        const float textShowingTime = 500.0f;
        private Stopwatch stopwatch;
        public bool mouseOver;
        protected Vector2 location;
        protected float rotationAmount;
        protected float layer;
        private Vector2 initialLoc;
        private string playerDragging;
        private float transparency;

        #endregion

        #region Constructor

        public MovingEntity()
        {
            ;
        }

        public MovingEntity(int ID,Texture2D bTexture, Texture2D sTexture, Texture2D textBackground, SpriteFont font, Color color, Vector2 location, float layer)
        {
            stopwatch = new Stopwatch();
            this.TextBackground = textBackground;
            this.Texture = bTexture;
            this.SelectedTexture = sTexture;
            this.Font = font;
            this.Color = color;
            this.location = location;
            this.Layer = layer;
            this.RotationValue = 0.0f;
            HasBeenSelected = IsSelected = IsDragging = mouseOver = OnGrid = IsLocked = false;
            IsSolid = IsUsingScreenCoordinates = true;
            Transparency = 1.0f;
            this.ID = ID;
        }

        #endregion

        #region Properties

        #region Moving

        public bool IsLocked
        {
            get;
            set;
        }

        public bool IsDragging
        {
            get;
            set;
        }

        public bool IsUsingScreenCoordinates
        {
            get;
            set;
        }

        #endregion

        #region Texture/Font

        Texture2D Texture
        {
            get;
            set;
        }

        Texture2D TextBackground
        {
            get;
            set;
        }

        Texture2D SelectedTexture
        {
            get;
            set;
        }

        SpriteFont Font
        {
            get;
            set;
        }

        #endregion

        #region Location/Size

        protected Vector2 Origin
        {
            get
            {
                return new Vector2(Texture.Width / 2, Texture.Width / 2);
            }
        }

        public virtual int Width
        {
            get
            {
                return (int)(Texture.Width * Scale);
            }
        }

        public virtual int Height
        {
            get
            {
                 return (int)(Texture.Height * Scale);
            }
        }

        private Vector2 FontLocation
        {
            get
            {
                return new Vector2(Location.X - (FontSize.X / 2), Location.Y - (Texture.Height * Camera.Scale ) /2  - FontSize.Y);
            }
        }

        Vector2 FontSize
        {
            get
            {
                return Font.MeasureString(PlayerDragging);
            }
        }

        private float OffSetX
        {
            get;
            set;
        }

        Rectangle TextBackgroundRectangle
        {
            get
            {
                Vector2 textLocation = Camera.VectorWorldToScreen(FontLocation);
                return new Rectangle((int)textLocation.X, (int)textLocation.Y, (int)FontSize.X, (int)FontSize.Y);
            }
        }

        public Vector2 ForcedLocation
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        protected Vector2 Location
        {
            get
            {
                if (IsUsingScreenCoordinates)
                {
                    OffSetX = Camera.ViewPortWidth - location.X;
                    return location + Camera.Position;
                }
                else
                    return location;

            }
            set
            {
                location = Camera.AdjustInWorldBounds(value, Width, Height,Origin*Scale);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(Location.X - Origin.X * Scale), (int)(Location.Y - Origin.Y * Scale), Width, Height);
            }
        }

        #endregion

        #region Information

        public bool? RotateTo
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public int ID
        {
            get;
            private set;
        }

        public bool OnGrid
        {
            get;
            set;
        }

        public bool HasBeenSelected
        {
            get;
            set;
        }

        public float Transparency
        {
            get
            {
                if (mouseOver || IsDragging)
                    transparency = 1.0f;
                else if (IsSolid)
                    transparency = MathHelper.Min(transparency + 0.05f, 1.0f);

                return transparency;
            }
            set
            {
                transparency = value;
                IsSolid = false;
            }
        }

        public bool IsSolid
        {
            get;
            set;
        }

        private float Scale
        {
            get
            {
                if (!IsUsingScreenCoordinates)
                    return Camera.Scale;
                else
                    return 1.0f;
            }

        }

        private bool Draggable
        {
            get;
            set;
        }

        public bool ShowString
        {
            get
            {
                if (stopwatch.IsRunning)
                {
                    if (stopwatch.ElapsedMilliseconds > textShowingTime)
                    {
                        stopwatch.Stop();
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public virtual float RotationValue
        {
            get
            {
                return rotationAmount;
            }
            set
            {
                rotationAmount = value;
            }
        }

        public Color Color
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }


        public float ForcedLayer
        {
            get
            {
                return layer;
            }
            set
            {
                layer = value;
            }
        }

        public virtual float Layer
        {
            get
            {
                if (IsUsingScreenCoordinates)
                    return 0.05f;
                else if (IsDragging || ShowString)
                    return 0.07f;
                else if (OnGrid)
                    return layer + 0.3f;
                else
                    return layer;
            }
            set
            {
                layer = value;
            }
        }

        public string PlayerDragging
        {
            get
            {
                return playerDragging;
            }
            set
            {
                playerDragging = value;
                stopwatch.Reset();
                stopwatch.Start();
            }
        }

        #endregion

        #endregion

        #region Dragging

        public bool MouseOver
        {
            get
            {
                mouseOver=(CollisionRectangle.Intersects(InputManager.MouseWorldRectangle));
                return mouseOver;
            }
        }

        private void CheckForClicks()
        {
            if (!IsDragging)
            {
                if (!MouseOver && InputManager.LeftButtonIsClicked())
                    Draggable = false;
                else if (!InputManager.LeftButtonIsClicked())
                    Draggable = true;

                if (MouseOver && InputManager.LeftButtonIsClicked() && Draggable)
                {
                    if (IsUsingScreenCoordinates)
                    {
                        IsUsingScreenCoordinates = false;
                        ResetLocation();
                    }
                    IsDragging = true;
                    ReleaseSquare();
                    initialLoc = InputManager.MouseWorldPosition;
                }
            }
        }

        private void ResetLocation()
        {
            if (!CollisionRectangle.Intersects(InputManager.MouseRectangle))
                Location = InputManager.MousePosition + Camera.Position;
            else
                Location = Location + Camera.Position;
        }

        public bool IsReleased()
        {
            if (!InputManager.LeftButtonIsClicked() && IsDragging)
            {
                IsDragging = false;
                SnapToGrid();
                HasBeenSelected = true;
                return true;
            }
            return false;
        }

        private void Drag()
        {
            if (IsDragging)
            {
                Move(InputManager.MouseWorldPosition-initialLoc);
                initialLoc = InputManager.MouseWorldPosition;
            }
        }

        public void Move(Vector2 moveAmount)
        {
            Location += moveAmount;
        }

        public virtual void SnapToGrid()
        {
            return;
        }

        public virtual void ReleaseSquare()
        {
            return;
        }

        public virtual void ResetAll()
        {
            IsUsingScreenCoordinates = true;
            HasBeenSelected = false;
        }

        #endregion

        #region Implement IEquatable

        public bool Equals(MovingEntity other)
        {
            return (CollisionRectangle.Intersects(other.CollisionRectangle)
                && other.Layer > Layer);
        }

        #endregion

        #region Methods

        public void AdjustLocation(float oldScale, float newScale)
        {
            if (!IsDragging && !IsUsingScreenCoordinates)
                location *= (newScale / oldScale);
        }

        public void AdjustLocation()
        {
            if (IsUsingScreenCoordinates)
                location.X = Camera.ViewPortWidth - OffSetX;
        }

        public virtual void HandleEntityFeatures()
        {
            //TODO: add entity selected feature here
        }

        public virtual void RotateLogic(bool? rotation)
        {
            return;
        }

        public virtual bool Update(GameTime gameTime)
        {
            if(!ShowString)
                CheckForClicks();
            IsReleased();
            Drag();
            RotateLogic(null);
            return (MouseOver && !ShowString);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Camera.ObjectIsVisible(CollisionRectangle))
            {
                spriteBatch.Draw(Texture, Camera.WorldToScreen(Location), null, Color.White * Transparency, RotationValue, Origin, Scale, SpriteEffects.None, Layer);

                if (mouseOver || IsDragging)
                    spriteBatch.Draw(SelectedTexture, Camera.WorldToScreen(Location), null, Color.White * Transparency, rotationAmount, Origin, Scale, SpriteEffects.None, Layer - 0.0001f);
                else if (IsSelected)
                {
                    spriteBatch.Draw(SelectedTexture, Camera.WorldToScreen(Location), null, Color.White * 0.5f, rotationAmount, Origin, Scale, SpriteEffects.None, Layer - 0.0002f);
                    IsSelected = false;
                }
                if (ShowString)
                {
                    spriteBatch.Draw(TextBackground, TextBackgroundRectangle, null, BackgroundColor * 0.4f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.01f);
                    spriteBatch.DrawString(Font, PlayerDragging, Camera.VectorWorldToScreen(FontLocation), Color.Black);
                }
            }
            //TODO : review code
                    //on object is not on screen, rotation is on hold
            /*
            else
            {
                RotationValue;
            }
            */
        }

        #endregion
    }
}
