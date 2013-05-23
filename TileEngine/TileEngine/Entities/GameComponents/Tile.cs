using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.GameComponents
{
    public class Tile : RotatingEntity
    {

        #region Constructor

        public Tile(int ID, Texture2D bTexture, Texture2D sTexture, Texture2D tbTexture, SpriteFont font, Color color, Vector2 location, float layer)
            : base(ID,bTexture, sTexture, tbTexture, font, color, location, layer)
        {
            rotations = new Stack<bool?>();
        }

        #endregion

        #region Grid Methods

        public override void ReleaseSquare()
        {
            if (OnGrid)
            {
                TileGrid.OccupySquareAt(location, false);
                OnGrid = false;
            }
        }

        public override void SnapToGrid()
        {
            if (!TileGrid.SquareIsOccupied(Location))
            {
                location = TileGrid.GetSquareLocation(Location) + Origin * Camera.Scale;
                TileGrid.OccupySquareAt(location, true);
                OnGrid = true;
            }
            else
                OnGrid = false;
        }

        #endregion

        #region Override Feature

        public override void HandleEntityFeatures()
        {
            if (!ShowString)
            {
                RotateTo = HandleRotation();
                RotateLogic(HandleRotation());
            }
        }

        public override void RotateLogic(bool? rotation)
        {
            if (rotation != null)
                rotations.Push(rotation);

            if (!Active && rotations.Count > 0 )
            {
                RotateEntity(rotations.Pop());
            }
        }

        #endregion

        #region Override Update

        public override bool Update(GameTime gameTime)
        {
            return base.Update(gameTime);
        }

        #endregion
    }
}
