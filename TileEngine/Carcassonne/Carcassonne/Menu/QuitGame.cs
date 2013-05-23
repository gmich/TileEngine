using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;
using TileEngine;

namespace Carcassonne.Menu
{
    public class QuitGame : IMenu
    {
        List<IButton> buttons;
        CenteredTitle title;

        #region Constructor

        public QuitGame(ContentManager Content)
        {
            buttons = new List<IButton>();

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "Disconnect", false, Color.Black);
            buttons.Add(new GameButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 280), "Yes", 0.03f));
            buttons.Add(new GameButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 380), "No", 0.03f));     
        }

        #endregion

        public object GetInformation()
        {
            return (object)null;
        }

        public void SetTitle(string Title)
        {
            title.Text = Title;
        }

        public void LoadFromDatabase()
        {
            return;
        }

        private void SetButtonLocation()
        {
            buttons[0].ButtonLocation = new Vector2(Camera.ViewPortWidth / 2 + 76, 225);
            buttons[1].ButtonLocation = new Vector2(Camera.ViewPortWidth / 2 - 76, 225);
        }

        public MenuStates Update(GameTime gameTime)
        {
            SetButtonLocation();

            foreach (IButton button in buttons)
                button.Update(gameTime);

            if (buttons[0].IsClicked())
            {
                buttons[0].OffSet = 0;
                return MenuStates.Disconnected;
            }
            if (buttons[1].IsClicked())
            {
                buttons[1].OffSet = 0;
                TileGrid.GameState = GameStates.Playing;
            }

            return MenuStates.IDLE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);

            foreach (IButton button in buttons)
                button.Draw(spriteBatch);
        }
    }
}
