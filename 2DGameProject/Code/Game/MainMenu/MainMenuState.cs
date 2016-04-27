using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    class MainMenuState : IGameState
    {
        Sprite background;

        public MainMenuState()
        {
            background = new Sprite(new Texture("Textures/MainMenu_Background.jpg"));
        }

        public GameState Update(float deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
            {
                return GameState.InGame;
            }

            return GameState.MainMenu;
        }

        public void Draw(RenderWindow win, View view, float deltaTime)
        {
            //win.Draw(background);
        }

        public void DrawGUI(GUI gui, float deltaTime)
        {
            gui.Draw(background);
        }
    }
}
