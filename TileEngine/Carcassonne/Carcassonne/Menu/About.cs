using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;

namespace Carcassonne.Menu
{
    public class About : IMenu
    {
        List<IButton> buttons;
        CenteredTitle title;

        #region Constructor

        public About(ContentManager Content)
        {
            buttons = new List<IButton>();

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "About", false, Color.Black);
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(0, 180), "Back", 0.03f));
        }
            
        #endregion

        public object GetInformation()
        {
            return null;
        }

        public void SetTitle(string Title)
        {
            return;
        }

        public void LoadFromDatabase()
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
                return MenuStates.MainMenu;
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
