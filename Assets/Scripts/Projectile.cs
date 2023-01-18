using UnityEngine;

public class Projectile : MonoBehaviour{
    private Rigidbody2D _rigidbody2d;

    private void Awake(){
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force){
        _rigidbody2d.AddForce(direction * force);
    }

    private void Update(){
        if (transform.position.magnitude > 1000.0f){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        var e = other.collider.GetComponent<EnemyController>();
        if (e != null){
            e.Fix();
        }

        Destroy(gameObject);
    }
}