using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;
using TileEngine.Input;
using TileEngine;
using Carcassonne.Configuration;

namespace Carcassonne.Menu
{
    public class SetColor : IMenu
    {
        List<IButton> buttons;
        int colorState, maxColorState;
        Object information;

        #region Constructor

        public SetColor(ContentManager Content,Vector2 location)
        {
            buttons = new List<IButton>();
            buttons.Add(new ColorButton(Content.Load<Texture2D>(@"Textures\Buttons\BlankButton"), Content.Load<SpriteFont>(@"Fonts\font"),location, " ", 0.05f));
            buttons[0].SetFontColor(Color.White);
            maxColorState = 4;
            LoadFromDatabase();
        }

        #endregion

        #region Helper Methods

        public Vector2 Location
        {
            get
            {
                return buttons[0].ButtonLocation;
            }
        }

        public void LoadFromDatabase()
        {
            PlayerConfiguration configuration = new PlayerConfiguration();
            colorState = configuration.ColorID;
            buttons[0].SetFontColor(TileGrid.ColorComparer(configuration.ColorID));
        }

        private int NextColorState
        {
            get
            {
                colorState += 1;
                if (colorState > maxColorState)
                    colorState = 1;
                return colorState;
            }
        }

        #endregion

        public object GetInformation()
        {
            return information;
        }

        public void SetTitle(string Title)
        {
            return;
        }

        public MenuStates Update(GameTime gameTime)
        {
            foreach (IButton button in buttons)
                button.Update(gameTime);

            if (buttons[0].IsClicked())
            {
                int colorID = NextColorState;
                information = (object)colorID;
                buttons[0].SetFontColor(TileGrid.ColorComparer(colorID));
                return MenuStates.Color;
            }

            return MenuStates.IDLE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IButton button in buttons)
                button.Draw(spriteBatch);

        }
    }
}
