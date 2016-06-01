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

        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }
        
        public Bullet(Vector2f position, int bounceCount)
            : base(position, 5F)
        {
            this.movement = new Vector2f(0F, 500F);
        }

        public void preCollisionUpdate(float deltaTime)
        {
            position += deltaTime * movement;
        }

        protected override void OnCollision(Body other)
        {
            Vector2 nearestPoint;
            if(other is LineSegmentBody)
            {
                LineSegmentBody lineSegment = other as LineSegmentBody;
            }


            // bounce if possible, else disappear
        }


        public void draw(RenderWindow win, View view)
        {
            
        }

        public void debugDraw(RenderWindow win, View view)
        {
            base.debugDraw(win, view);
        }
    }
}
