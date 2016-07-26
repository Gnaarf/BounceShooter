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
    public class Bullet : CircleBody, Updateable, PostCollisionUpdatable
    {
        int bounceCount;

        float speed;
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }

        List<Vector2> bounceOffNormals = new List<Vector2>();
        
        public Bullet(Vector2f position, Vector2 direction, int bounceCount)
            : base(position, 5F)
        {
            this.bounceCount = bounceCount;

            this.speed = 500F;
            this.movement = direction * speed;
        }

        public void Update(float deltaTime)
        {
            position += deltaTime * movement;

            bounceOffNormals.Clear();

            if(position.X < Map.LowerBorder.X || position.Y < Map.LowerBorder.Y || position.X > Map.UpperBorder.X || position.Y > Map.UpperBorder.Y)
            {
                BodyManager.Remove(this);
            }
        }

        public void PostCollisionUpdate(float deltaTime)
        {
            // bounce off, if collision occured
            if(bounceOffNormals.Count > 0)
            {
                BounceOff();
                bounceCount--;

                position += deltaTime * movement;
            }
        }

        protected override void OnCollision(Body other, Vector2 approximateCollisionPoint)
        {
            if(other is Wall)
            {
                // remove if no bounces left
                if (bounceCount <= 0)
                {
                    BodyManager.Remove(this);
                }
                else
                {
                    LineSegmentBody wall = other as LineSegmentBody;

                    // get correct noraml
                    Vector2 normal;
                    if (approximateCollisionPoint == wall.start || approximateCollisionPoint == wall.end)
                    {
                        // IF the bullet collides with the endpoint of a wall, treat it like a point collision
                        normal = (midPoint - approximateCollisionPoint).normalized;
                    }
                    else
                    {
                        // ELSE decide on which side of the wall the bullet was before the collision
                        normal = wall.direction.right;

                        if (Vector2.dot((midPoint - movement) - approximateCollisionPoint, normal) < 0)
                        {
                            normal *= -1;
                        }
                    }

                    // collect the normal, in case there are multiple collisions
                    bounceOffNormals.Add(normal);
                }
            }
        }

        private void BounceOff()
        {
            Vector2 averageNormal = Vector2.average(bounceOffNormals.ToArray());

            movement = Vector2.reflect(movement, averageNormal);
            movement = movement.normalized * speed;
        }

        public void draw(RenderWindow win, View view)
        {
            
        }

        public void debugDraw(RenderWindow win, View view)
        {
            base.DebugDraw(win, view);
        }
    }
}
