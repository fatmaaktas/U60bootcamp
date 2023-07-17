using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text dialogText;
    public string[] dialogLines; 
    public float textSpeed = 0.05f; 

    private int currentLine = 0; 
    private bool isPrinting = false; 

    private bool inTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            dialogPanel.SetActive(true);
            StartDialog();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            dialogPanel.SetActive(false);
            currentLine = 0; 
        }
    }

    private void Update()
    {
        if (inTrigger && Input.GetMouseButtonDown(0))
        {
            if (isPrinting)
            {
                
                textSpeed = 0.01f;
            }
            else if (currentLine < dialogLines.Length - 1)
            {
                
                currentLine++;
                StartDialog();
            }
            else
            {
                
                dialogPanel.SetActive(false);
            }
        }
    }

    private void StartDialog()
    {
        dialogText.text = "";
        StartCoroutine(PrintDialog(dialogLines[currentLine]));
    }

    private IEnumerator PrintDialog(string dialog)
    {
        isPrinting = true;
        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isPrinting = false;
    }
}
