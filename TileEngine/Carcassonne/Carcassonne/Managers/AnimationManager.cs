using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TileEngine;
using TileEngine.Input;
using TileEngine.Animations;
using TileEngine.Networking.Args;

namespace Carcassonne.Managers
{
    public class AnimationManager
    {
        #region Declarations

        private List<IAnimation> animations;
        public event EventHandler<AnimationArgs> PointingAnimation;

        #endregion

        #region Constructor

        public AnimationManager(ContentManager Content)
        {
            animations = new List<IAnimation>();

            CircleTexture = Content.Load<Texture2D>(@"Textures\Animations\Circle");
        }

        #endregion

        #region Properties

        Texture2D CircleTexture
        {
            get;
            set;
        }

        #endregion

        #region Event Calls

        public void OnAddPointingAnimation(Vector2 location, int color, float scale)
        {
            EventHandler<AnimationArgs> pointingAnimation = PointingAnimation;

            if (pointingAnimation != null)
                pointingAnimation(PointingAnimation, new AnimationArgs(location, color, scale));
        }

        #endregion

        #region Add Animations

        public void AddPointingAnimation(Vector2 location, Color color)
        {
            animations.Add(new PointingAnimation(CircleTexture, location, color));
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {

            if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.LeftControl) && InputManager.MouseInBounds)
            {
                AddPointingAnimation(InputManager.MousePosition + Camera.Position, TileGrid.activeColor);
                OnAddPointingAnimation(InputManager.MousePosition + Camera.Position, TileGrid.ColorComparer(TileGrid.activeColor),Camera.Scale);
            }
   
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Alive)
                    animations[i].Update(gameTime);
                else
                    animations.RemoveAt(i);
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IAnimation animation in animations)
                animation.Draw(spriteBatch);            
        }

        #endregion
    }
}
