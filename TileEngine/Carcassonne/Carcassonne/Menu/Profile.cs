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
    public class Profile : IMenu
    {
        List<IButton> buttons;
        TextBox textBox;
        Object information;
        CenteredTitle title;
        SetColor setColor;

        #region Constructor

        public Profile(ContentManager Content)
        {
            buttons = new List<IButton>();

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "Profile", false, Color.Black);
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"),Content.Load<SpriteFont>(@"Fonts\font"),new Vector2(50,270),"Load Avatar",0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 360), "Back", 0.03f));
            textBox = new TextBox(Content.Load<SpriteFont>(@"Fonts\font"),Content.Load<Texture2D>(@"Textures\Buttons\layout"),Vector2.Zero,200,30);
            setColor = new SetColor(Content, new Vector2(50, 180));
            SetTextBoxLocation();
            LoadInfoFromDatabase();
        }

        #endregion

        #region Helper Methods

        private void LoadInfoFromDatabase()
        {
            PlayerConfiguration configuration = new PlayerConfiguration();
            textBox.Text = configuration.Name;
        }

        public void SetTitle(string Title)
        {
            return;
        }

        private void SetTextBoxLocation()
        {
            textBox.Location = buttons[0].ButtonLocation + new Vector2(76,-15);
        }

        public void LoadFromDatabase()
        {
            setColor.LoadFromDatabase();
        }

        #endregion

        public object GetInformation()
        {
            return information;
        }

        public MenuStates Update(GameTime gameTime)
        {
            foreach (IButton button in buttons)
                button.Update(gameTime);

            SetTextBoxLocation();

            if (buttons[0].IsClicked())
                return MenuStates.UseSteamAPI;
            if (setColor.Update(gameTime) == MenuStates.Color)
            {
                information = setColor.GetInformation();
                return MenuStates.Color;
            }
            if (buttons[1].IsClicked())
            {
                buttons[1].OffSet = 0;
                return MenuStates.MainMenu;
            }
            textBox.Update(gameTime);

            if (textBox.IsTextBoxUpdated())
            {
                information = (object)textBox.Text;
                return MenuStates.ProfileNameUpdated;
            }

            return MenuStates.IDLE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);
            setColor.Draw(spriteBatch);

            foreach (IButton button in buttons)
                button.Draw(spriteBatch);

            textBox.Draw(spriteBatch);
        }
    }
}
