using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;


namespace PixelArsenal
{
    public class PixelArsenalLightFade : MonoBehaviour
    {
        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private Light2D li;
        private float initIntensity;

        // Use this for initialization
        void Awake()
        {
            if (gameObject.GetComponent<Light2D>())
            {
                li = gameObject.GetComponent<Light2D>();
                initIntensity = li.intensity;
            }
            else
                print("No light object found on " + gameObject.name);
        }

        private void OnEnable()
        {
            li.intensity = initIntensity;
        }

        // Update is called once per frame
        void Update()
        {
            if (li.intensity > 0)
                li.intensity -= initIntensity * (Time.deltaTime / life);
            //if (killAfterLife && li.intensity <= 0)
            //    Destroy(gameObject);
            //if (gameObject.GetComponent<Light2D>())
            //{
            //    li.intensity -= initIntensity * (Time.deltaTime / life);
            //    if (killAfterLife && li.intensity <= 0)
            //        Destroy(gameObject);
            //}
        }
    }
}