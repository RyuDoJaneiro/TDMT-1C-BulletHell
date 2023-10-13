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

    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    // Parameters    
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public float lastMoveX { get; set; }
    public float lastMoveY { get; set; }
    public bool IsMoving { get; set; }


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
            if (lastMoveX == 1)
                currentAnim = idleRightAnim;
            else if (lastMoveX == -1)
                currentAnim = idleLeftAnim;
            else if (lastMoveY == 1)
                currentAnim = idleUpAnim;
            else if (lastMoveY == -1)
                currentAnim = idleDownAnim;
        }

        currentAnim.HandleUpdate();
        wasPreviouslyMove = IsMoving;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            lastMoveX = 1;
        else if (dir == FacingDirection.Left)
            lastMoveX = -1;
        else if (dir == FacingDirection.Down)
            lastMoveY = -1;
        else if (dir == FacingDirection.Up)
            lastMoveY = 1;
    }

    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    }
}

public enum FacingDirection { Up, Down, Right, Left }
