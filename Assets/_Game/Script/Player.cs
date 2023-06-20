using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask brickLayer;

    private bool isMoving = false;
    private Vector2 currentPos;
    private Vector2 targetPos;

    private void Update()
    {
        OnDrag();
    }

    private void OnDrag()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                targetPos = Input.mousePosition;
                Vector2 moveDir = targetPos - currentPos;
                moveDir = GetDirection(moveDir);
                StopAllCoroutines();
                StartCoroutine(Move(moveDir));
                StartCoroutine(CheckWall(moveDir));
                StartCoroutine(CheckBrick());
            }
        }
    }

    private Vector2 GetDirection(Vector2 direction)
    {
        float horizontal = Mathf.RoundToInt(direction.x);
        float vertical = Mathf.RoundToInt(direction.y);

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            return new Vector2(horizontal, 0f).normalized;
        }
        else if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            return new Vector2(0f, vertical).normalized;
        }
        else
        {
            return new Vector2(0f, 0f);
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        while (true)
        {
            isMoving = true;
            Vector3 movement = new Vector3(direction.x, direction.y, 0f);
            transform.position += movement * speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CheckWall(Vector2 direction)
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 0.5f, wallLayer))
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
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 5f, brickLayer))
            {
                Debug.Log("+1");
                Destroy(hit.collider.gameObject);
            }
            yield return null;
        }
    }
}
