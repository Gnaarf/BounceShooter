using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    public class Bullet : CircleBody, Updateable, PostCollisionUpdatable, Drawable
    {
        int bounceCount;

        float speed;
        Vector2 position { get { return midPoint; } set { midPoint = value; } }
        Vector2 movement { get; set; }

        float _damage = 10F;
        public float damage { get { return _damage; } }

        List<LineSegmentBody> PrecurserLines = new List<LineSegmentBody>();


        List<Vector2> bounceOffNormals = new List<Vector2>();

        // draw-stuff
        Sprite sprite;

        public Bullet(Vector2f position, Vector2 direction, int bounceCount)
            : base(position, 1F)
        {
            this.bounceCount = bounceCount;

            this.speed = 500F;
            this.movement = direction * speed;

            this.sprite = new Sprite(AssetManager.GetTexture(AssetManager.TextureName.Bullet));
            this.sprite.Origin = ((Vector2)this.sprite.Texture.Size) * 0.5F;
        }

        public void Update(float deltaTime)
        {
            position += deltaTime * movement;

            bounceOffNormals.Clear();

            if(position.x < Map.lowerBorder.x || position.y < Map.lowerBorder.y || position.x > Map.upperBorder.x || position.y > Map.upperBorder.y)
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

                    bounceOffNormals.Add((midPoint - player.midPoint).normalized);
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

                        if (Vector2.Dot((midPoint - movement) - approximateCollisionPoint, normal) < 0)
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
            Vector2 averageNormal = Vector2.Average(bounceOffNormals.ToArray());

            movement = Vector2.Reflect(movement, averageNormal);
            movement = movement.normalized * speed;
        }

        public void Draw(RenderWindow win, View view)
        {
            sprite.Position = this.position;
            win.Draw(sprite);
        }

        public void debugDraw(RenderWindow win, View view)
        {
            base.DebugDraw(win, view);
        }
    }
}
