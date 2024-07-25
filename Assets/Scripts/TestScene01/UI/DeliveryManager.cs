using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[System.Serializable]
public class DeliveryItem
{
    //Public Variable Fi
    [SerializeField]
    public bool Active;
    [SerializeField]
    public GameObject DeliveryLocation;
    [SerializeField]
    public GameObject Owner;
    [SerializeField]
    public float Timer;
    [SerializeField]
    public float TimeForDelivery;
    [SerializeField]
    public Transform transform;
    [SerializeField]
    public Material mat;
    public int meshIndex;

    private float value;
    private bool stolen;

    public DeliveryItem()
    {
        Active = false;
        DeliveryLocation = null;
        Timer = 30;
        TimeForDelivery = 30;
        transform = null;
        mat = null;
        meshIndex = 0;
        value = 3;
        stolen = false;
    }

    public float TotalPayment()
    {
        //float total = value;

        //if (!stolen) total++;

        //total += total * (Timer / TimeForDelivery);

        //total = Mathf.Round(total * 10.0f) * 0.1f;

        //return total;

        return value;
    }



    public void Stolen()
    {
        stolen = true;
    }

    //Sets the max time and timer
    public void SetTimer(float timer)
    {
        Timer = timer;
        TimeForDelivery = timer;
    }
}

//Kieran Coded
public class DeliveryManager : MonoBehaviour
{
    [HideInInspector] public bool spawningPoints;
    //[HideInInspector] public bool hasAnyActiveDeliveries;
    //PRIVATE
    //Used to store all the Pick Up points (Objects to pick the product up from)
    private List<GameObject> pickUpPoints = new List<GameObject>();


    //Used to store all the drop off points (Objects to deliver the products to)
    private List<GameObject> dropOffPoints;

    private Transform[] playerLocations;

    private PlayerScore[] playerScores;

    [SerializeField]
    float distanceTresholdPickUp = 50f;

    [SerializeField]
    float distanceTresholdDropOff = 350f;


    //Used to spawn in pick up in maps
    private Object genericPickUpPrefab;

    [SerializeField]
    private List<DeliveryItem> ItemsInWorld;  
  

    //PUBLIC
    //Used to store all active Pick Up points (Objects to pick the product up from)
    public List<GameObject> ActivePickUpPoints = new List<GameObject>();
    public GameObject StartingPickUp;

    //Used to store all active drop off points (Objects to deliver the products to)
    public List<GameObject> ActiveDropOffPoints = new List<GameObject>();
    //Used to know the active amount of points
    [SerializeField] public int ActiveAmount;
    [SerializeField] public DeliveryItem lastItem;

    [SerializeField] public ZoneMatSO zoneMaterials;

    public static DeliveryManager instance = null;    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        spawningPoints = true;
        ItemsInWorld = new List<DeliveryItem>();

        GameObject pickUpRoot = GameObject.FindGameObjectWithTag("PickUpPoint").transform.root.gameObject;

        for (int i = 0; i < pickUpRoot.GetComponentsInChildren<PickUp>().Length; i++)
        {
            pickUpPoints.Add(pickUpRoot.GetComponentsInChildren<PickUp>()[i].gameObject);
        }

        dropOffPoints = GameObject.FindGameObjectsWithTag("DropOffPoint").ToList<GameObject>();
        


