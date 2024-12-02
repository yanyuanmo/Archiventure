using UnityEngine;

namespace Archiventure 
{
    public class Tile : MonoBehaviour
    {
        [HideInInspector]
        public bool isOcupied;
        [HideInInspector]
        public bool stayingState;

        public Color placeIsOccupied;
        public Color placeIsNotOccupied;
        public Color usualColorOfBuilding;

        public GameObject building;

        public SpriteRenderer spriteOfBuilding;
        private GameManager gameManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void Update()
        {
            if (stayingState == true)
            {
                if (gameManager.destroyMode == true)
                {
                    spriteOfBuilding.color = placeIsOccupied;
                }
                else if (gameManager.replaceMode == true && gameManager.selectedBuilding != building)
                {
                    spriteOfBuilding.color = usualColorOfBuilding;
                }
                else if (gameManager.buildingSelected == true && gameManager.selectedBuilding != building)
                {
                    spriteOfBuilding.color = placeIsNotOccupied;
                }
                else
                {
                    spriteOfBuilding.color = usualColorOfBuilding;
                }
            }
            else
            {
                if (isOcupied == true)
                {
                    spriteOfBuilding.color = placeIsOccupied;
                }
                else
                {
                    spriteOfBuilding.color = placeIsNotOccupied;
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "IsOcupied")
            {
                isOcupied = true;
            }
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "IsOcupied" && isOcupied == true)
            {
                isOcupied = false;
            }
        }
        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == "IsOcupied")
            {
                isOcupied = true;
            }
        }
    }
}

