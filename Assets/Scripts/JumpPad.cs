using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Zıplatma Ayarları")]
    public float jumpForce = 12f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // mevcut hızı al
            Vector3 vel = rb.linearVelocity;

            // düşüşü sıfırla
            vel.y = 0f;

            // yatayı koruyup yukarı sabit hız ver
            rb.linearVelocity = new Vector3(vel.x, jumpForce, vel.z);

            Debug.Log("Sabit zıplama uygulandı!");
        }
    }
}