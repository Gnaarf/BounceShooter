using SFML.Graphics;

namespace GameProject2D
{
    interface IGameState
    {
        GameState Update(float deltaTime);
        void Draw(RenderWindow win, View view, float deltaTime);
        void DrawGUI(GUI gui, float deltaTime);
    }
}
