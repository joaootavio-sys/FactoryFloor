using UnityEngine;

public class tiro : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * -5f; // Ajuste a velocidade conforme necessário
        Destroy(gameObject, 5f); // Destrói o projétil após 5 segundos)
    }
        
}
