using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;
using TileEngine;

namespace Carcassonne.Menu
{
    public class CreateLobby : IMenu
    {
        List<IButton> buttons;
        CenteredTitle title;
        SetColor setColor;
        Object information;

        #region Constructor

        public CreateLobby(ContentManager Content)
        {
            buttons = new List<IButton>();

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "Create Lobby", false, Color.Black);
            setColor = new SetColor(Content, new Vector2(50, 180));
            buttons.Add(new GameButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 280), "Begin", 0.03f));
            buttons.Add(new GameButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 380), "Disconnect", 0.03f));
        
        }

        #endregion

        public object GetInformation()
        {
            return information;
        }

        public void SetTitle(string Title)
        {
            title.Text = Title;
            TileGrid.Expansions = Title;
        }

        public void LoadFromDatabase()
        {
            setColor.LoadFromDatabase();
        }

        private void SetButtonLocation()
        {
            buttons[0].ButtonLocation = new Vector2(Camera.ViewPortWidth / 2 + 152, 225);
            buttons[1].ButtonLocation = new Vector2(Camera.ViewPortWidth / 2 - 152, 225);
        }

        public MenuStates Update(GameTime gameTime)
        {
            SetButtonLocation();

            foreach (IButton button in buttons)
                button.Update(gameTime);

            if (setColor.Update(gameTime) == MenuStates.Color)
            {
                information = setColor.GetInformation();
                return MenuStates.Color;
            }
            if (buttons[0].IsClicked())
            {
                buttons[0].OffSet = 0;
                return MenuStates.Start;
            }
            if (buttons[1].IsClicked())
            {
                buttons[0].OffSet = 0;
                return MenuStates.Disconnected;
            }
            return MenuStates.IDLE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);
            setColor.Draw(spriteBatch);

            foreach (IButton button in buttons)
                button.Draw(spriteBatch);

        }
    }
}
