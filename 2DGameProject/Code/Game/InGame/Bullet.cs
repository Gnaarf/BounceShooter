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
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }
        
        public Bullet(Vector2f position)
            : base(position, 5F)
        {
            this.movement = new Vector2f(0F, 500F);
        }

        public void update(float deltaTime)
        {
            float speed = deltaTime;

            position += speed * movement;
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
