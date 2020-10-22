using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawningScript : MonoBehaviour {

    public GameObject fish;


    Vector2 point1;
    Vector2 point2;

    Vector2 whereToSpawn;
    


    //every 5 seconds
    public float spawnRate = 5.0f;

    //every half second
    public float decrementRate = .5f;

    float nextSpawn = 0.0f;
    float fishBarNegativeDecrement = 0.0f;


    //int numberToSpawn = 10;

    public List<GameObject> fishes;

    public bool globFishEventOccuring = false;

    public GameObject FishSnootBar;

    public bool canBeSnooted;

    public GameObject inventoryButton;

    //IMPORANT VALUES FOR EVERY SPAWNER

    //Number of fish the area starts with already having spawned
    public int NumOfFishAtBeginning = 1;

    //Number of fish max able to be present
    public int NumOfFishMax = 3;

    //a boolean that will also be checked, true when the fish event is occuring AND it is related to a fish created by THIS spawner
    public bool fishEventForThisSpawner = false;

    public bool StopAllSpawnerAction = false;

    public GameObject FishSpawnManager;

    public int randomChanceAFishSpawnsWhenAble = 1;

    //THE SEED for the random stuff for this spawner. Initialized to 1331 but is taken as an input and overriden for each spawner
    public int seed = 1331;

    //Implement this later: generalized spawning

    //detect clicks on fishes
    int indexOfList = 0;

    // Use this for initialization

    public GameObject backpack;

    //SOUND FX
    public AudioSource splashSound;
    public AudioSource[] tapSounds = new AudioSource[3];

    System.Random r;

    //object repreenting the successfulcatch menu
    GameObject catchMenuSuccess;

    //object representing the failed catch menu (player failed to catch the fish within the time limit/game mechanics)
    GameObject failedCatchMenu;

    public Inventory inv;
    void Start () {
        
         for (int i = 0; i < NumOfFishAtBeginning; i++)
            {
                point1 = new Vector2(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
                    Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));

                GameObject f = Instantiate(fish, point1, Quaternion.identity);
                f.name = fish.GetComponent<Item>().name;
                fishes.Add(f);
                canBeSnooted = true;
            }
        inv = backpack.GetComponent<Inventory>();
        r = new System.Random(seed);
        catchMenuSuccess = FishSpawnManager.GetComponent<ManageFishSpawners>().successfulCatchMenu;
        failedCatchMenu = FishSpawnManager.GetComponent<ManageFishSpawners>().failedCatchMenu;

    }
	
	// Update is called once per frame
	void Update () {
        combThroughFishDestroySpecifiedOnes();
        //perpetually set canbesnootedValue such that it means the pause isnt active
        canBeSnooted = FishSpawnManager.GetComponent<PauseScript>().isPaused;

        //combThroughFishDestroySpecifiedOnes();
        bool globFishEventOccuring = FishSpawnManager.GetComponent<ManageFishSpawners>().GLOBALFISHEVENT;


        //check if we have less than the max number of fish
        if (fishes.Count - 1 > NumOfFishMax)
        {
            //dont spawn any more
        }
        else
        {
            

            //we want to spawn another fish if we have waited long enough, AND player is not trying to catch a fish
            if (Time.time > nextSpawn && !globFishEventOccuring)
                {
                if(shouldAFishSpawn())
                {
                    nextSpawn = Time.time + spawnRate;
                    point1 = new Vector2(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
                    Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y));

                    GameObject f = Instantiate(fish, point1, Quaternion.identity);
                    f.name = fish.GetComponent<Item>().name;
                    fishes.Add(f);
                    canBeSnooted = true;
                }
                   else
                {
                    nextSpawn = Time.time + spawnRate;
                }
                }
        }

            //make sure fish are moving when they should be, fish stop moving when player has targeted one, start moving again when the fish even is complete
            if (!globFishEventOccuring && !checkIfAllFishMoving())
        {
            //means fish are not moving when they should be
            startFishMoving();
        }

            //target a fish
            if (!globFishEventOccuring)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    foreach (GameObject f in fishes)
                    {

                        Vector3 mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        BoxCollider2D collider = f.GetComponent<BoxCollider2D>();
                        if (collider.OverlapPoint(mouseClick) && !canBeSnooted)
                        {
                            indexOfList = fishes.IndexOf(f);
                            
                            f.GetComponent<MovementScript>().setSpeed(5.0f);
                            f.GetComponent<MovementScript>().isBeingSnooted = true;
                            FishSpawnManager.GetComponent<ManageFishSpawners>().GLOBALFISHEVENT = true;
                            fishEventForThisSpawner = true;
                            //set this fish to be above water and thus more visible
                            fishes[indexOfList].GetComponent<SpriteRenderer>().sortingOrder = 5;
                            foreach (GameObject killFish in fishes)
                            {
                                if (fishes.IndexOf(killFish) != indexOfList)
                                {
                                    killFish.GetComponent<MovementScript>().setMove(false);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            if (globFishEventOccuring && !fishEventForThisSpawner)
        {
            stopFishMoving();
        }
            //modify the fish bar accordingly
            if (globFishEventOccuring && fishEventForThisSpawner)
            {
            
            if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    BoxCollider2D collider = fishes[indexOfList].GetComponent<BoxCollider2D>();
                    if (collider.OverlapPoint(mouseClick) && !canBeSnooted)
                    {
                        //successfully clicked fish after target was already established
                        FishSnootBar.SetActive(true);

                        Transform bar = FishSnootBar.transform.Find("Bar");
                    //play tap sound
                    int chooseTapSoundRand = r.Next(tapSounds.Length);
                    tapSounds[chooseTapSoundRand].Play();
                        if (bar.localScale.x + .1f < 1f)
                        {
                            bar.localScale = new Vector3(bar.localScale.x + .1f, 1f);
                        }
                        else
                        {

                            //TODO: implement this so that research points come from fish instead of being a fixed value
                            NotificationScriptManager.Instance.SetNewNotification("You earned 5 research points!");
                        

                            //play splash sound
                            splashSound.Play();

                            //You caught the fish and it leaves the area
                            Item itemToAdd = fishes[indexOfList].GetComponent<Item>();
                            inv.AddItem(itemToAdd.gameObject, itemToAdd.ID, itemToAdd.type, itemToAdd.description, itemToAdd.icon);
                            //player now has the fish in their inventory

                            //also, let's save the inventory
                            SaveSystem.SaveCatchInventoryData(inv);
                            FishSnootBar.SetActive(false);

                            //before destroying the fish I need to save the string and image in order to put it on the catch success menu screen
                            string fishName = (fishes[indexOfList]).GetComponent<Item>().name;
                            Sprite fishSprite = (fishes[indexOfList]).GetComponent<Item>().icon;
                            Destroy(fishes[indexOfList]);
                            fishes.RemoveAt(indexOfList);
                            indexOfList = 0;
                            FishSpawnManager.GetComponent<ManageFishSpawners>().GLOBALFISHEVENT = false;
                            fishEventForThisSpawner = false;

                            bar.localScale = new Vector3(.2f, 1f);
                            foreach (GameObject restoreFish in fishes)
                            {
                                restoreFish.GetComponent<MovementScript>().setMove(true);
                            }
                        
                            //pasue game
                            FishSpawnManager.GetComponent<PauseScript>().pauseGame();
                            //now show the congratulations screen
                            catchMenuSuccess.SetActive(true);
                            //enter the correct values of the fish
                            (catchMenuSuccess.transform.GetChild(5)).GetComponent<Text>().text = fishName;
                            (catchMenuSuccess.transform.GetChild(2)).GetComponent<Image>().sprite = fishSprite;

                    }

                }
                
                }
            else
            {
                //PLAYER HAS NOT TAPPED FISH THIS UPDATE CALL
                if (Time.time > fishBarNegativeDecrement)
                {
                    fishBarNegativeDecrement = Time.time + decrementRate;
                    Transform bar = FishSnootBar.transform.Find("Bar");
                    //do fish bar decrement. fish bar goes down some portion, and fish may swim away if player doesnt catch it
                    if (bar.localScale.x - .01f < 0f)
                    {
                        fishRanAway();
                    }
                    else
                    {
                        bar.localScale = new Vector3(bar.localScale.x - .01f, 1f);
                    }
                }
            }
            }

    }
    //Adding a function to stop all the fish from moving except the one the button was pressed for
    public void stopFishMoving()
    {
        foreach(GameObject f in fishes)
        {
            f.GetComponent<MovementScript>().setMove(false);
        }
    }
    //Adding a function to start all the fish from moving again
    public void startFishMoving()
    {
        foreach (GameObject f in fishes)
        {
            f.GetComponent<MovementScript>().setMove(true);
        }
    }
    public bool checkIfAllFishMoving()
    {
        
        foreach (GameObject f in fishes)
        {
            
            if (f.GetComponent<MovementScript>().move == false)
            {
                return false;
            }
        }
        return true;
    }
    public bool shouldAFishSpawn()
    {
        int c = r.Next(100);
        if(c < randomChanceAFishSpawnsWhenAble)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void combThroughFishDestroySpecifiedOnes()
    {
        for(int i = 0; i < fishes.Count; i++)
        {
            
            if(fishes[i].GetComponent<Item>().shouldBeDestroyed)
            {
                if(fishes[i].GetComponent<MovementScript>().isBeingSnooted)
                {
                    fishRanAway();
                }
                else
                {
                    //otherwise just quietly delete
                    Destroy(fishes[i]);
                    fishes.RemoveAt(i);
                    return;
                }
            }
        }
    }
    
    void fishRanAway()
    {
        Transform bar = FishSnootBar.transform.Find("Bar");
        //despawn fish AND show fish got away screen
        Destroy(fishes[indexOfList]);
        fishes.RemoveAt(indexOfList);
        indexOfList = 0;
        FishSpawnManager.GetComponent<ManageFishSpawners>().GLOBALFISHEVENT = false;
        fishEventForThisSpawner = false;

        bar.localScale = new Vector3(0.2f, 1f);
        foreach (GameObject restoreFish in fishes)
        {
            restoreFish.GetComponent<MovementScript>().setMove(true);
        }
        //pasue game
        FishSpawnManager.GetComponent<PauseScript>().pauseGame();
        //now show the congratulations screen
        failedCatchMenu.SetActive(true);
        FishSnootBar.SetActive(false);
    }
}
