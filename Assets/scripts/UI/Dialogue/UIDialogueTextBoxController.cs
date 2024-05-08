using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
    [SerializeField]
    private TextMeshProUGUI m_SpeakerText;
    [SerializeField]
    private TextMeshProUGUI m_DialogueText;

    [SerializeField]
    private RectTransform m_ChoicesBoxTransform;
    [SerializeField]
    private UIDialogueChoiceController m_ChoiceControllerPrefab;


    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    public Image fadetoblack;
    public int FadeSpeed;

    private bool m_ListenToInput = true;
    private DialogueNode m_NextNode = null;

    [SerializeField]
    Animator AB_Animator;
    [SerializeField]
    AudioSource audioSource;

    public XRIDefaultInputActions customInputs;

    private InputAction menuButton;

    private void OnEnable() {
        customInputs.Enable();
        menuButton.Enable();
        menuButton.performed += Menu;

    }

    private void OnDisable() {
        
        customInputs.Disable();
        menuButton.Disable();
    }


    private void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);

        customInputs = new XRIDefaultInputActions();
        menuButton = customInputs.Custom.Menu;
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    private void Update()
    {
       
    }

    private void OnDialogueNodeStart(DialogueNode node)
    {
        gameObject.SetActive(true);

        m_DialogueText.text = node.DialogueLine.Text;
        m_SpeakerText.text = node.DialogueLine.Speaker.CharacterName;

        if (node.DialogueLine.voiceClip != null) //check to make sure line has audio before trying to play it
        {
            audioSource.PlayOneShot(node.DialogueLine.voiceClip);
        }

        node.Accept(this);
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        m_NextNode = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxTransform)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
        StartCoroutine(fadeCoroutine());
        

    }

    IEnumerator fadeCoroutine()
    {
        float timer =0;
        timer += Time.deltaTime;
        float fadeAmount = fadetoblack.color.a;
        fadeAmount += Time.deltaTime;
        if (timer >= 1)
        {
            yield return null; 
        }
        else
        {
            fadetoblack.color = new Color(fadetoblack.color.r, fadetoblack.color.g, fadetoblack.color.b, fadeAmount );
        }
        
    }

    public void Visit(BasicDialogueNode node)
    {
        m_ListenToInput = true;
        m_NextNode = node.NextNode;
    }



    public void Visit(ChoiceDialogueNode node)
    {
        m_ChoicesBoxTransform.gameObject.SetActive(true);
        m_ListenToInput = true;

        foreach (DialogueChoice choice in node.Choices)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
            newChoice.Choice = choice;
        }
    }

    private void Menu(InputAction.CallbackContext callbackContext)
    {   
        Debug.Log(m_ListenToInput);
            Debug.Log("Tab ui");
        if (m_ListenToInput)
        {
            m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
        }
    }
}