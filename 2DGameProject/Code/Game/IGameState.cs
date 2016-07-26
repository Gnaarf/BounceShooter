using SFML.Graphics;

namespace GameProject2D
{
    interface IGameState
    {
        /// <summary>
        /// process the game logic for this state.
        /// </summary>
        /// <param name="deltaTime">time passed since last update</param>
        /// <returns>GameState-Changes are handeled by this return-value</returns>
        GameState Update(float deltaTime);

        /// <summary>
        /// do the drawing for this state.
        /// </summary>
        /// <param name="win">usually a RenderWindow</param>
        /// <param name="view">the "camera"</param>
        /// <param name="deltaTime">time passed since last update</param>
        void Draw(RenderWindow win, View view, float deltaTime);

        /// <summary>
        /// do the GUI-drawing for this state.
        /// </summary>
        /// <param name="gui">GUI</param>
        /// <param name="deltaTime">time passed since last update</param>
        void DrawGUI(GUI gui, float deltaTime);
    }
}
