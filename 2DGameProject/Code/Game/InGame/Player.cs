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
    public class Player : CircleBody, Updateable
    {
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }
        Vector2 forward { get; set; } = Vector2.Up;

        float speed = 300F;

        List<Bullet> bullets = new List<Bullet>();

        public int index;

        public Player(Vector2 position, int index)
            : base(position, 30F)
        {
            this.index = index;

            this.movement = new Vector2f(0F, 0F);
        }
        
        public void Update(float deltaTime)
        {
            // move
            move(deltaTime);

            // rotate
            rotate(deltaTime);

            // shoot and update Bullets
            Shoot(deltaTime);
        }

        private void Shoot(float deltaTime)
        {
            bool shootABullet = false;

            if (GamePadInputManager.IsConnected(index))
            {
                shootABullet = GamePadInputManager.Downward(GamePadButton.RB, index);
            }
            else if(index == 0)
            {
                shootABullet = KeyboardInputManager.Downward(Keyboard.Key.Space);
            }


            if(shootABullet)
            { 
                Bullet bullet = new Bullet(midPoint, forward, 0);
                bullet.midPoint += forward * (radius + bullet.radius);

                BodyManager.Add(bullet);
                bullets.Add(bullet);
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.preCollisionUpdate(deltaTime);
            }
        }

        private void rotate(float deltaTime)
        {
            if (GamePadInputManager.IsConnected(index))
            {
                Vector2 input = GamePadInputManager.GetRightStick(index);
                if(input != Vector2.Zero)
                {
                    forward = input * new Vector2(1, -1);
                    forward = forward.normalize();
                }
            }
            else if (index == 0)
            {
                if (KeyboardInputManager.Downward(Keyboard.Key.Up))
                    forward = Vector2.Up;
                if (KeyboardInputManager.Downward(Keyboard.Key.Down))
                    forward = Vector2.Down;
                if (KeyboardInputManager.Downward(Keyboard.Key.Left))
                    forward = Vector2.Left;
                if (KeyboardInputManager.Downward(Keyboard.Key.Right))
                    forward = Vector2.Right;
            }
        }

        private void move(float deltaTime)
        {

            if (GamePadInputManager.IsConnected(index))
            {
                Vector2 inputMovement = GamePadInputManager.GetLeftStick(index) * new Vector2(1, -1);
                if (inputMovement.lengthSqr > 1F)
                {
                    inputMovement.normalize();
                }
                movement = inputMovement * speed;

                position += movement * deltaTime;
            }
            else if(index == 0)
            {
                Vector2 inputMovement = Vector2.Zero;

                inputMovement += KeyboardInputManager.IsPressed(Keyboard.Key.Up) ? Vector2.Up : Vector2.Zero;
                inputMovement += KeyboardInputManager.IsPressed(Keyboard.Key.Down) ? Vector2.Down : Vector2.Zero;

                inputMovement += KeyboardInputManager.IsPressed(Keyboard.Key.Left) ? Vector2.Left : Vector2.Zero;
                inputMovement += KeyboardInputManager.IsPressed(Keyboard.Key.Right) ? Vector2.Right : Vector2.Zero;

                if (inputMovement.lengthSqr > 1F)
                {
                    inputMovement.normalize();
                }
                movement = inputMovement * speed;

                position += movement * deltaTime;
            }
        }

        protected override void OnCollision(Body other, Vector2 approximateCollisionPoint)
        {
            if(other is Wall)
            {
                Vector2 pushVector = approximateCollisionPoint - position;

                float distance = pushVector.length;

                position -= (pushVector / distance) * (0.1F + radius - distance);
            }
        }

        public void draw(RenderWindow win, View view)
        {
            
        }

        public override void DebugDraw(RenderWindow win, View view)
        {
            base.DebugDraw(win, view);
            
            CircleShape forwardShape = new CircleShape(5);
            forwardShape.Origin = Vector2.One * forwardShape.Radius;
            forwardShape.Position = midPoint + 50 * forward;
            forwardShape.FillColor = Color.Blue;
            win.Draw(forwardShape);
        }
    }
}
