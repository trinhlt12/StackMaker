namespace _GAME.Scripts.Camera
{
    using UnityEngine;

    public class FollowCamera : MonoBehaviour

    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3   offset      = new Vector3(0, 10f, -10f);
        [SerializeField] private float     followSpeed = 5f;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            this.transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
    }
}