        genericPickUpPrefab = pickUpPoints[0];

    }

    // Start is called before the first frame update
    void Start()
    {
        //sets up the pick up points
        for (int i = 0; i < pickUpPoints.Count; i++)
        {
            pickUpPoints[i].SetActive(false);
            pickUpPoints[i].GetComponent<PickUp>().PickUpID = i;
        }
        //Sets up the drop off points
        for (int i = 0; i < dropOffPoints.Count; i++)
        {
            dropOffPoints[i].SetActive(false);
            dropOffPoints[i].GetComponent<DropOff>().DropOffID = i;
        }

        if (zoneMaterials)
        {
            for (int i = 0; i < zoneMaterials.zoneMatsList.Count; i++)
            {
                zoneMaterials.zoneMatsList[i].active = false;
            }
        }

        for (int i = 0; i < ActiveAmount; i++)
        {
            //Actiavtes the pick up objects
            if(i == 0)
            {
                ActivatePickUp(0);
            }
            else
            {
                // For multiple deliveries
                ActivateRandomPickUp();
            }
            
        }

        //SpawnActiveZone(new Vector3(-848.29f, -387, 85.9f), true);

        GameObject[] tempPlayerObjects = GameObject.FindGameObjectsWithTag("Player");

        playerLocations = new Transform[tempPlayerObjects.Length];
        playerScores = new PlayerScore[tempPlayerObjects.Length];
        for(int i = 0; i < playerLocations.Length; i++)
        {
            playerLocations[i] = tempPlayerObjects[i].transform;
            playerScores[i] = tempPlayerObjects[i].GetComponent<PlayerScore>();
        }

    }


    //Activates one of the inactive drop off points and giving it an item to be delivered to
    //public DeliveryItem ActivateDropOff(DeliveryItem item)
    //{
    //    bool m_foundNewPoint = false;
    //    GameObject m_temp;
    //    do
    //    {
    //        //Gets the Drop off point
    //        m_temp = dropOffPoints[Random.Range(0, dropOffPoints.Count)];
    //        if (!m_temp.activeSelf)
    //        {
    //            //Activates point
    //            m_temp.SetActive(true);
    //            m_temp.GetComponent<DropOff>().ItemToBeDelivered = item;
    //            item.DeliveryLocation = m_temp;
    //            m_temp.GetComponent<MeshRenderer>().material = item.mat;
    //            m_foundNewPoint = true;
    //        }
    //    } while (!m_foundNewPoint);

    //    ActiveDropOffPoints.Add(m_temp);
    //    //Returns the drop off id of activated drop off point
    //    return m_temp.GetComponent<DropOff>().ItemToBeDelivered;
    //}

    //Activates one of the inactive drop off points and giving it an item to be delivered to
    public DeliveryItem ActivateRandomDropOff(DeliveryItem item)
    {
        bool m_foundNewPoint = false;
        GameObject m_temp;
        do
        {
            //Gets the Drop off point
            m_temp = dropOffPoints[Random.Range(0, dropOffPoints.Count)];
            if (!m_temp.activeSelf)
            {
                //Activates point
                m_temp.SetActive(true);
                m_temp.GetComponent<DropOff>().ItemToBeDelivered = item;
                item.DeliveryLocation = m_temp;
                if(ActiveAmount > 1)
                {
                    m_temp.GetComponentInChildren<MeshRenderer>().material = item.mat;
                }
                else
                {
                    m_temp.GetComponentInChildren<MeshRenderer>().material = zoneMaterials.defaultDropOffMat;
                }
                m_foundNewPoint = true;
            }
        } while (!m_foundNewPoint);

        ActiveDropOffPoints.Add(m_temp);
        //Returns the item to be delivered
        return m_temp.GetComponent<DropOff>().ItemToBeDelivered;
    }

    public DeliveryItem ActivateDropOff(DeliveryItem item)
    {
        //Looks for all possible zones based on a calculation
        List<GameObject> zones = availableDropOffs(item.transform.position);

        //If there wasnt a avaiable zone it then uses the entire list of zones
        if (zones == null)
        {
            Debug.Log("There was no available zones");
            RemoveItemFromList(item);
            return null;
           
        }

        GameObject m_temp;
        bool m_foundNewPoint = false;
        do
        {
          
            //Gets the Drop off point
            m_temp = zones[Random.Range(0, zones.Count)];
            if (!m_temp.activeSelf)
            {
                //Activates point
                m_temp.SetActive(true);
                m_temp.GetComponent<DropOff>().ItemToBeDelivered = item;
                item.DeliveryLocation = m_temp;
                if (ActiveAmount > 1)
                {
                    m_temp.GetComponentInChildren<MeshRenderer>().material = item.mat;
                }
                else
                {
                    m_temp.GetComponentInChildren<MeshRenderer>().material = zoneMaterials.defaultDropOffMat;
                }
                m_foundNewPoint = true;
            }
        } while (!m_foundNewPoint);

        ActiveDropOffPoints.Add(m_temp);
        //Returns the item to be delivered
        return m_temp.GetComponent<DropOff>().ItemToBeDelivered;
    }

    //Disables Drop off point
    public void DisableDropOffPoint(int num)
    {
        //Checks if valid drop off id
        if(num >= 0 && num < dropOffPoints.Count)
        {
            GameObject m_temp;
            m_temp = dropOffPoints[num];
            //Sets the material that was used to inactive
            if (zoneMaterials)
            {
                foreach (ZoneMats mat in zoneMaterials.zoneMatsList)
                {
                    if (mat.material == dropOffPoints[num].GetComponent<DropOff>().ItemToBeDelivered.mat)
                    {
                        mat.active = false;
                        break;
                    }
                }
            }
            //Disables the drop off point
            m_temp.SetActive(false);
            //removes from the list of active points
            ActiveDropOffPoints.Remove(m_temp);
        }  
    }

    //Activates one of the inactive pick up points
    public void ActivateRandomPickUp()
    {
        //Used to check for valid point
        bool m_foundNewPoint = false;
        GameObject m_temp;
        do
        {
            //Gets the pickup point
            //m_temp = pickUpPoints[Random.Range(0, pickUpPoints.Count - 1)];
            m_temp = pickUpPoints[Random.Range(0, pickUpPoints.Count - 1)];
            //Checks to see if already active
            if (!m_temp.activeSelf)
            {
                //Activates point
                m_temp.SetActive(true);
                DeliveryItem item = CreateItem(new DeliveryItem(), m_temp);

                if (ActiveAmount > 1)
                {
                    m_temp.GetComponentInChildren<MeshRenderer>().material = item.mat;
                }
                else
                {
                    m_temp.GetComponentInChildren<MeshRenderer>().material = zoneMaterials.defaultPickUpMat;
                }
                m_temp.GetComponent<PickUp>().SetDeliveryItem(item);
                //Point found
                m_foundNewPoint = true;
            }
        } while (!m_foundNewPoint);

        ActivePickUpPoints.Add(m_temp);
    }

    //Activates one of the inactive pick up points
    public void ActivatePickUp()
    {
        //Vector3 averagePositionPlayers = findAveragePlayersPosition();
        int currentIndex = 0;
        float lowestScore = 1000;
        Vector3 averagePositionPlayers = new Vector3(0,0,0);
        //Finds the player with the lowest score and stores the position
        foreach (PlayerScore score in playerScores)
        {
            if(score.GetScore() < lowestScore)
            {
                lowestScore = score.GetScore();
                 averagePositionPlayers = playerLocations[currentIndex].position;
            }
            currentIndex++;
        }    


        //List<GameObject> zones = availablePickUps();
        List<GameObject> zones = pickUpPoints;
        if (zones == null)
        {
            zones = pickUpPoints;
        }

        float distance = float.PositiveInfinity;
        GameObject currentZone = null;
        foreach(GameObject zone in zones)
        {
            if (!zone.activeSelf)
            {
                Vector3 zonePos = new Vector3(zone.transform.position.x, 0, zone.transform.position.z);
                Vector3 playerPos = new Vector3(averagePositionPlayers.x, 0, averagePositionPlayers.z);

                float temp = Vector3.Distance(playerPos, zonePos);

                if (temp < distanceTresholdPickUp)
                {
                    continue;
                }

                if (temp < distance)
                {
                    distance = temp;
                    currentZone = zone;
                }
            }
            
        }

        if (currentZone.gameObject == null)
        {
            return;
        }

        //Activates point
        currentZone.SetActive(true);

        DeliveryItem item = CreateItem(new DeliveryItem(), currentZone);

        if (ActiveAmount > 1)
        {
            currentZone.GetComponentInChildren<MeshRenderer>().material = item.mat;
        }
        else
        {
            currentZone.GetComponentInChildren<MeshRenderer>().material = zoneMaterials.defaultPickUpMat;
        }
        currentZone.GetComponent<PickUp>().SetDeliveryItem(item);

        ActivePickUpPoints.Add(currentZone);
    }

    public bool ActivateStartingPickUp()
    {
        if (StartingPickUp.IsUnityNull())
        {
            ActivateRandomPickUp();
            return false;
        }
        //Checks to see if pick up is already active
        if (!StartingPickUp.activeSelf)
        {
            ActivateRandomPickUp();
            return false;
        }

        //Sets up pick up
        StartingPickUp.SetActive(true);
        DeliveryItem item = CreateItem(new DeliveryItem(), StartingPickUp);

        if (ActiveAmount > 1)
        {
            StartingPickUp.GetComponentInChildren<MeshRenderer>().material = item.mat;
        }
        else
        {
            StartingPickUp.GetComponentInChildren<MeshRenderer>().material = zoneMaterials.defaultPickUpMat;
        }
        StartingPickUp.GetComponent<PickUp>().SetDeliveryItem(item);
        //Add pick up to current active
        ActivePickUpPoints.Add(StartingPickUp);

        return true;
    }

    public bool ActivatePickUp(int pickUp)
    {
        if(pickUp > pickUpPoints.Count - 1 || pickUp < 0)
        {
            return false;
        }
        //Checks to see if pick up is already active
        if (pickUpPoints[pickUp].activeSelf)
        {
            return false;
        }

        //Sets up pick up
        pickUpPoints[pickUp].SetActive(true);
        DeliveryItem item = CreateItem(new DeliveryItem(), pickUpPoints[pickUp]);

        if (ActiveAmount > 1)
        {
            pickUpPoints[pickUp].GetComponentInChildren<MeshRenderer>().material = item.mat;
        }
        else
        {
            pickUpPoints[pickUp].GetComponentInChildren<MeshRenderer>().material = zoneMaterials.defaultPickUpMat;
        }
        pickUpPoints[pickUp].GetComponent<PickUp>().SetDeliveryItem(item);
        //Add pick up to current active
        ActivePickUpPoints.Add(pickUpPoints[pickUp]);

        return true;
    }

    public void SpawnActiveZone(Vector3 position, bool active)
    {
        //Spawns new zone
        GameObject tempObject= Instantiate(genericPickUpPrefab, position, Quaternion.identity) as GameObject;

        tempObject.SetActive(false);       
        pickUpPoints.Add(tempObject);
        //Stores Id
        tempObject.GetComponent<PickUp>().PickUpID = pickUpPoints.Count-1;
        if (active)
        {
            //Activates Zone
            ActivatePickUp(pickUpPoints.Count - 1);
        }


    }

    //Disables pick up point
    public void DisablePickUpPoint(int num)
    {
        //Checks if valid pick up id
        if (num >= 0 && num < pickUpPoints.Count)
        {
            GameObject m_temp;
            m_temp = pickUpPoints[num];
            m_temp.SetActive(false);
            ActivePickUpPoints.Remove(m_temp);
        }
    }

    //Creates an item to be used in deliver manager
    //public DeliveryItem CreateItem()
    //{
    //    DeliveryItem item = new DeliveryItem();
    //    ItemsInWorld.Add(item);
    //    item.Active = true;
    //    item.Timer = 50;
    //    item.ID = 1;
    //    return item;
    //}

    //Creats a new item in the world
    public DeliveryItem CreateItem(DeliveryItem item, GameObject owner)
    {
        if(ItemsInWorld.Count == 0)
        {
            ItemsInWorld.Add(item);
        }
        else
        {
            bool isThereSpace = false;
            for(int i = 0; i < ItemsInWorld.Count; i++)
            {
                if (ItemsInWorld[i] == null)
                {
                    ItemsInWorld[i] = item;
                    isThereSpace = true;
                    break;
                }
            }
            if(isThereSpace == false)
            {
                ItemsInWorld.Add(item);
            }
        }

        item.Active = true;
        item.transform = owner.transform;
        item.Owner = owner;

        if (zoneMaterials)
        {
            foreach (ZoneMats mat in zoneMaterials.zoneMatsList)
            {
                if (!mat.active)
                {
                    item.mat = mat.material;
                    mat.active = true;
                    break;
                }
            }
        }
        return item;
    }

    //public void RemoveItemFromList(int id)
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        if (ItemsInWorld[i].ID == id)
    //        {
    //            ItemsInWorld.RemoveAt(i);
    //        }
    //    }
    //}

    public void RemoveItemFromList(DeliveryItem item)
    {
        for(int i = 0; i < ItemsInWorld.Count; i++)
        {
            if (ItemsInWorld[i] == item)
            {
                ItemsInWorld[i] = null;
            }
        }
        //ItemsInWorld.Remove(item);
    }

    public void RemoveAllItemsFromList()
    {
        ItemsInWorld.Clear();
    }

    //Checks if the item is in the world
    public bool IsItemInWorld(DeliveryItem item)
    {
        for(int i = 0; i < ItemsInWorld.Count; i++)
        {
            if(item == ItemsInWorld[i])
            {
                return true;
            }
        }    
        return false;
    }

    public GameObject GetDropOffPoint(DeliveryItem item)
    {
        for(int i = 0; i < ActiveDropOffPoints.Count; i++)
        {
            if (ActiveDropOffPoints[i].GetComponent<DropOff>().ItemToBeDelivered == item)
            {
                return ActiveDropOffPoints[i];
            }
        }
        return null;

    }

    public List<DeliveryItem> GetItemsInWorld()
    {
        return ItemsInWorld;
    }

    //Gets a position based on the average position of the players aswell as scaling it using the players score to give the lower score players and advantage
    private Vector3 findAveragePlayersPosition()
    {
        Vector3 result = Vector3.zero;
        float totalPlayers = 0;
        float maxScore = 0;

        foreach (PlayerScore scores in playerScores)
        {
            //gets the highest player score in the game
            maxScore += scores.GetScore() + 1;
        }

        int currentPlayer = 0;
        foreach (Transform location in playerLocations)
        {
            //Get a scale to multiply the position to lean more towards the losing players
            float scale = maxScore / (playerScores[currentPlayer].GetScore() + 1f);
            result += location.position * scale;
            currentPlayer++;
            totalPlayers += 1 * scale;
        }

        result.x /= totalPlayers;
        result.y /= totalPlayers;
        result.z /= totalPlayers;

        return result;
    }

    //Gets the available pickups based on the players positions
    private List<GameObject> availablePickUps()
    {
        List<GameObject> result = new List<GameObject>();
     
        foreach(GameObject zone in pickUpPoints)
        {
            bool isOutsideThreshold = true;
            foreach (Transform playerLoc in playerLocations)
            {
                float distance = Vector3.Distance(playerLoc.position, zone.transform.position);
                if(distance < distanceTresholdPickUp)
                {
                    isOutsideThreshold = false;
                    break;
                }
            }
            if (isOutsideThreshold)
            {
                result.Add(zone);
            }
        }

        
        if(result.Count == 0)
        {
            return null;
        }
        return result;
    }

    //Gets the available pickups based on the players positions
    private List<GameObject> availableDropOffs(Vector3 pickUpPosition)
    {
        List<GameObject> result = new List<GameObject>();

        //Checks to see if there is an available zone outside a certain range
        foreach (GameObject zone in dropOffPoints)
        {
            if (zone.activeSelf)
            {
                continue;
            }

            float distance = Vector3.Distance(pickUpPosition, zone.transform.position);

            if (distance > distanceTresholdDropOff)
            {
                result.Add(zone);
            }
        }

        //If it coudnt find a zone check through the list to find any available zone
        if (result.Count == 0)
        {
            bool checkIfAvailable = false;
            //Checks for a free zone to use
            foreach (GameObject zone in dropOffPoints)
            {
                if (!zone.activeSelf)
                {
                    checkIfAvailable = true;
                }
            }
            //If no drop off zone is availabe removes the item and returns null
            if (!checkIfAvailable)
            {
                return null;
            }
        }

        return result;
    }

    public void OvertimeRemoveActiveAmount()
    {        
        if (!AreThereItems())
        {
            GameManager.instance.gameHandler.EndGame();
        }        
    }

    public bool AreThereItems()
    {
        if(ItemsInWorld.Count==0)return false;

        foreach (DeliveryItem item in ItemsInWorld)
        {
            if(item != null) return true;
        }

        return false;
    }
    public Rigidbody GetLeadingPlayerExcludingSelf(Rigidbody rb)
    {
        float highestScore = -1f;
        int highestScoreIndex = 1;
        for (int i = 0; i < playerScores.Count(); i++)
        {
            if (rb == playerScores[i].GetComponent<Rigidbody>())
            {
                continue;
            }
            if (playerScores[i].score > highestScore)
            {
                highestScore = playerScores[i].score;
                highestScoreIndex = i;
            }
        }
        return playerScores[highestScoreIndex].GetComponent<Rigidbody>();
    }
    public Rigidbody GetSecondPlacePlayer()
    {
        return playerScores.OrderByDescending(r => r).Take(2).LastOrDefault().GetComponent<Rigidbody>();
    }
}