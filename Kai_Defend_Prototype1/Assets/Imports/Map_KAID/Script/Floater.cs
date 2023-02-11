using UnityEngine;

namespace Script
{
    public class Floater : MonoBehaviour
    {
        public new Rigidbody rigidbody;
        public float depthBeforeSubmerged = 1f;
        public float displacementAmount = 3f;
        public int floaterCount = 1;
        public float waterDrag = 0.99f;
        public float waterAngularDrag = 0.5f;

        private void FixedUpdate()
        {
            rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        
            var waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
            if (transform.position.y < waveHeight) 
            {
                var displacementMulitiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
                rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulitiplier, 0f),transform.position,ForceMode.Acceleration);
                rigidbody.AddForce(displacementMulitiplier * -rigidbody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                rigidbody.AddTorque(displacementMulitiplier * -rigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }
}
