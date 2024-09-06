using UnityEngine;

public class CubesKiller : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Cube>(out Cube cube))
        {
            cube.Die();
        }
    }
}
