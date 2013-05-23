    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne.Menu
{
    public interface IMenu
    {
        object GetInformation();

        void LoadFromDatabase();

        void SetTitle(string Title);

        MenuStates Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
