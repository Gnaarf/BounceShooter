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
    public class Player : CircleBody, Updateable, Drawable
    {
        // game logic stuff
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }
        Vector2 forward { get; set; } = Vector2.Up;

        float speed = 300F;

        public int index;

        float chargedBounceCount = 0F;
        float chargingSpeed = 10.2F;

        public static readonly float MaxLife = 100F;
        float life = MaxLife;
        public bool isDead { get { return life <= 0F; } }

        public static readonly float ReviveTime = 5F;
        float reviveCountdown = 0F;

        // visualization stuff
        Text DebugText;

        Sprite sprite;

        public static int Count;

        public Player(Vector2 position, int index)
            : base(position, 30F)
        {
            this.index = index;

            this.movement = new Vector2f(0F, 0F);

            // drawing-stuff
            DebugText = new Text("", AssetManager.GetFont(AssetManager.FontName.Calibri));

            sprite = new Sprite(AssetManager.GetTexture(AssetManager.TextureName.Player));
            sprite.Origin = ((Vector2) sprite.Texture.Size) * 0.5F;
            sprite.Scale = (2 * this.radius * Vector2.One) / (Vector2)sprite.Texture.Size;

            // other stuff
            GamePadInputManager.UnregisterPadEvent += Destroy;

            Count++;
            Console.WriteLine("PlayerCount = " + Count);
        }

        private void Destroy(uint i)
        {
            if(index == i)
            {
                BodyManager.Remove(this);
                GamePadInputManager.UnregisterPadEvent -= Destroy;
                Count--;
                Console.WriteLine("PlayerCount = " + Count);
            }
        }

        public void Update(float deltaTime)
        {
            // move
            Move(deltaTime);

            // rotate
            Rotate(deltaTime);

            if (!isDead)
            {
                // shoot and update Bullets
                Shoot(deltaTime);
            }
            else
            {
                reviveCountdown -= deltaTime;
                if(reviveCountdown <= 0F)
                {
                    Revive();
                }
            }
        }

        private void Shoot(float deltaTime)
        {
            bool isCharging = false;
            bool isShooting = false;

            if (GamePadInputManager.IsConnected(index))
            {
                isCharging = GamePadInputManager.IsPressed(GamePadButton.RB, index);
                isShooting = GamePadInputManager.Upward(GamePadButton.RB, index);
            }
            else if(index == 0)
            {
                isCharging = KeyboardInputManager.IsPressed(Keyboard.Key.Space);
                isShooting = KeyboardInputManager.Upward(Keyboard.Key.Space);
            }

            if(isCharging)
            {
                chargedBounceCount += deltaTime * chargingSpeed / (chargedBounceCount + 1);
            }   
            if(isShooting)
            { 
                Bullet bullet = new Bullet(midPoint, forward, (int)chargedBounceCount + 1);
                bullet.midPoint += forward * (radius + bullet.radius + 1);

                chargedBounceCount = 0F;
            }
        }

        private void Rotate(float deltaTime)
        {
            if (GamePadInputManager.IsConnected(index))
            {
                Vector2 input = GamePadInputManager.GetRightStick(index);
                if(input != Vector2.Zero)
                {
                    forward = input * new Vector2(1, -1);
                    forward = forward.normalized;
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

        private void Move(float deltaTime)
        {
            if (GamePadInputManager.IsConnected(index))
            {
                Vector2 inputMovement = GamePadInputManager.GetLeftStick(index) * new Vector2(1, -1);
                if (inputMovement.lengthSqr > 1F)
                {
                    inputMovement = inputMovement.normalized;
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
                    inputMovement = inputMovement.normalized;
                }
                movement = inputMovement * speed;

                position += movement * deltaTime;
            }
        }

        protected override void OnCollision(Body other, Vector2 approximateCollisionPoint)
        {
            if(other is Wall)
            {
                // can't walk through walls
                Vector2 pushVector = approximateCollisionPoint - position;

                float distance = pushVector.length;

                position -= (pushVector / distance) * (0.1F + radius - distance);
            }
            else if (other is Player)
            {
                // can't walk through players
                Player otherPlayer = other as Player;

                position = approximateCollisionPoint + (position - approximateCollisionPoint).normalized * radius;
            }
            else if (other is Bullet)
            {
                // get hit by bullet
                if (!isDead)
                {
                    RecieveDamage(((Bullet)other).damage);
                }
            }
        }

        private void RecieveDamage(float damage)
        {
            life -= damage;
            if (isDead)
            {
                reviveCountdown = ReviveTime;
            }
        }
        
        private void Revive()
        {
            life = MaxLife;
        }

        public void Draw(RenderWindow win, View view)
        {
            sprite.Position = position;
            sprite.Rotation = forward.rotation * Helper.RadianToDegree;
            sprite.Color = Helper.Lerp(Color.White, Color.Transparent, isDead ? 1.0F : 0.5F);

            win.Draw(sprite);
        }

        public override void DebugDraw(RenderWindow win, View view)
        {
            base.DebugDraw(win, view);

            if (!isDead)
            {
                CircleShape forwardShape = new CircleShape(5);
                forwardShape.Origin = Vector2.One * forwardShape.Radius;
                forwardShape.Position = midPoint + 50 * forward;
                forwardShape.FillColor = Color.Blue;
                win.Draw(forwardShape);
            }

            if (isDead)
            {
                DebugText.DisplayedString = "" + ((int)reviveCountdown + 1);
            }
            else
            {
                DebugText.DisplayedString = "" + (int)chargedBounceCount;
            }
            FloatRect TextRect = DebugText.GetGlobalBounds();
            DebugText.Position = midPoint - new Vector2(TextRect.Width, (TextRect.Height + DebugText.CharacterSize)) * 0.5F;
            win.Draw(DebugText);
        }
    }
}
