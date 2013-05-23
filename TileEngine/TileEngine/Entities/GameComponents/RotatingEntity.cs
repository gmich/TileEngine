using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Input;

namespace TileEngine.Entities.GameComponents
{
    public class RotatingEntity : MovingEntity
    {
        #region Declarations

        private bool? clockwise;
        protected bool active;
        private static float rotationRate;
        private int rotationTicksRemaining;
        protected Stack<bool?> rotations;
        #endregion

        #region Constructors

        public RotatingEntity()
        {
            ;
        }

        public RotatingEntity(int ID,Texture2D bTexture, Texture2D sTexture, Texture2D tbTexture, SpriteFont font, Color color, Vector2 location, float layer)
            : base(ID, bTexture, sTexture, tbTexture,font, color, location, layer)
        {
            rotationRate = (MathHelper.PiOver2 / 10);
            rotationTicksRemaining = 10;
            active = false;
            clockwise = null;
        }

        #endregion

        #region Properties

        public override float Layer
        {
            get
            {
                if (IsUsingScreenCoordinates)
                    return 0.08f;
                else if (IsDragging || ShowString)
                    return 0.07f;
                else if (Active && OnGrid)
                    return layer + 0.1f;
                else if (OnGrid)
                    return layer + 0.2f;
                else
                    return layer;
            }
            set
            {
                layer = value;
            }
        }

        protected bool Active
        {
            get { return active; }
        }

        public override float RotationValue
        {
            get
            {
                if (Active)
                {
                    UpdateRotation();
                    if (clockwise==true)
                        rotationAmount += rotationRate;
                    else if (clockwise == false)
                        rotationAmount -= rotationRate;

                    return rotationAmount;
                }
                else
                    return rotationAmount;
            }
        }

        #endregion

        #region Public Methods

        public bool? HandleRotation()
        {
            if (!Active)
            {

                if ((InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Q)
                    || InputManager.ForwardButtonIsPressed()) && InputManager.MouseInBounds)
                    return true;
                else if ((InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.W)
                    || InputManager.BackButtonIsPressed()) && InputManager.MouseInBounds)
                    return false;
            }
            return null;
        }

        public void RotateEntity(bool? clockwise)
        {
            if (clockwise != null)
            {
                this.clockwise = clockwise;
                rotationTicksRemaining = 10;
                active = true;
            }
        }

        public void UpdateRotation()
        {
            rotationTicksRemaining = (int)MathHelper.Max(0, rotationTicksRemaining - 1);

            if (rotationTicksRemaining == 0)
                active = false;
        }

        #endregion
    }
}
