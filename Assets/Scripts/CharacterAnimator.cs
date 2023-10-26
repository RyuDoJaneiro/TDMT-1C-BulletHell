using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private bool hasLeftAnimation = false;

    [Header("Walk Animations")]
    [Space(10)]
    [SerializeField] private List<Sprite> walkUpSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> walkDownSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> walkRightSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> walkLeftSprites = new List<Sprite>();

    [Header("Idle Animations")]
    [Space(10)]
    [SerializeField] private List<Sprite> idleUpSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> idleDownSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> idleRightSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> idleLeftSprites = new List<Sprite>();

    [SerializeField] private List<Sprite> deathSprites = new List<Sprite>();

    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    // Parameters    
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public float LastMoveX { get; set; }
    public float LastMoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool IsDying { get; set; }


    // States
    private SpriteAnimator walkDownAnim;
    private SpriteAnimator walkUpAnim;
    private SpriteAnimator walkRightAnim;
    private SpriteAnimator walkLeftAnim;

    private SpriteAnimator idleDownAnim;
    private SpriteAnimator idleUpAnim;
    private SpriteAnimator idleRightAnim;
    private SpriteAnimator idleLeftAnim;

    private SpriteAnimator deathAnim;

    private SpriteAnimator currentAnim;
    bool wasPreviouslyMove;

    // References
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        idleDownAnim = new SpriteAnimator(idleDownSprites, spriteRenderer);
        idleUpAnim = new SpriteAnimator(idleUpSprites, spriteRenderer);
        idleRightAnim = new SpriteAnimator(idleRightSprites, spriteRenderer);
        idleLeftAnim = new SpriteAnimator(idleLeftSprites, spriteRenderer);

        deathAnim = new SpriteAnimator(deathSprites, spriteRenderer, 0.20f);

        SetFacingDirection(defaultDirection);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;

        if (IsMoving)
        {
            // Play walk animations
            if (MoveX == 1)
            {
                spriteRenderer.flipX = false;
                currentAnim = walkRightAnim;
            }
            else if (MoveX == -1)
            {
                if (!hasLeftAnimation)
                    spriteRenderer.flipX = true;
                currentAnim = walkLeftAnim;
            }
            else if (MoveY == 1)
                currentAnim = walkUpAnim;
            else if (MoveY == -1)
                currentAnim = walkDownAnim;
        }
        else
        {
            // Play idle animations
            if (LastMoveX == 1)
                currentAnim = idleRightAnim;
            else if (LastMoveX == -1)
                currentAnim = idleLeftAnim;
            else if (LastMoveY == 1)
                currentAnim = idleUpAnim;
            else if (LastMoveY == -1)
                currentAnim = idleDownAnim;
        }

        if (IsDying)
        {
            currentAnim = deathAnim;
        }

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMove)
            currentAnim.Start();

        currentAnim.HandleUpdate();
        wasPreviouslyMove = IsMoving;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            LastMoveX = 1;
        else if (dir == FacingDirection.Left)
            LastMoveX = -1;
        else if (dir == FacingDirection.Down)
            LastMoveY = -1;
        else if (dir == FacingDirection.Up)
            LastMoveY = 1;
    }

    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    }

    private void HandleBasicAnimations()
    {
        
    }
}

public enum FacingDirection { Up, Down, Right, Left }
