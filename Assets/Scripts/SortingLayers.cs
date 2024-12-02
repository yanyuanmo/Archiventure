using UnityEngine;

namespace Archiventure 
{
    public class SortingLayers : MonoBehaviour
    {
        private SpriteRenderer sprite;
        public float number;
        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            sprite.sortingOrder = (int)((transform.position.y - number) * (-100));
        }
    }
}

