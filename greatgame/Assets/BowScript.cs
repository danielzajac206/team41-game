using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class BowScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject arrow;
    Animator animator;
    Vector3 mousePos;
    bool isDrawing = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        arrow.GetComponent<SpriteRenderer>().enabled = false;
        player = transform.parent;
    }
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z - player.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = (mousePos - player.position).normalized;
        if (Input.GetMouseButton(1))
        {
            arrow.GetComponent<BoxCollider2D>().enabled = false;

            GetComponent<SpriteRenderer>().enabled = true;
            arrow.GetComponent<SpriteRenderer>().enabled = true;
            animator.SetBool("drawing", true);
            arrow.GetComponent<Animator>().SetBool("drawing", true);

            dir.z = -0.02f;
            transform.position = player.position + dir * 0.8f;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (dir.y > 0)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                arrow.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                arrow.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            player.gameObject.GetComponent<PlayerController>().isLocked = true;
        } else
        {
            //GetComponent<SpriteRenderer>().enabled = false;
            //arrow.GetComponent<SpriteRenderer>().enabled = false;
            //animator.SetTrigger("Reset");
            //arrow.GetComponent<Animator>().SetTrigger("Reset");
            player.gameObject.GetComponent<PlayerController>().isLocked = false;
        }
        if (isDrawing)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(1))
            {
                arrow.GetComponent<BoxCollider2D>().enabled = true;
                animator.Play("bow_shoot");
                arrow.transform.parent = null;
                Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
                arrowRB.AddForce(arrow.transform.right * 20, ForceMode2D.Impulse);
                Debug.Log("shot");
                arrow.GetComponent<Animator>().enabled = false;
                StartCoroutine(DeleteBow());
            }
        }
    }

    public void DrawBow(Vector3 dir)
    {
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        arrow.GetComponent<SpriteRenderer>().sortingOrder = 2;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                animator.Play("bow_draw");
                arrow.GetComponent<Animator>().Play("arrow_draw");
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                arrow.GetComponent<SpriteRenderer>().flipX = true;
                animator.Play("bow_draw");
                arrow.GetComponent<Animator>().Play("arrow_draw_left");
            }
        }
        else
        {
            if (dir.y > 0)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                arrow.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            transform.rotation = Quaternion.Euler(0, 0, dir.y > 0 ? 90 : -90);
            animator.Play("bow_draw");
            arrow.GetComponent<Animator>().Play("arrow_draw");
        }
    }

    IEnumerator DeleteBow()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    void SetIsDrawingFalse()
    {
        isDrawing = false;
        Debug.Log("not isdrawing");
    }

    void SetIsDrawingTrue()
    {
        isDrawing = true;
        Debug.Log("not isdrawing");
    }
}
