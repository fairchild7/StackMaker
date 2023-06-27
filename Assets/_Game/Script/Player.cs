using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float brickHeight;

    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask brickLayer;
    [SerializeField] LayerMask unbrickLayer;
    [SerializeField] LayerMask endPoint;
    [SerializeField] GameObject brickPrefab;

    private bool isMoving = false;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private Vector3 moveDir;

    private int point = 0;

    private void Start()
    {
    
    }

    private void Update()
    {
        OnDrag();
    }

    public void OnInit()
    {
        isMoving = false;
        ClearBottomBrick();
    }

    private void ClearBottomBrick()
    {
        int childCount = gameObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = gameObject.transform.GetChild(i);
            Destroy(child.gameObject);
        }
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
                moveDir = targetPos - currentPos;
                moveDir = GetDirection(moveDir);
                Debug.Log("Direction: " + moveDir.x + ":" + moveDir.y + ":" + moveDir.z);
                if (moveDir != Vector3.zero)
                {
                    Move(moveDir);
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

    private void Move(Vector3 moveDir)
    {
        StopAllCoroutines();
        StartCoroutine(MoveWithDirection(moveDir));
        StartCoroutine(CheckWall(moveDir));
        StartCoroutine(CheckBrick());
        StartCoroutine(CheckUnbrick());
        StartCoroutine(CheckEndPoint());
    }

    private IEnumerator MoveWithDirection(Vector3 direction)
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
            raycastPos.y = 0f;
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
            Vector3 raycastPos = transform.position;
            raycastPos.y += 0.5f;
            RaycastHit hit;
            if (Physics.Raycast(raycastPos, Vector3.down, out hit, 50f, brickLayer))
            {
                point += 10;
                UIManager.Instance.SetPoint(point);

                //Debug.Log("+1");
                Destroy(hit.collider.gameObject);

                Vector3 playerPos = transform.position;
                playerPos.y += brickHeight;
                transform.position = playerPos;

                Vector3 spawnPos = new Vector3(transform.position.x, 0f, transform.position.z);
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
            Vector3 raycastPos = transform.position;
            raycastPos.y += 0.5f;
            if (Physics.Raycast(raycastPos, Vector3.down, out hit, 50f, unbrickLayer))
            {
                StepOnUnbrick(hit.collider.gameObject);
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

    private void SwitchTurn(GameObject obj)
    {
        switch (obj.transform.localEulerAngles.y)
        {
            case 0:
                moveDir = new Vector3(moveDir.z, moveDir.y, moveDir.x);
                return;
            case 90:
                moveDir = new Vector3(-moveDir.z, moveDir.y, -moveDir.x);
                return;
            case 180:
                moveDir = new Vector3(moveDir.z, moveDir.y, moveDir.x); 
                return;
            case 270:
                moveDir = new Vector3(-moveDir.z, moveDir.y, -moveDir.x);
                return;
            default:
                return;
        }
    }

    private void StepOnUnbrick(GameObject unbrickObj)
    {
        Transform bottomBrick = GetBottomBrick();

        if (unbrickObj.CompareTag("TurnPoint"))
        {
            SwitchTurn(unbrickObj);
            Move(moveDir);
        }

        if (unbrickObj.GetComponent<Unbrick>().active == true)
        {
            if (bottomBrick != null)
            {
                unbrickObj.GetComponent<Unbrick>().active = false;
                Transform[] unbricks = unbrickObj.GetComponentsInChildren<Transform>();
                foreach (Transform unbrick in unbricks)
                {
                    unbrick.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                Destroy(bottomBrick.gameObject);

                //Debug.Log("-1");
                Instantiate(brickPrefab, unbrickObj.transform.position, Quaternion.identity, LevelManager.Instance.map.transform).transform.Rotate(Vector3.up, 180f);

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
    }

    private IEnumerator CheckEndPoint()
    {
        while (true)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.position;
            raycastPos.y += 0.5f;
            if (Physics.Raycast(raycastPos, Vector3.down, out hit, 50f, endPoint))
            {
                StopAllCoroutines();
                LevelManager.Instance.isCompleted = true;
                Debug.Log("Congrats!");
            }
            yield return null;
        }
    }
}
