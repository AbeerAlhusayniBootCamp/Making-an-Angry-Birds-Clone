using System.Collections;
using UnityEngine;

public class MovingBall : MonoBehaviour
{
    //GameObject
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject HingeJoint;
    //Rigitbodys
    [SerializeField]private Rigidbody2D theBall;
     private SpringJoint2D theBallSpringJoint;
    //time before we despawn the ball
   [SerializeField] private float ballDespawnTime;
    [SerializeField] private float releaseDelay=0.15f;
   [SerializeField] private float respawnTime;

    private Camera mainCamera;
    private bool isPressed=false;
    private bool disableTouchInput = false;


    void Start()
    {
        mainCamera = Camera.main;
        SpawnBall();
    }

    void Update()
    {
        //if (theBall == null)
        //{
        //    return;
        //}
        //if (disableTouchInput)
        //{
        //    return;
        //}
        //if (!Touchscreen.current.primaryTouch.press.isPressed)
        //{
        //    if (isFingerDown)
        //    {
        //        Launch();
        //    }
        //    return;
        //}
        //PullBack();

        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    UpdateBallPosition();
                    break;
                case TouchPhase.Moved:
                    UpdateBallPosition();
                    break;
                case TouchPhase.Stationary:
                    UpdateBallPosition(); 
                    break;
                case TouchPhase.Ended:
                    ReleaseFinger();
                    break;
                case TouchPhase.Canceled:
                    ReleaseFinger();
                    break;
            }
        }
    }
    private void UpdateBallPosition()
    {
        isPressed = true;
        //get the Vector2 position of the first finger on the screen (touch 0)
        Vector2 screenTouchPosition = Input.GetTouch(0).position;
        //translate the screen position to a world position
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenTouchPosition);
        //put the ball at the WorldPosition location
        theBall.transform.position = worldPosition;
        theBall.bodyType = RigidbodyType2D.Kinematic;
    }
    private void ReleaseFinger()
    {
        isPressed = false;
        //make the ball dynamic
        theBall.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(DelayRelease());
    }
    private void SpawnBall()
    {
        //spawn ballPrefab at the position of the HingeJoint
        GameObject newBall = Instantiate(ballPrefab, HingeJoint.transform.position, Quaternion.identity);
        //set theBall global variable to the newBall's Rigidbody2D
        theBall = newBall.GetComponent<Rigidbody2D>();
        //theBallSpringJoint will now reference the SpringJoint2D from the newBall GameObject
        theBallSpringJoint = newBall.GetComponent<SpringJoint2D>();
        //set the ConnectedBody of theBallSpringJoint to the RigidBody2D of the HingeJoint GameObject
        theBallSpringJoint.connectedBody = HingeJoint.GetComponent<Rigidbody2D>();
    }
    private IEnumerator DelayRelease()
    {
        disableTouchInput = true;
        yield return new WaitForSeconds(releaseDelay);
        theBallSpringJoint.enabled = false;
        theBall = null;
        theBallSpringJoint = null;
        disableTouchInput = false;
    }

    private IEnumerator RespawnBall()
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnBall();
    }
    private void Lunch()
    {
        theBall.isKinematic = false;
        StartCoroutine(DelayRelease());
        Destroy(theBall.gameObject, ballDespawnTime);
        StartCoroutine(RespawnBall());
        isPressed = false;
    }
}
