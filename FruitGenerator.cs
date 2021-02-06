using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitGenerator : MonoBehaviour
{
    public GameObject[] fruits;
    public Camera mainCamera;
    public float pivotHeight = 3;
    public static int fruitNum = 0;
    public static bool isGameOver = false;
    private GameObject geneFruit;
    public bool isGene;
    public bool isFall;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Init()
    {
        fruitNum = 0;
        isGameOver = false;
        Fruit.isMoves.Clear();
        StartCoroutine(StateReset());
    }

    void Update (){
        if(isGameOver)
        {
            return;
        }

        if(CheckMove(Fruit.isMoves))
        {
            return;
        }

        if(!isGene)
        {
            StartCoroutine(GenerateFruit());
            isGene = true;
            return;
        }

        Vector2 v = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x,pivotHeight);

        if(Input.GetMouseButtonUp(0))
        {
            if(!RotateButton.onButtonDown)
            {
                geneFruit.transform.position = v;
                geneFruit.GetComponent<Rigidbody2D>().isKinematic = false;
                fruitNum++;
                isFall = true;
            }
            RotateButton.onButtonDown = false;
        }
        else if(Input.GetMouseButton(0))
        {
            geneFruit.transform.position = v;
        }
        }

    IEnumerator　StateReset()
    {
        while(!isGameOver)
        {
            yield return new WaitUntil(() => isFall);
            yield return new WaitForSeconds(0.1f);
            isFall = false;
            isGene = false;
        }
    }

    IEnumerator GenerateFruit()
    {
        while(CameraController.isCollision)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.transform.Translate(0,0.1f,0);
            pivotHeight += 0.1f;
        }
        geneFruit = Instantiate(fruits[Random.Range(0,fruits.Length)],new Vector2(0,pivotHeight),Quaternion.identity);
        geneFruit.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void RotateFruit()
    {
        if(!isFall)
            geneFruit.transform.Rotate(0,0,-30);
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

     bool CheckMove(List<Moving> isMoves)
    {
        if (isMoves == null)
        {
            return false;
        }
        foreach (Moving b in isMoves)
        {
            if (b.isMove)
            {
                //Debug.Log;
                return true;
            }
        }
        return false;
    }
}
