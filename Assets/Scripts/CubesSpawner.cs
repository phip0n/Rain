using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private float _spawnTime;
    [SerializeField] private int _cubesPerSpawn;
    [SerializeField] private Vector3 _spawnSize;
    [SerializeField] private int _poolSize = 100;
    private bool _isCreating = false;
    private ObjectPool<Cube> _cubesPool;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(
            createFunc: () => CreateCube(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => cube.Destroy(),
            defaultCapacity: _poolSize,
            maxSize: _poolSize,
            collectionCheck: true);
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void ActionOnGet(Cube cube)
    {
        cube.SetActive(true);

        if (_isCreating)
        {
            _isCreating = false;
            cube.Init();
        }
        else
        {
            cube.Init(GetSpawnPosition());
        }

        cube.Dying += ReleaseCube;
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.Dying -= ReleaseCube;
        cube.SetActive(false);
    }

    private Cube CreateCube()
    {
        _isCreating = true;
        Cube cube = Instantiate(_cube, GetSpawnPosition(), Quaternion.identity);
        return cube;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 position = new Vector3(Random.Range(-_spawnSize.x, _spawnSize.x), Random.Range(-_spawnSize.y, _spawnSize.y), Random.Range(-_spawnSize.z, _spawnSize.z));
        position += transform.position;
        return position;
    }

    private void ReleaseCube(Cube cube)
    {
        _cubesPool.Release(cube);
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds time = new WaitForSeconds(_spawnTime);

        while (this.enabled)
        {
            yield return time;

            for (int i = 0; i < _cubesPerSpawn; i++)
            {
                _cubesPool.Get();
            }
        }
    }
}
