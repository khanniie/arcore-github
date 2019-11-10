namespace GoogleARCore.Examples.AddAnchorsEverywhere
{
    using System.Collections;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class AddAnchorsEverywhere : MonoBehaviour
    {

        public GameObject prefabObject; //the game object being cloned upon tap
        public int distanceFromCamera = 1; //how far away the object is placed when cloned
        public Text textIndicator; //used to indicate how many seconds left until deletion
        public float timeUntilRemove = 10.0f; //number of seconds of inactivity until deletion

        private ArrayList clones; //list to track the gameobjects that we've added to the scene
        private float originalTimeUntilRemove; //a number used to reset the time

        /* Update is called once per frame
        * If the user is tapping, we call AddObject()
        * Then, we call UpdateState to update our counter for deleting objects */
        void Update()
        {
            /* the below code checks, at compile time, if we are in the unity editor,
            * and if we are, we check for the mouse down button
            * otherwise, we are looking for finger touches instead, and then it compiles the code accordingly 
            * Google C# Preprocessor directives for more info if interested*/
    #if UNITY_EDITOR
            if (Input.GetMouseButtonDown (0)) 
    #else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    #endif
            {
                AddObject();
            }
            UpdateState();
        }

        /* Awake is called once upon the beginnings*/
        void Awake()
        {
            clones = new ArrayList();
            originalTimeUntilRemove = timeUntilRemove;
        }

        /* Called in the Update functino
        * Clones the prefabObject using Instantiate
        * The new object position is projceted out forwards by distanceFromCamera amount
        * It also initializes the clones list and sets the value of originalTimeUntilRemove */
        void AddObject()
        {
            GameObject clone = Instantiate(prefabObject, Camera.main.transform.position + (this.distanceFromCamera * Camera.main.transform.forward), Quaternion.identity);
            clones.Add(clone);
            timeUntilRemove = originalTimeUntilRemove;
        }

        //Updates the state by updating the time counter, and deleting the first object in our clones list if time is up
        void UpdateState()
        {
            //just remove anchors after a certain amount of time for example's sake.
            timeUntilRemove -= Time.deltaTime;
            textIndicator.text = "Time until oldest anchor removed: " + (int)timeUntilRemove + "s";
            if (timeUntilRemove <= 0.0f && clones.Count > 0)
            {
                GameObject clone = (GameObject)clones[0];
                clones.RemoveAt(0);
                timeUntilRemove = originalTimeUntilRemove;
            }
        }
    }
}
