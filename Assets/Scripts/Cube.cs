using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof (Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minDyingTime = 2;
    [SerializeField] private float _maxDyingTime = 5;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _waitForDeath;
    private bool _isDying = false;

    public event UnityAction<Cube> Dying;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDying == false && collision.gameObject.TryGetComponent<CubesKiller>(out CubesKiller cubeskiller))
        {
            Die();
        }
    }

    public void Init()
    {
        if (_waitForDeath != null)
        {
            StopCoroutine(_waitForDeath);
        }

        transform.rotation = Quaternion.Euler(Vector3.zero);
        _renderer.material.color = Color.white;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _isDying = false;
    }

    public void Init(Vector3 position)
    {
        Init();
        transform.position = position;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void Die()
    {
        _isDying = true;
        float dyingTime = Random.Range(_minDyingTime, _maxDyingTime);
        _renderer.material.color = Random.ColorHSV();
        _waitForDeath = StartCoroutine(WaitForDeath(dyingTime));
    }

    private IEnumerator WaitForDeath(float time)
    {
        WaitForSeconds dyingTime = new WaitForSeconds(time);
        yield return dyingTime;
        Dying?.Invoke(this);
    }    
}
