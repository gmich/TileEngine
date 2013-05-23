using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;

namespace Carcassonne.Menu
{
    public class MainMenu : IMenu
    {
        List<IButton> buttons;
        CenteredTitle title;

        #region Constructor

        public MainMenu(ContentManager Content)
        {
            buttons = new List<IButton>();

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "Main Menu", false, Color.Black);
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 180), "Play", 0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 270), "Profile", 0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 360), "About", 0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 450), "Exit", 0.03f));
        }

        #endregion

        public object GetInformation()
        {
            return null;
        }

        public void LoadFromDatabase()
        {
            ;
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
                buttons[0].OffSet = 0;
                return MenuStates.Play;
            }
            if (buttons[1].IsClicked())
            {
                buttons[1].OffSet = 0;
                return MenuStates.Profile;
            }
            if (buttons[2].IsClicked())
            {
                buttons[2].OffSet = 0;
                return MenuStates.About;
            }
            if (buttons[3].IsClicked())
                return MenuStates.Exit;

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
