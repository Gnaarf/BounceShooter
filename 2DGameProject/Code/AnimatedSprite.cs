using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

public class AnimatedSprite : Sprite
{
    public float secondsPerFrame { get; private set; }
    public Vector2 spriteSize { get; private set; }
    public int frameCount { get; private set; }
    Vector2 upperLeftCorner;

    float? startSecond;

    public AnimatedSprite(Texture spriteSheet, float secondsPerFrame, int frameCount, Vector2 spriteSize)
        : this(spriteSheet, secondsPerFrame, frameCount, spriteSize, new Vector2(0, 0))
    {
    }

    public AnimatedSprite(Texture spriteSheet, float secondsPerFrame, int frameCount, Vector2 spriteSize, Vector2 upperLeftCorner)
        : base(spriteSheet)
    {
        this.secondsPerFrame = secondsPerFrame;
        this.frameCount = frameCount;
        this.spriteSize = spriteSize;
        this.upperLeftCorner = upperLeftCorner;
        startSecond = 0F;
    }

    /// <summary>start or restart the animation</summary>
    public void RestartAnimation(GameTime currentTime)
    {
        startSecond = (float)currentTime.totalTime.TotalSeconds;
    }

    /// <summary>start or restart the animation</summary>
    public void StopAnimation()
    {
        startSecond = null;
    }

    public Sprite UpdateFrame(GameTime currentTime)
    {
        int currentFrame = 0;

        if (startSecond.HasValue)
        {
            float passedSeconds = 0F;
            passedSeconds = (float)currentTime.totalTime.TotalSeconds - startSecond.Value;
            passedSeconds /= ((float)frameCount * secondsPerFrame);
            passedSeconds -= (float)Math.Floor(passedSeconds);

            currentFrame = (int)(passedSeconds * frameCount);
        }

        TextureRect = new IntRect((int)(upperLeftCorner.x + (currentFrame * spriteSize.x)), (int)(upperLeftCorner.y), (int)(spriteSize.y), (int)(spriteSize.y));
        return this;

    }
}