using UnityEngine;

public class NaturePlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 8f;
    public float acceleration = 50f;
    public float deceleration = 40f;

    [Header("Zıplama Ayarları")]
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    
    [Header("Referanslar")]
    public Transform visualRoot; // Karakter modelini (Görseli) buraya sürükle
    public LayerMask groundLayer; 
    
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded;
    private float inputX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        
        // Z eksenini ve dönmeleri fizik motoru için kilitle
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        // 1. Girişleri al
        inputX = Input.GetAxisRaw("Horizontal");

        // 2. Yer kontrolü
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f, groundLayer);

        // 3. Zıplama Girişi
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // 4. Görseli Döndürme (Sola ve Sağa Bakma)
        if (inputX > 0) 
        {
            // Sağa bak (90 derece)
            visualRoot.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (inputX < 0) 
        {
            // Sola bak (-90 veya 270 derece)
            visualRoot.localRotation = Quaternion.Euler(0, -180, 0);
        }

        // 5. Fonksiyonları Çağır
        UpdateAnimations();
        ApplySmoothJump();
    }

    void FixedUpdate()
    {
        // Yumuşak Yatay Hareket
        float targetSpeed = inputX * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        rb.AddForce(speedDif * accelRate * Vector3.right, ForceMode.Force);
    }

    void ApplySmoothJump()
    {
        // Yerçekimi hissini iyileştirme
        if (rb.linearVelocity.y < 0) 
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0)
{
    rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
}
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        anim.SetFloat("SpeedX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsJumping", !isGrounded && rb.linearVelocity.y > 0.1f);
        anim.SetFloat("VelocityY", rb.linearVelocity.y);
    }
}