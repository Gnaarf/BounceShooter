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
        List<Player> players = new List<Player>();

        public static List<Body> bodies = new List<Body>();

        public int index;

        public Map()
        {
            bodies.Clear();

            startpositions.Add(new Vector2(300, 100));
            startpositions.Add(new Vector2(500, 500));

            // create players
            for (int i = 0; i < startpositions.Count; i++)
            {
                Player player = new Player(startpositions[i], i);
                players.Add(player);
                bodies.Add(player);
            }

            // build Map
            float segmentAngle = 2F * Helper.PI / 6F;
            for (float angle = 0F; angle < 2 * Helper.PI; angle += segmentAngle)
            {
                Vector2 midpoint = new Vector2(400, 300);
                Vector2 start = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 300;
                Vector2 end = new Vector2((float)Math.Cos(angle + segmentAngle), (float)Math.Sin(angle + segmentAngle)) * 300;
                start += midpoint;
                end += midpoint;
                LineBody borderSegment = new LineBody(start, end);

                bodies.Add(borderSegment);
            }
        }

        public void update(float deltaTime)
        {
            foreach (Player player in players)
            {
                player.update(deltaTime);
            }

            foreach (Body b in bodies)
            {
                b.resetCollision();
            }

            foreach (Body b1 in bodies)
            {
                foreach (Body b2 in bodies)
                {
                    if(b1 != b2)
                        b1.checkCollision(b2);
                }
            }
        }

        public void draw(RenderWindow win, View view)
        {
            
        }

        public void debugDraw(RenderWindow win, View view)
        {
            foreach (Body body in bodies)
            {
                body.debugDraw(win, view);
            }
        }
    }
}
