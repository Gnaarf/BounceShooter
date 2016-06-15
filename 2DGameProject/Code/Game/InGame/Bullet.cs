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
    public class Bullet : CircleBody
    {
        int bounceCount;

        float speed;
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }
        
        public Bullet(Vector2f position, Vector2 direction, int bounceCount)
            : base(position, 5F)
        {
            this.bounceCount = bounceCount;

            this.speed = 500F;
            this.movement = direction * speed;
        }

        public void preCollisionUpdate(float deltaTime)
        {
            position += deltaTime * movement;
        }

        protected override void OnCollision(Body other, Vector2 approximateCollisionPoint)
        {
            if(other is Wall)
            {
                LineSegmentBody lineSegment = other as LineSegmentBody;

                BodyManager.CacheForRemoval(this);
            }


            // bounce if possible, else disappear
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
