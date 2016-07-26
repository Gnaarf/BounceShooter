using SFML.Window;
using System.Collections.Generic;

/// <summary>
/// A Wrapper-Class for the SFML.Window.Joystick. Better usability.
/// </summary>
namespace GameProject2D
{
    public enum GamePadButton { A, B, X, Y, LB, RB, Select, Start, LT, RT, BUTTONNUM };

    public static class GamePadInputManager
    {
        public struct Input
        {
            public Vector2 leftStick;
            public Vector2 rightStick;
            public float LTRT;

            public bool[] oldButton;
            public bool[] currentButton;
        }

        private static bool isInitialized = false;

        private const float deadZone = 0.1F;

        private static Dictionary<uint, Input> padInputs = new Dictionary<uint, Input>();


        public static readonly int numSupportedPads = 15;

        public static int numConnectedPads { get; private set; }

        public static Dictionary<uint, Input>.KeyCollection connectedPadIndices { get { return padInputs.Keys; } }

        
        public delegate void PadEventHandler(uint index);

        public static event PadEventHandler RegisterPadEvent;
        public static event PadEventHandler UnregisterPadEvent;


        static void Initialize()
        {
            padInputs.Clear();

            Joystick.Update();

            numConnectedPads = 0;

            for (uint i = 0; i < numSupportedPads; i++)
            {
                if (Joystick.IsConnected(i))
                {
                    RegisterPad(i);
                }
            }

            isInitialized = true;
        }

        private static void RegisterPad(uint i)
        {
            numConnectedPads++;

            Input input = new Input();

            input.oldButton = new bool[(int)GamePadButton.BUTTONNUM];
            input.currentButton = new bool[(int)GamePadButton.BUTTONNUM];

            input.leftStick = new Vector2(0, 0);
            input.rightStick = new Vector2(0, 0);

            padInputs[i] = input;

            RegisterPadEvent?.Invoke(i);
        }

        private static void UnregisterPad(uint i)
        {
            numConnectedPads--;

            UnregisterPadEvent?.Invoke(i);

            padInputs.Remove(i);
        }

        public static void Update()
        {
            if (!isInitialized) { Initialize(); }

            Joystick.Update();

            for (uint index = 0; index < numSupportedPads; index++)
            {
                if (!Joystick.IsConnected(index))
                {
                    if (padInputs.ContainsKey(index))
                    {
                        UnregisterPad(index);
                    }
                }
                else
                {
                    if (!padInputs.ContainsKey(index))
                    {
                        RegisterPad(index);
                    }

                    Input input = padInputs[index];

                    for (uint i = 0; i < (uint)GamePadButton.BUTTONNUM; i++)
                    {
                        input.oldButton[i] = input.currentButton[i];
                        input.currentButton[i] = Joystick.IsButtonPressed(index, i);
                    }

                    input.rightStick = 0.01F * new Vector2(Joystick.GetAxisPosition(index, Joystick.Axis.U), -Joystick.GetAxisPosition(index, Joystick.Axis.R));
                    input.rightStick = AdjustDeadZone(input.rightStick);

                    input.leftStick = 0.01F * new Vector2(Joystick.GetAxisPosition(index, Joystick.Axis.X), -Joystick.GetAxisPosition(index, Joystick.Axis.Y));
                    input.leftStick = AdjustDeadZone(input.leftStick);

                    input.LTRT = Joystick.GetAxisPosition(index, Joystick.Axis.Z);

                    padInputs[index] = input;
                }
            }
        }

        private static Vector2 AdjustDeadZone(Vector2 v)
        {
            if (v.lengthSqr < deadZone)
            {
                v = Vector2.Zero;
            }
            return v;
        }


        public static bool IsConnected(int padIndex)
        {
            if (padIndex < 0)
                throw new System.Exception("padIndex must be non-negative, but is '" + padIndex + "'");
            return IsConnected((uint)padIndex);
        }

        public static bool IsConnected(uint padIndex)
        {
            if (!isInitialized) { Initialize(); }

            return padInputs.ContainsKey(padIndex);
        }


        public static Vector2 GetLeftStick(int padIndex)
        {
            if (padIndex < 0)
                throw new System.Exception("padIndex must be non-negative, but is '" + padIndex + "'");
            return GetLeftStick((uint)padIndex);
        }

        public static Vector2 GetLeftStick(uint padIndex)
        {
            if (!isInitialized) { Initialize(); }

            return padInputs[padIndex].leftStick;
        }


        public static Vector2 GetRightStick(int padIndex)
        {
            if (padIndex < 0)
                throw new System.Exception("padIndex must be non-negative, but is '" + padIndex + "'");
            return GetRightStick((uint)padIndex);
        }

        public static Vector2 GetRightStick(uint padIndex)
        {
            if (!isInitialized) { Initialize(); }

            return padInputs[padIndex].rightStick;
        }


        public static bool Downward(GamePadButton button, int padIndex)
        {
            if (padIndex < 0)
                throw new System.Exception("padIndex must be non-negative, but is '" + padIndex + "'");
            return Downward(button, (uint)padIndex);
        }

        public static bool Downward(GamePadButton button, uint padIndex)
        {
            if (!isInitialized) { Initialize(); }

            return padInputs[padIndex].currentButton[(int)button] && !padInputs[padIndex].oldButton[(int)button];
        }


        public static bool IsPressed(GamePadButton button, int padIndex)
        {
            if (padIndex < 0)
                throw new System.Exception("padIndex must be non-negative, but is '" + padIndex + "'");
            return IsPressed(button, (uint)padIndex);
        }

        public static bool IsPressed(GamePadButton button, uint padIndex)
        {
            if (!isInitialized) { Initialize(); }

            if (button == GamePadButton.LT)
                return padInputs[padIndex].LTRT > 50;
            if (button == GamePadButton.RT)
                return padInputs[padIndex].LTRT < -50;

            return padInputs[padIndex].currentButton[(int)button];
        }


        public static bool Upward(GamePadButton button, int padIndex)
        {
            if (padIndex < 0)
                throw new System.Exception("padIndex must be non-negative, but is '" + padIndex + "'");
            return Upward(button, (uint)padIndex);
        }

        public static bool Upward(GamePadButton button, uint padIndex)
        {
            if (!isInitialized) { Initialize(); }

            return !padInputs[padIndex].currentButton[(int)button] && padInputs[padIndex].oldButton[(int)button];
        }
    }
}
