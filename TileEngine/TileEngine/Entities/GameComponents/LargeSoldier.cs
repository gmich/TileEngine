using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Input;

namespace TileEngine.Entities.GameComponents
{
    public class LargeSoldier : RotatingEntity
    {
        #region Declarations

        private enum RotateState { Fallen, Standing } ;
        private RotateState rotateState;

        #endregion

        #region Constructors

        public LargeSoldier()
        {
            ;
        }

        public LargeSoldier(int ID, Texture2D bTexture, Texture2D sTexture,Texture2D tbTexture, SpriteFont font, Color color, Vector2 location, float layer)
            : base(ID, bTexture, sTexture, tbTexture, font, color, location, layer)
        {
            rotateState = RotateState.Standing;
            rotations = new Stack<bool?>();
        }

        #endregion

        #region Override Rotation

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
                bool rotationToTest = (bool)rotations.Pop();

                if (rotationToTest == true && rotateState == RotateState.Standing)
                {
                    RotateEntity(true);
                    rotateState = RotateState.Fallen;
                }
                else if (rotationToTest == true && rotateState == RotateState.Fallen)
                {
                    RotateEntity(false);
                    rotateState = RotateState.Standing;
                }
                else if (rotationToTest == false && rotateState == RotateState.Fallen)
                {
                    RotateEntity(false);
                    rotateState = RotateState.Standing;
                }
                else if (rotationToTest == false && rotateState == RotateState.Standing)
                {
                    RotateEntity(true);
                    rotateState = RotateState.Fallen;
                }
            }
        }

        #endregion

        #region Override Reset

        public override void ResetAll()
        {
            rotateState = RotateState.Standing;
            rotations = new Stack<bool?>();
            active = false;
            RotationValue = 0.0f;

            base.ResetAll();
        }

        #endregion
    }
}
