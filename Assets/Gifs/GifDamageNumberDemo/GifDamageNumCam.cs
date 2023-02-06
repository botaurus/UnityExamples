using UnityEngine;

namespace Gifs.GifDamageNumberDemo
{
    public class GifDamageNumCam : MonoBehaviour
    {
        private Camera _cam;
        public float GrowSpeed = 1;
        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Update()
        {
            _cam.orthographicSize += Time.deltaTime * GrowSpeed;
        }
    }
}