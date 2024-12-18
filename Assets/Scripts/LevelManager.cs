using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {

        public static CinemachineConfiner2D currentConfiner;

        private CinemachineBrain brain;
        private CinemachineCamera cam;

        static BoxCollider2D currentSection;

        // Use this for initialization
        void Start()
        {
            brain = CinemachineBrain.GetActiveBrain(0);
            currentConfiner = GameObject.Find("CM").GetComponent<CinemachineConfiner2D>();

        }

        public static void ChangeSection(string sectionName)
        {
            currentSection = GameObject.Find(sectionName).GetComponent<BoxCollider2D>();

            if (currentSection != null)
            {
                currentConfiner.InvalidateBoundingShapeCache();

                currentConfiner.BoundingShape2D = currentSection;

                GameObject rightLimiter = GameObject.Find("Right");

                //Vector3
                rightLimiter.transform.position = new Vector3
                (
                    currentConfiner.BoundingShape2D.bounds.max.x,
                    rightLimiter.transform.position.y   
                );
            }
        }

    }
}