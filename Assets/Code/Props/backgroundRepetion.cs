using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundRepetion : MonoBehaviour
{
    public float ascensionSpeed;
    private Vector2 originalPosition;
    public bool canStartScrolling;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.gameObject.transform.position;
        StartCoroutine(deleteFirstPart());
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canStartScrolling)
        {
            timer += Time.deltaTime;
            float nPos = Mathf.Repeat(timer * ascensionSpeed, 10);
            transform.position = originalPosition - Vector2.up * nPos;
        }
    }

    private IEnumerator deleteFirstPart()
    {
        while (!canStartScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.5f);
        transform.Find("toDelete").gameObject.SetActive(false);
        transform.Find("toActivate").gameObject.SetActive(true);
        yield return null;
    }
}
