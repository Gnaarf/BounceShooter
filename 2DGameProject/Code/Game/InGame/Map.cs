﻿using System;
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
            int numSegments = 6;
            float segmentAngle = 2F * Helper.PI / (float)numSegments;
            Vector2 midpoint = new Vector2(400, 300);
            float radius = 300F;
            for (int i = 0; i < numSegments; i++)
            {
                float angle = i * segmentAngle;
                Vector2 start = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                float endAngle = angle + segmentAngle - Helper.PI / 20F;
                Vector2 end = new Vector2((float)Math.Cos(endAngle), (float)Math.Sin(endAngle)) * radius;
                start += midpoint;
                end += midpoint;
                Wall borderSegment = new Wall(start, end);

                bodies.Add(borderSegment);
            }
        }

        public void update(float deltaTime)
        {
            foreach (Player player in players)
            {
                player.update(deltaTime);
            }

            int wallCount = 0;

            foreach (Body b in bodies)
            {
                b.resetCollision();

                if (b is Wall)
                    wallCount++;
            }

            Console.WriteLine(wallCount);

            foreach (Body b1 in bodies)
            {
                foreach (Body b2 in bodies)
                {
                    if(b1 != b2)
                        b1.checkAndInformCollision(b2);
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
