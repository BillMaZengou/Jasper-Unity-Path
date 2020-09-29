using UnityEngine;

public class MovingSphere: MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;
    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    Vector3 velocity, desiredVelocity;
    Vector3 contactNormal;
    bool desiredJump;
    int jumpPhase;

    Rigidbody body;
    //bool onGround;
    int groundContactCount;
    bool OnGround => groundContactCount > 0;
    float minGroundDotProduct;

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        OnValidate();
    }

    private void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        desiredJump |= Input.GetButtonDown("Jump");

        GetComponent<Renderer>().material.SetColor(
            "_Color", Color.white * (groundContactCount * 0.25f)
        );
    }

    private void FixedUpdate()
    {
        //velocity = body.velocity;
        UpdateState();
        AdjustVelocity();
        //float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        //float maxSpeedChange = acceleration * Time.deltaTime;
        //velocity.x =
        //    Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        //velocity.z =
        //    Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        body.velocity = velocity;
        //onGround = false;
        ClearState();
    }

    void Jump()
    {
        //velocity.y += 5f;
        //if (onGround || jumpPhase < maxAirJumps)
        if (OnGround || jumpPhase < maxAirJumps)
        {
            jumpPhase++;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
            if (alignedSpeed > 0f)
            {
                //jumpSpeed -= velocity.y;
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            velocity += contactNormal * jumpSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    onGround = false;
    //}

    private void OnCollisionStay(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    void EvaluateCollision (Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            //onGround |= normal.y >= minGroundDotProduct;
            if (normal.y >= minGroundDotProduct)
            {
                //onGround = true;
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    void UpdateState()
    {
        velocity = body.velocity;
        //if (onGround)
        if (OnGround)
        {
            jumpPhase = 0;
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }

    Vector3 ProjectOnContactPlane (Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        //float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    void ClearState()
    {
        //onGround = false;
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }
}