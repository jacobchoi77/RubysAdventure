using UnityEngine;

public class EnemyController : MonoBehaviour{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;

    private Rigidbody2D _rigidbody2D;
    private float _timer;
    private int _direction = 1;
    private bool _broken = true;

    private Animator _animator;
    private static readonly int MoveX = Animator.StringToHash("Move X");
    private static readonly int MoveY = Animator.StringToHash("Move Y");
    private static readonly int Fixed = Animator.StringToHash("Fixed");

    private void Start(){
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _timer = changeTime;
        _animator = GetComponent<Animator>();
    }

    private void Update(){
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!_broken) return;
        _timer -= Time.deltaTime;
        if (_timer < 0){
            _direction = -_direction;
            _timer = changeTime;
        }

        var position = _rigidbody2D.position;
        if (vertical){
            position.y += Time.deltaTime * speed * _direction;
            _animator.SetFloat(MoveX, 0);
            _animator.SetFloat(MoveY, _direction);
        }
        else{
            position.x += Time.deltaTime * speed * _direction;
            _animator.SetFloat(MoveX, _direction);
            _animator.SetFloat(MoveY, 0);
        }

        _rigidbody2D.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other){
        var player = other.gameObject.GetComponent<RubyController>();
        if (player != null){
            player.ChangeHealth(-1);
        }
    }

    public void Fix(){
        _broken = false;
        _rigidbody2D.simulated = false;
        _animator.SetTrigger(Fixed);
        smokeEffect.Stop();
    }
}