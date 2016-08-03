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
        private static List<Body> cachedForAddition = new List<Body>();

        public static void Initialize()
        {
            bodies.Clear();
            cachedForRemoval.Clear();
        }

        public static void Update(float deltaTime)
        {

            foreach (Body body in bodies)
            {
                Updateable up = body as Updateable;

                if(body is Updateable)
                {
                    ((Updateable)body).Update(deltaTime);
                }
            }

            AddCachedBodies();

            CheckAndInformCollision();

            foreach(Body body in bodies)
            {
                PostCollisionUpdatable updatable = body as PostCollisionUpdatable;
                if (updatable != null)
                {
                    updatable.PostCollisionUpdate(deltaTime);
                }
            }

            RemoveCachedBodies();
        }

        /// <summary>
        /// will be removed at end of frame
        /// </summary>
        /// <param name="body"></param>
        public static void Remove(Body body)
        {
            cachedForRemoval.Add(body);
        }

        public static void Add(Body body)
        {

            cachedForAddition.Add(body);
        }

        public static bool Contains(Body body)
        {
            return bodies.Contains(body);
        }

        private static void RemoveCachedBodies()
        {
            foreach (Body toBeRemoved in cachedForRemoval)
            {
                Console.WriteLine("Rem" + toBeRemoved);
                bodies.Remove(toBeRemoved);
            }
            cachedForRemoval.Clear();
        }

        private static void AddCachedBodies()
        {
            foreach (Body toBeAdded in cachedForAddition)
            {
                bodies.Add(toBeAdded);
            }
            cachedForAddition.Clear();
        }

        private static void CheckAndInformCollision()
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
        }

        public static void Draw(RenderWindow win, View view)
        {
            foreach (Body b in bodies)
            {
                if(b is Drawable)
                    ((Drawable)b).Draw(win, view);
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
