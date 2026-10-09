using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    [SerializeField] float jumpForce;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            Rigidbody2D player = other.GetComponent<Rigidbody2D>();

            if (player != null)
            {
                player.linearVelocity = new Vector2(player.linearVelocity.x, 0f);
                player.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
