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

        public static Vector2 lowerBorder = new Vector2(100, 0);
        public static Vector2 upperBorder = new Vector2(700, 600);

        public Map()
        {
            // add new player, when new gamePad registers
            GamePadInputManager.RegisterPadEvent += AddPlayer;

            Vector2 midpoint = new Vector2(400, 300);

            // Set startpositions
            startpositions = CreatePolygon(midpoint, GamePadInputManager.numSupportedPads, 200F);

            // build Map
            CreateWalls(CreatePolygon(midpoint, 7, 300F));
            CreateWalls(CreatePolygon(new Vector2(500, 200), 3, 70F));
            CreateWalls(CreatePolygon(new Vector2(250, 280), 4, 30F));
            CreateWalls(CreatePolygon(new Vector2(500, 400), 5, 20F));

            AddPlayer(0);
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
            BodyManager.Draw(win, view);
        }

        public void debugDraw(RenderWindow win, View view)
        {
            BodyManager.DebugDraw(win, view);
        }

        List<Vector2> CreatePolygon(Vector2 midPoint, int numEdges, float radius, float startAngle = 0F)
        {
            List<Vector2> result = new List<Vector2>();

            float deltaAngle = 2F * Helper.PI / (float)numEdges;
            for (int i = 0; i < numEdges; i++)
            {
                result.Add(midPoint + Vector2.Right.Rotate(i * deltaAngle) * radius);
            }

            return result;
        }

        void CreateWalls(List<Vector2> edges)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                Vector2 start = edges[i];
                Vector2 end = edges[(i + 1) % edges.Count];
                Wall borderSegment = new Wall(start, end);
            }
        }
    }
}
