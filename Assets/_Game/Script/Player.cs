using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float brickHeight;
    [SerializeField] private float baseY = -7f;

    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask brickLayer;
    [SerializeField] LayerMask unbrickLayer;
    [SerializeField] GameObject brickPrefab;

    private bool isMoving = false;
    private Vector3 currentPos;
    private Vector3 targetPos;

    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        OnDrag();
    }

    public void OnInit()
    {

    }

    private void OnDrag()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentPos = Input.mousePosition;
                //Debug.Log("Current: " + currentPos.x + ":" + currentPos.y + ":" + currentPos.z);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                targetPos = Input.mousePosition;
                //Debug.Log("Target: " + targetPos.x + ":" + targetPos.y + ":" + targetPos.z);
                Vector3 moveDir = targetPos - currentPos;
                moveDir = GetDirection(moveDir);
                //Debug.Log("Direction: " + moveDir.x + ":" + moveDir.y + ":" + moveDir.z);
                if (moveDir != Vector3.zero)
                {
                    StopAllCoroutines();
                    StartCoroutine(Move(moveDir));
                    StartCoroutine(CheckWall(moveDir));
                    StartCoroutine(CheckBrick());
                    StartCoroutine(CheckUnbrick());
                }
            }
        }
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        float horizontal = Mathf.RoundToInt(direction.x);
        float vertical = Mathf.RoundToInt(direction.y);

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            return new Vector3(horizontal, 0f, 0f).normalized;
        }
        else if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            return new Vector3(0f, 0f, vertical).normalized;
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }
    }

    private IEnumerator Move(Vector3 direction)
    {
        while (true)
        {
            isMoving = true;
            Vector3 movement = new Vector3(direction.x, 0f, direction.z);
            transform.position += movement * speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CheckWall(Vector3 direction)
    {
        while (true)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.position;
            raycastPos.y = baseY;
            if (Physics.Raycast(raycastPos, direction, out hit, 0.5f, wallLayer))
            {
                StopAllCoroutines();
                isMoving = false;
            }
            yield return null;
        }
    }

    private IEnumerator CheckBrick()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f, brickLayer))
            {
                //Debug.Log("+1");
                Destroy(hit.collider.gameObject);

                Vector3 playerPos = transform.position;
                playerPos.y += brickHeight;
                transform.position = playerPos;

                Vector3 spawnPos = new Vector3(transform.position.x, baseY - 2 * brickHeight, transform.position.z);
                GameObject newBrick = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                newBrick.transform.SetParent(transform);
            }
            yield return null;
        }
    }

    private IEnumerator CheckUnbrick()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f, unbrickLayer))
            {
                Transform bottomBrick = GetBottomBrick();
                if (bottomBrick != null)
                {
                    Destroy(bottomBrick.gameObject);

                    //Debug.Log("-1");
                    Instantiate(brickPrefab, hit.collider.gameObject.transform.position, Quaternion.identity);
                    Destroy(hit.collider.gameObject);

                    Vector3 playerPos = transform.position;
                    playerPos.y -= brickHeight;
                    transform.position = playerPos;
                }
                else
                {
                    Debug.Log("Game over");
                    StopAllCoroutines();
                }
            }
            yield return null;
        }
    }

    private Transform GetBottomBrick()
    {
        Transform bottomBrick = null;

        Transform[] bricks = transform.GetComponentsInChildren<Transform>();
        foreach (Transform brick in bricks)
        {
            if (brick.CompareTag("LoadBrick"))
            {
                if (bottomBrick == null || brick.position.y < bottomBrick.position.y)
                {
                    bottomBrick = brick;
                }
            }
        }
        return bottomBrick;
    }
}
