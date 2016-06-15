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
    public static class BodyManager
    {
        private static List<Body> bodies = new List<Body>();
        private static List<Body> cachedForRemoval = new List<Body>();

        public static void Initialize()
        {
            bodies.Clear();
            cachedForRemoval.Clear();
        }

        /// <summary>
        /// will be removed at end of frame
        /// </summary>
        /// <param name="body"></param>
        public static void CacheForRemoval(Body body)
        {
            cachedForRemoval.Add(body);
        }

        public static void Add(Body body)
        {
            bodies.Add(body);
        }

        public static void RemoveCachedBodies()
        {
            foreach(Body toBeRemoved in cachedForRemoval)
            {
                bodies.Remove(toBeRemoved);
            }
        }

        public static void CheckAndInformCollision()
        {
            foreach (Body b in bodies)
            {
                b.ResetCollision();
            }

            for(int i = 0; i < bodies.Count; i++)
            {
                for(int j = i + 1; j < bodies.Count; j++)
                {
                    bodies[i].CheckAndInformCollision(bodies[j]);
                }
            }
            foreach (Body b1 in bodies)
            {
                foreach (Body b2 in bodies)
                {
                }
            }
        }

        public static void DebugDraw(RenderWindow win, View view)
        {
            foreach (Body b in bodies)
            {
                b.DebugDraw(win, view);
            }
        }
    }
}
