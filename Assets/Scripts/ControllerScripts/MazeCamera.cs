using UnityEngine;

namespace ControllerScripts
{
    public class MazeCamera : MonoBehaviour
    {
        private Vector3 _destinationPoint;

        private GameObject _camera;
        // Use this for initialization
        void Start()
        {
            _camera = FindObjectOfType<Camera>().gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                _camera.transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, -10);
            }

            else if (Input.GetKey(KeyCode.Keypad5))
            {
                _camera.transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, -10);
            }

            else if (Input.GetKey(KeyCode.Keypad4))
            {
                _camera.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y, -10);
            }

            else if (Input.GetKey(KeyCode.Keypad6))
            {
                _camera.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y, -10);
            }
            else if (Input.GetKey(KeyCode.KeypadMinus))
            {
                if (_camera.GetComponent<Camera>().orthographicSize>0)
                    _camera.GetComponent<Camera>().orthographicSize-=0.5f;
            }
            else if (Input.GetKey(KeyCode.KeypadPlus))
            {
                _camera.GetComponent<Camera>().orthographicSize += 0.5f;
            }

        }
    }
}
