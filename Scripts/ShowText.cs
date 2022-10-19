using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textUI;
    [SerializeField] string text;
    bool used = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        showText();

        
    }

    void showText()
    {
        if (!used)
        {
            used = true;
            StartCoroutine(showTextCoroutine());
        }
    }

    IEnumerator showTextCoroutine()
    {
        textUI.text = text;
        textUI.color = new Color(1, 1, 1, 0);

        for(float i = 0; i <= 1; i+=0.02f)
        {
            textUI.color = new Color(1, 1, 1, i);
            textUI.transform.position = new Vector3(textUI.transform.position.x - 0.1f, textUI.transform.position.y);
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 1; i >= 0; i -= 0.02f)
        {
            textUI.color = new Color(1, 1, 1, i);
            textUI.transform.position = new Vector3(textUI.transform.position.x - 0.1f, textUI.transform.position.y);
            yield return new WaitForSeconds(0.01f);
        }
        textUI.gameObject.SetActive(false);
    }
}
