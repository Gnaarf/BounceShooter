using SFML.Graphics;
using SFML.Window;
using System;

namespace GameProject2D
{
    class Program
    {
        public static GameTime GameTime;

        static bool running = true;

        static GameState currentGameState = GameState.InGame;
        static GameState prevGameState = GameState.InGame;
        static IGameState state;

        public static RenderWindow win;
        static readonly Vector2 windowSize = new Vector2(800, 600);
        static View view;
        static GUI gui;

        static void Main(string[] args)
        {
            // initialize window and view
            win = new RenderWindow(new VideoMode((uint)windowSize.X, (uint)windowSize.Y), "Hadoken!!!");
            view = new View();
            ResetView();
            gui = new GUI(win, view);
            
            // prevent window resizing
            win.Resized += (sender, e) => { (sender as Window).Size = windowSize; };

            // exit Program, when window is being closed
            //win.Closed += new EventHandler(closeWindow);
            win.Closed += (sender, e) => { (sender as Window).Close(); };

            // initialize GameState
            HandleNewGameState();

            // initialize GameTime
            GameTime = new GameTime();
            GameTime.Start();

            // debug Text
            Text debugText = new Text("debug Text", AssetManager.GetFont(AssetManager.FontName.Calibri));

            while (running && win.IsOpen())
            {
                KeyboardInputManager.Update();
                GamePadInputManager.Update();

                // update GameTime
                GameTime.Update();
                float deltaTime = (float)GameTime.EllapsedTime.TotalSeconds;

                currentGameState = state.Update(deltaTime);

                if (currentGameState != prevGameState)
                {
                    HandleNewGameState();
                }

                // gather drawStuff from State
                win.Clear(new Color(100, 149, 237));    //cornflowerblue ftw!!! 1337
                state.Draw(win, view, deltaTime);
                state.DrawGUI(gui, deltaTime);

                // some DebugText
                debugText.DisplayedString = "fps: " + (1.0F / deltaTime);
                win.Draw(debugText);

                // do the actual drawing
                win.SetView(view);
                win.Display();

                // check for window-events. e.g. window closed        
                win.DispatchEvents();
            }
        }

        static void HandleNewGameState()
        {
            switch (currentGameState)
            {
                case GameState.None:
                    running = false;
                    break;

                case GameState.MainMenu:
                    state = new MainMenuState();
                    break;

                case GameState.InGame:
                    state = new InGameState();
                    break;

                case GameState.Reset:
                    currentGameState = prevGameState;
                    prevGameState = GameState.Reset;
                    HandleNewGameState();
                    break;
            }

            prevGameState = currentGameState;

            ResetView();
        }

        static void ResetView()
        {
            view.Center = new Vector2(win.Size.X / 2F, win.Size.Y / 2F);
            view.Size = new Vector2(win.Size.X, win.Size.Y);
        }
    }
}