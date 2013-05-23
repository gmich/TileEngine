using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;
using TileEngine;

namespace Carcassonne.Menu
{
    public class ExpansionLobby : IMenu
    {
        List<IButton> buttons;
        CenteredTitle title, expansionTitle;
        Object information;

        #region Constructor

        public ExpansionLobby(ContentManager Content)
        {
            buttons = new List<IButton>();
            River = InnsCathedrals = true;
            Selected = Content.Load<Texture2D>(@"Textures\Buttons\selectedButton");
            NotSelected = Content.Load<Texture2D>(@"Textures\Buttons\NotSelectedButton");

            title = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\largeFont"), new Vector2(0, 100), "Expansions", false, Color.Black);

            buttons.Add(new GameButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 0), "Create", 0.03f));
            buttons.Add(new GameButton(Content.Load<Texture2D>(@"Textures\Buttons\Button1"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 0), "Back", 0.03f));
            buttons.Add(new MenuButton(Content.Load<Texture2D>(@"Textures\Buttons\NeutralButton"), Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 270), "Base", 0.03f));
            buttons.Add(new MenuButton(Selected, Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 327), "River", 0.03f));
            buttons.Add(new MenuButton(Selected, Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(50, 384), "  Inns & Cathedrals", 0.03f));
            expansionTitle = new CenteredTitle(Content.Load<SpriteFont>(@"Fonts\font"), new Vector2(0, 150), " ", false, Color.Black);
        }

        #endregion

        #region Properties

        Texture2D Selected
        {
            get;
            set;
        }

        Texture2D NotSelected
        {
            get;
            set;
        }

        bool BaseGame
        {
            get { return true; }
        }

        bool River
        {
            get;
            set;
        }

        bool InnsCathedrals
        {
            get;
            set;
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

        public void LoadFromDatabase()
        {
            return;
        }

        private void SetButtonTextures()
        {
            if (River)
                buttons[3].Texture = Selected;
            else if (!River)
                buttons[3].Texture = NotSelected;

            if (InnsCathedrals)
                buttons[4].Texture = Selected;
            else if (!InnsCathedrals)
                buttons[4].Texture = NotSelected;
        }

        private string GetSelectedExpansions()
        {
            string expansions = "Base ";
            if (River)
                expansions += "/ River ";
            if(InnsCathedrals)
                expansions += "/ Inns & Cathedrals ";

            return expansions;
        }

        private void SetButtonLocation()
        {
            buttons[0].ButtonLocation = new Vector2(Camera.ViewPortWidth / 2 + 76, 225);
            buttons[1].ButtonLocation = new Vector2(Camera.ViewPortWidth / 2 - 76, 225);
        }

        public MenuStates Update(GameTime gameTime)
        {
            SetButtonLocation();
            SetButtonTextures();
            expansionTitle.Text = GetSelectedExpansions();
            foreach (IButton button in buttons)
                button.Update(gameTime);

            if (buttons[0].IsClicked())
            {
                buttons[0].OffSet = 0;
                information = (object)GetSelectedExpansions();
                return MenuStates.CreateLobby;
            }
            if (buttons[1].IsClicked())
            {
                buttons[1].OffSet = 0;
                return MenuStates.Play;
            }
            if (buttons[3].IsClicked())
                River = !River;
            if (buttons[4].IsClicked())
                InnsCathedrals = !InnsCathedrals;

            return MenuStates.IDLE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);
            expansionTitle.Draw(spriteBatch);

            foreach (IButton button in buttons)
                button.Draw(spriteBatch);

        }
    }
}
