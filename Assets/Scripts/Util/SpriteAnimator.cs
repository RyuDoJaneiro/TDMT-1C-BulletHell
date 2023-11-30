using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    private SpriteRenderer spriteRenderer;
    private List<Sprite> frames;
    private float frameRate;
    private bool loop;

    private int currentFrame;
    private float timer;

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f, bool loop = true)
    {
        this.frames = frames;
        this.spriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
        this.loop = loop;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        if (!loop && currentFrame == frames.Count - 1)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }

    }

    public List<Sprite> Frames
    {
        get { return frames; }
    }
}
