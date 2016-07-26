using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    public class Map
    {
        List<Vector2> startpositions = new List<Vector2>();

        public int index;

        public static Vector2 LowerBorder = new Vector2(100, 0);
        public static Vector2 UpperBorder = new Vector2(700, 600);

        public Map()
        {
            // add new player, when new gamePad registers
            GamePadInputManager.RegisterPadEvent += AddPlayer;

            Vector2 midpoint = new Vector2(400, 300);

            // Set startpositions
            float deltaAngle = 2F * Helper.PI / (float)GamePadInputManager.numSupportedPads;
            float startPositionRadius = 200F;
            for (int i = 0; i < GamePadInputManager.numSupportedPads; i++)
            {
                startpositions.Add(midpoint + Vector2.Up.rotate(i * deltaAngle) * startPositionRadius);
            }

            // build Map
            int numSegments = 7;
            float segmentAngle = 2F * Helper.PI / (float)numSegments;
            float radius = 300F;
            for (int i = 0; i < numSegments; i++)
            {
                float angle = i * segmentAngle;
                Vector2 start = Vector2.Up.rotate(angle) * radius;
                float endAngle = angle + segmentAngle;
                Vector2 end = Vector2.Up.rotate(endAngle) * radius;
                start += midpoint;
                end += midpoint;
                Wall borderSegment = new Wall(start, end);
            }
        }



        private void AddPlayer(uint i)
        {
            AddPlayer((int)i);
        }

        private void AddPlayer(int i)
        {
            Player player = new Player(startpositions[i], i);
        }

        public void update(float deltaTime)
        {
            BodyManager.Update(deltaTime);
        }

        public void draw(RenderWindow win, View view)
        {

        }

        public void debugDraw(RenderWindow win, View view)
        {
            BodyManager.DebugDraw(win, view);
        }
    }
}
