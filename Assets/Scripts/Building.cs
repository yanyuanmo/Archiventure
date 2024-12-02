using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Archiventure
{
    public class Building : MonoBehaviour
    {
        public enum BuildingState { staying, replacing, updating, firstStart, destroying };
        [HideInInspector]
        public BuildingState buildingState;
        public BuildingType buildingType;
        [HideInInspector]
        public BuildingData buildingData;

        //Variables
        [HideInInspector]
        public bool notToMove;
        [HideInInspector]
        public bool mouseDrag;
        [HideInInspector]
        Vector3 currentPosition;
        private float zCordinateOfMouse;
        private Vector3 mouseOffset;

        [Header("Game Objects")]
        public Button yesButton;
        public GameObject yesNoPanel;
        public GameObject arrows;
        public GameObject updatePanel;

        [Header("Scripts")]
        private GameManager gameManager;
        private SortingLayers slayers;
        public Tile tile;
        private SpriteRenderer spriteRenderer;

        [System.Serializable]
        public class Update
        {
            public Sprite updateLevelSprite;
        }
        public Update[] updates;
        public int nextUpdate;


        void Start()
        {
            //Set currentPosition to current position of building 
            currentPosition = transform.position;

            //Get the scripts
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            slayers = GetComponent<SortingLayers>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void FixedUpdate()
        {
            if (buildingState == BuildingState.staying)
            {
                yesNoPanel.SetActive(false);
                arrows.SetActive(false);
                updatePanel.SetActive(false);
                slayers.number = -0.2f;
                tile.stayingState = true;
            }
            if (buildingState == BuildingState.replacing)
            {
                yesNoPanel.SetActive(true);
                arrows.SetActive(true);
                updatePanel.SetActive(false);
                slayers.number = 0.2f;
                tile.stayingState = false;

                if (tile.isOcupied == true)
                {
                    yesButton.interactable = false;
                }
                else
                {
                    yesButton.interactable = true;
                }
            }
            if (buildingState == BuildingState.updating)
            {
                yesNoPanel.SetActive(false);
                arrows.SetActive(false);
                if (nextUpdate + 1 != updates.Length)
                {
                    updatePanel.SetActive(true);
                }
                else
                {
                    updatePanel.SetActive(false);
                }

                slayers.number = 0.2f;
                tile.stayingState = true;
            }
            if (buildingState == BuildingState.firstStart)
            {
                yesNoPanel.SetActive(true);
                arrows.SetActive(true);
                updatePanel.SetActive(false);
                slayers.number = 0.2f;
                tile.stayingState = false;
            }
            if (buildingState == BuildingState.destroying)
            {
                yesNoPanel.SetActive(true);
                arrows.SetActive(false);
                updatePanel.SetActive(false);
                slayers.number = 0.2f;
                tile.stayingState = true;
            }
            if (gameManager.selectedBuilding != gameObject)
            {
                buildingState = BuildingState.staying;
            }

            spriteRenderer.sprite = updates[nextUpdate].updateLevelSprite;

        }

        void OnMouseDown()
        {
            zCordinateOfMouse = Camera.main.WorldToScreenPoint(transform.position).z;
            mouseOffset = transform.position - mouseWorldPos();

            if (gameManager.buildingSelected == false)
            {
                if (gameManager.replaceMode == true)
                {
                    buildingState = BuildingState.replacing;
                    gameManager.buildingSelected = true;
                    mouseDrag = true;
                }
                else if (gameManager.destroyMode == true)
                {
                    buildingState = BuildingState.destroying;
                    gameManager.buildingSelected = true;
                    mouseDrag = false;
                }
                else
                {
                    if (nextUpdate + 1 != updates.Length)
                    {
                        buildingState = BuildingState.updating;
                        gameManager.buildingSelected = true;
                    }
                    mouseDrag = false;
                }
                gameManager.selectedBuilding = gameObject;

            }
        }

        void OnMouseDrag()
        {
            if (mouseDrag == true)
            {
                transform.position = mouseWorldPos() + mouseOffset;
            }
        }

        private Vector3 mouseWorldPos()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = zCordinateOfMouse;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

        public void UpdateButton()
        {
            nextUpdate++;
            buildingState = BuildingState.staying;
            gameManager.selectedBuilding = null;
            gameManager.buildingSelected = false;

            int numberInArray1 = NumberOfElementFromSaveArray();

            gameManager.save.buildings[numberInArray1].level = nextUpdate;
            gameManager.OnSave();
        }

        public void YesButton()
        {
            if (buildingState == BuildingState.firstStart)
            {
                buildingState = BuildingState.staying;
                currentPosition = transform.position;
                mouseDrag = false;
                gameManager.mainPanel.SetActive(true);

                buildingData.buildingType = buildingType;
                buildingData.position = transform.position;
                buildingData.rotation = transform.rotation;
                buildingData.level = nextUpdate;
                gameManager.save.buildings.Add(buildingData);
                gameManager.OnSave();
            }
            if (buildingState == BuildingState.replacing)
            {
                buildingState = BuildingState.staying;
                mouseDrag = false;
                gameManager.mainPanel.SetActive(false);

                int numberInArray2 = NumberOfElementFromSaveArray();

                gameManager.save.buildings[numberInArray2].position = transform.position;
                gameManager.save.buildings[numberInArray2].rotation = transform.rotation;
                gameManager.OnSave();

                currentPosition = transform.position;
            }
            if (buildingState == BuildingState.destroying)
            {
                buildingState = BuildingState.staying;
                mouseDrag = false;
                gameManager.mainPanel.SetActive(false);

                int numberInArray3 = NumberOfElementFromSaveArray();

                Destroy(gameObject);
                gameManager.save.buildings.Remove(gameManager.save.buildings[numberInArray3]);
                gameManager.OnSave();
            }
            gameManager.buildingSelected = false;
            gameManager.selectedBuilding = null;
        }

        public void NoButton()
        {
            if (buildingState == BuildingState.firstStart)
            {
                Destroy(gameObject);
                gameManager.mainPanel.SetActive(true);
            }
            if (buildingState == BuildingState.replacing)
            {
                transform.position = currentPosition;
                buildingState = BuildingState.staying;
                mouseDrag = false;
            }
            if (buildingState == BuildingState.destroying)
            {
                buildingState = BuildingState.staying;
                gameManager.mainPanel.SetActive(false);
                mouseDrag = false;
            }
            gameManager.buildingSelected = false;
            gameManager.selectedBuilding = null;
        }

        private int NumberOfElementFromSaveArray()
        {
            for (int i = 0; i < gameManager.save.buildings.Count; i++)
            {
                if (gameManager.save.buildings[i].position == currentPosition)
                {
                    return i;
                }
            }
            return 0;
        }

        public void ResetCanvas()
        {
            buildingState = BuildingState.staying;
        }
    }
}

