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
    public class Player : CircleBody
    {
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }

        List<Bullet> bullets = new List<Bullet>();

        public int index;

        public Player(Vector2f position, int index)
            : base(position, 30F)
        {
            this.index = index;

            this.movement = new Vector2f(0F, 0F);
        }

        public void update(float deltaTime)
        {
            if (index != 0)
                return;

            // move
            move(deltaTime);

            // shoot
            if(KeyboardInputManager.Downward(Keyboard.Key.Space))
            {
                Bullet bullet = new Bullet(midPoint);
                bullet.midPoint += Vector2.Down * (radius + bullet.radius);

                Map.bodies.Add(bullet);
                bullets.Add(bullet);
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.update(deltaTime);
            }
        }

        private void move(float deltaTime)
        {
            float speed = deltaTime;

            Vector2 inputMovement = new Vector2(0F, 0F);

            inputMovement.Y += Keyboard.IsKeyPressed(Keyboard.Key.Down) ? speed : 0F;
            inputMovement.Y += Keyboard.IsKeyPressed(Keyboard.Key.Up) ? -speed : 0F;

            inputMovement.X += Keyboard.IsKeyPressed(Keyboard.Key.Left) ? -speed : 0F;
            inputMovement.X += Keyboard.IsKeyPressed(Keyboard.Key.Right) ? speed : 0F;

            if (inputMovement.Y != 0F || inputMovement.X != 0F)
            {
                movement += inputMovement * speed / (float)Math.Sqrt(inputMovement.X * inputMovement.X + inputMovement.Y * inputMovement.Y);
            }

            movement *= (1F - deltaTime * 4F);    // friction

            position += movement;

            if (position.X < 0)
            {
                position -= movement;
                movement *= Vector2.Up;
            }
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
