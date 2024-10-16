using UnityEngine;
using UnityEngine.Events;

namespace Test
{
    public class AutoRun : MonoBehaviour
    {
        [SerializeField] private UnityEvent autorun;
        [SerializeField] private bool destroyObjectAfter;

        private void Start()
        {
            autorun.Invoke();
            if (destroyObjectAfter) Destroy(gameObject);
            else Destroy(this);
        }
    }
}
