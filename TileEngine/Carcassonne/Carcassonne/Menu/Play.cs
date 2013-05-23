using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;
using TileEngine.Input;

namespace Carcassonne.Menu
{
    public class Play : IMenu
    {
        List<IButton> buttons;
        TextBox textBox;
        CenteredTitle title;

        #region Constructor

        public Play(ContentManager Content)
        {
            buttons = new List<IButton>();

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "Play", false, Color.Black);
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 180), "Create Lobby", 0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 270), "Join Lobby", 0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 360), "Back", 0.03f));
            textBox = new TextBox(Content.Load<SpriteFont>(@"Fonts\font"), Content.Load<Texture2D>(@"Textures\Buttons\layout"), new Vector2(150, 220), 200, 30);
            //TODO: REMOVE
            textBox.Text = "127.0.0.1";
            SetTextBoxLocation();
        }

        #endregion

        private void SetTextBoxLocation()
        {
            textBox.Location = buttons[1].ButtonLocation + new Vector2(76, -10);
        }

        public void LoadFromDatabase()
        {
            return;
        }

        public object GetInformation()
        {
            return (object)textBox.Text;
        }

        public void SetTitle(string Title)
        {
            return;
        }

        public MenuStates Update(GameTime gameTime)
        {
            foreach (IButton button in buttons)
                button.Update(gameTime);

            SetTextBoxLocation();

            if (buttons[0].IsClicked())
            {
                buttons[0].OffSet = 0;
                return MenuStates.ExpansionLobby;
            }
            if (buttons[1].IsClicked())
            {
                buttons[1].OffSet = 0;
                return MenuStates.JoinLobby;
            }
            if (buttons[2].IsClicked())
            {
                buttons[2].OffSet = 0;
                return MenuStates.MainMenu;
            }
            textBox.Update(gameTime);

            return MenuStates.IDLE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);

            foreach (IButton button in buttons)
                button.Draw(spriteBatch);

            textBox.Draw(spriteBatch);
        }
    }
}
