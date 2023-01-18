using Unity.VisualScripting;
using UnityEngine;

public class RubyController : MonoBehaviour{
    public float speed = 5.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;

    public int health{
        get{ return _currentHealth; }
    }

    private int _currentHealth;
    private bool _isInvincible;
    private float _invincibleTimer;

    private Rigidbody2D _rigidbody2d;

    private Animator _animator;
    private Vector2 _lookDirection = new Vector2(1, 0);

    private AudioSource _audioSource;
    private static readonly int Launch1 = Animator.StringToHash("Launch");
    private static readonly int LookX = Animator.StringToHash("Look X");
    private static readonly int LookY = Animator.StringToHash("Look Y");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start(){
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = maxHealth;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update(){
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)){
            _lookDirection.Set(move.x, move.y);
            _lookDirection.Normalize();
        }

        _animator.SetFloat(LookX, _lookDirection.x);
        _animator.SetFloat(LookY, _lookDirection.y);
        _animator.SetFloat(Speed, move.magnitude);

        var position = _rigidbody2d.position;

        position += move * (speed * Time.deltaTime);

        _rigidbody2d.MovePosition(position);

        if (_isInvincible){
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
                _isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C)){
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X)){
            var hit = Physics2D.Raycast(_rigidbody2d.position + Vector2.up * 0.2f, _lookDirection, 1.5f,
                LayerMask.GetMask("NPC"));
            if (hit.collider.IsUnityNull()) return;
            var character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character.IsUnityNull()) return;
            character.DisplayDialog();
        }
    }

    public void ChangeHealth(int amount){
        if (amount < 0){
            if (_isInvincible)
                return;
            _isInvincible = true;
            _invincibleTimer = timeInvincible;
            PlaySound(hitSound);
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        UIHealthBar.Instance.SetValue(_currentHealth / (float)maxHealth);
    }

    private void Launch(){
        var projectileObject =
            Instantiate(projectilePrefab, _rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);

        _animator.SetTrigger(Launch1);

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip){
        _audioSource.PlayOneShot(clip);
    }
}