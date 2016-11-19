using UnityEngine;
using UnityEngine.SceneManagement;

public class _zaszlo : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("_palyaValaszto");
    }
}
