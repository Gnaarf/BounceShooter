using System;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    class InGameState : IGameState
    {
        Map map;
        
        public InGameState()
        {
            map = new Map();
        }

        public GameState Update(float deltaTime)
        {
            map.update(deltaTime);
            return GameState.InGame;
        }

        public void Draw(RenderWindow win, View view, float deltaTime)
        {
            map.draw(win, view);
            map.debugDraw(win, view);
        }

        public void DrawGUI(GUI gui, float deltaTime)
        {
        }
    }
}
