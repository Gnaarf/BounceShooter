using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    public class Laser : LineSegmentBody, Updateable, PostCollisionUpdatable, Drawable
    {
        int bounceCount;

        float speed;
        Vector2 position { get { return end; } }
        Vector2 movement { get {return direction * speed; } }

        float maxTotalLength = 50;

        float _damage = 10F;
        public float damage { get { return _damage; } }

        List<LineSegmentBody> trail = new List<LineSegmentBody>();
        

        public Laser(Vector2f position, Vector2 direction, int bounceCount)
            : base(position, direction, 0.1F)
        {
            this.bounceCount = bounceCount;

            this.speed = 500F;

            trail.Add(this);
        }

        public void Update(float deltaTime)
        {
            // update linesegment in the front
            length += deltaTime * speed;

            // trim, if to long
            float summedLength = 0;
            for (int i = 0; i < trail.Count; i++)
            {
                summedLength += trail[i].length;
                if(summedLength >= maxTotalLength)
                {
                    LineSegmentBody lastSegment = trail[i];
                    float dif = summedLength - maxTotalLength;
                    lastSegment.start += dif * lastSegment.direction;
                    lastSegment.length -= dif;
                    if(i < trail.Count - 1)
                    {
                        trail.RemoveRange(i + 1, trail.Count - i);
                    }
                    break;
                }
            }
            
            if(position.x < Map.lowerBorder.x || position.y < Map.lowerBorder.y || position.x > Map.upperBorder.x || position.y > Map.upperBorder.y)
            {
                foreach(LineSegmentBody trailpart in trail)
                {
                    BodyManager.Remove(trailpart);
                }
            }
        }

        public void PostCollisionUpdate(float deltaTime)
        {
            // bounce off, if collision occured
        }

        protected override void OnCollision(Body other, Vector2 approximateCollisionPoint)
        {
            if(other is Player && !((Player)other).isDead)
            {
                // remove if no bounces left
                if (bounceCount <= 0)
                {
                    BodyManager.Remove(this);
                }
                else
                {
                    CircleBody player = other as CircleBody;

                }
            }
            else if(other is Wall)
            {
                // remove if no bounces left
                if (bounceCount <= 0)
                {
                    BodyManager.Remove(this);
                }
                else
                {
                }
            }
        }


        public void Draw(RenderWindow win, View view)
        {
        }

        public void debugDraw(RenderWindow win, View view)
        {
            base.DebugDraw(win, view);
        }
    }
}
