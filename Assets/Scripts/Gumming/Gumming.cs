using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class Gumming : MonoBehaviour
{
    #region Variables

    #region Public variables
    public SO_GummingData Data { get => data; private set => data = value; }

    public StateMachine StateMachine { get; private set; }

    public MoveState MoveState { get; private set; }
    public FallState FallState { get; private set; }
    public DeadState DeadState { get; private set; }

    public SpinDrillState SpinDrillState { get; private set; }
    public DigState DigState { get; private set; }

    public Animator Animator { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }
    public Abilities CurrentAbility { get; private set; }
    public int FacingDirection { get; private set; }
    #endregion

    #region Inspector variables
    [SerializeField] [Tooltip("Displays gizmos")] private bool debug;

    [Space(5)] [Header("Scriptable Object References")]
    [SerializeField] private SO_GummingData data;
    [SerializeField] private SO_SpinDrillData spinDrillData;
    [SerializeField] private SO_DigData digData;

    [Space(5)][Header("Check Transforms")]
    [SerializeField] private Transform[] wallCheck;
    [SerializeField] private Transform ceilingCheck;
    #endregion

    #region Private variables
    private AbilityManager abilityManager;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private SpriteRenderer sr;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[10];

    private Vector2 workspace;
    #endregion

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new StateMachine();

        MoveState = new MoveState(this, "move");
        FallState = new FallState(this, "fall");
        DeadState = new DeadState(this, "");
        
        SpinDrillState = new SpinDrillState(this, spinDrillData, "spin");
        DigState = new DigState(this, digData, "dig");

        Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        FacingDirection = 1;
        CurrentAbility = Abilities.None;
    }

    private void Start()
    {
        abilityManager = AbilityManager.instance;

        HideOutline();

        StateMachine.Initialize(MoveState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

        CurrentVelocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
        CurrentVelocity = velocity;
    }

    public void SetVelocity(float velocity, Vector2 angle)
    {
        angle.Normalize();
        rb.velocity = velocity * angle;
        CurrentVelocity = rb.velocity;
    }

    public void SetPosition(Vector2 pos) => transform.position = pos;

    public void SetGravity(float gravity) => rb.gravityScale = gravity;

    public void AddForce(float force, Vector2 angle)
    {
        angle.Normalize();
        rb.AddForce(force * angle, ForceMode2D.Impulse);
        CurrentVelocity = rb.velocity;
    }

    public void SetLinearDrag(float dragValue)
    {
        rb.drag = dragValue;
    }

    public  void SetColliderEnabled(bool value) => col.enabled = value;

    public void SetRigidbodyType(RigidbodyType2D rbType) => rb.bodyType = rbType;

    public void SetCurrentAbility(Abilities ability) => CurrentAbility = ability;
    #endregion

    #region Check Functions
    public bool CheckIfGrounded()
    {
        int nbHits = col.Cast(Vector2.down, hits, Data.groundCheckDistance, true);

        if(nbHits > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("World")) return true;
            }
        }
        return false;
    }

    public bool CheckIfWall()
    {
        RaycastHit2D hitTop = Physics2D.Raycast(wallCheck[0].position, Vector2.right * FacingDirection, Data.wallCheckDistance, Data.worldLayer);
        RaycastHit2D hitBottom = Physics2D.Raycast(wallCheck[1].position, Vector2.right * FacingDirection, Data.wallCheckDistance, Data.worldLayer);

        return hitTop && hitBottom;
    }

    public bool CheckIfCeiling()
    {
        RaycastHit2D hit = Physics2D.Raycast(ceilingCheck.position, Vector2.up, Data.ceilingCheckDistance, Data.worldLayer);
        return hit;
    }
    #endregion

    #region Other Functions
    public void ChangeDirection()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180f, 0.0f);
        SetVelocityY(0.0f); //Reset Y velocity when flipping in case it was going up
    }

    private void ChangeToAbilityState()
    {
        switch (abilityManager.CurrentAbility.data.abilityName)
        {
            case Abilities.SpinDrill:
                StateMachine.ChangeState(SpinDrillState);
                break;
            case Abilities.Dig:
                StateMachine.ChangeState(DigState);
                break;
        }
    }

    private void ShowOutline()
    {
        sr.material.SetFloat("_Thickness", Data.outlineThickness);
    }

    private void HideOutline()
    {
        sr.material.SetFloat("_Thickness", 0f);
    }
    #endregion

    #region OnMouse Functions
    private void OnMouseOver()
    {
        // -- Conditions to select Gumming:
        // 1 : Gumming must be selectable (depending on current state) and
        // 2 : An ability must be selected and
        // 3 : No Gumming already selected (itself or another one) and
        // 4 : The selected ability is not the Gumming's current ability
        if (StateMachine.CurrentState.IsSelectable && abilityManager.CurrentGumming == null && abilityManager.CurrentAbility != null && abilityManager.CurrentAbility.data.abilityName != CurrentAbility)
        {
            abilityManager.SelectGumming(this);
            ShowOutline();
        }
        // -- Conditions to deselect Gumming:
        // 1 : Currently selected Gumming is this one and
        // 2 : Gumming is no longer selectable or
        // 3 : No ability selected
        // 4 : The selected ability is the Gumming's current ability
        if (abilityManager.CurrentGumming == this && (!StateMachine.CurrentState.IsSelectable || abilityManager.CurrentAbility == null || abilityManager.CurrentAbility.data.abilityName == CurrentAbility))
        {
            abilityManager.DeselectGumming();
            HideOutline();
        }
    }

    private void OnMouseExit()
    {
        if(abilityManager.CurrentGumming == this)
        {
            abilityManager.DeselectGumming();
            HideOutline();
        }
    }

    private void OnMouseDown()
    {
        // -- Conditions to set ability:
        // 1 : Gumming must be selectable (depending on current state)
        // 2 : An ability must be selected
        // 3 : The selected ability is not the gumming's current ability
        if (StateMachine.CurrentState.IsSelectable && abilityManager.CurrentAbility != null && abilityManager.CurrentAbility.data.abilityName != CurrentAbility)
        {
            ChangeToAbilityState();
            abilityManager.UseAbility();
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (debug && StateMachine != null) //'StateMachine != null' to avoid Unity errors when Game is not launched
        {
            Gizmos.color = Color.cyan;

            //Ground check Gizmo
            workspace.Set(transform.position.x - col.offset.x, transform.position.y + col.offset.y - Data.groundCheckDistance);
            Gizmos.DrawWireSphere(workspace, col.radius);

            //Wall checks Gizmos
            for (int i = 0; i < wallCheck.Length; i++)
            {
                workspace.Set(wallCheck[i].position.x + (Data.wallCheckDistance * FacingDirection), wallCheck[i].position.y);
                Gizmos.DrawLine(wallCheck[i].position, workspace);
            }

            //Ceiling check Gizmo
            workspace.Set(ceilingCheck.position.x, ceilingCheck.position.y + Data.ceilingCheckDistance);
            Gizmos.DrawLine(ceilingCheck.position, workspace);
        }
    }
}
