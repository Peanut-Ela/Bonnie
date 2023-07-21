using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingSequence_Script : MonoBehaviour
{
    private enum EndingState
    {
        TITLE_FADEIN,
        TITLE_SPIRITFLYIN,
        TITLE_SCROLLING,
        WINNER_FADEIN,
        FINISHED,
    }

    [SerializeField]
    private AudioSource CreditsMusic;

    public float DefaultScrollingSpeed = 10;

    private float AdvancedSpeed = 1.0f;
    private float AdvancedSpeedHolddownTime = 0.0f;
    public float HoldDownSpeedUpTier1 = 4;
    public float HoldDownSpeedUpTier2 = 16;

    private float timer = 0.0f;
    public float State1Time = 2.0f;
    public float State2Time = 3.0f;
    public float State4Time = 2.0f;

    private GameObject ProductionImage;

    private EndingState state = EndingState.TITLE_FADEIN;

    private float PortfolioTimer = 0.0f;
    public float PortfolioTime = 4.0f;
    private bool PortfolioDisplaying = false;

    public void DisplayPortfolio(Sprite img, string text)
    {
        ProductionImage.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        ProductionImage.transform.GetChild(1).GetComponent<Image>().sprite = img;
        PortfolioDisplaying = true;
        PortfolioTimer = PortfolioTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProductionImage = GameObject.Find("ProductionImage");

        state = EndingState.TITLE_FADEIN;
        timer = State1Time;

        GameObject.Find("Title").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("winner").GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0);
        GameObject.Find("MenuButton").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("MenuButton").GetComponent<Button>().enabled = false;

        ProductionImage.transform.GetChild(0).GetComponent<TMP_Text>().alpha = 0;
        ProductionImage.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            AdvancedSpeedHolddownTime += Time.deltaTime;
            if (AdvancedSpeedHolddownTime >= 2.0f)
            {
                AdvancedSpeed = HoldDownSpeedUpTier2;
            }
            else
            {
                AdvancedSpeed = HoldDownSpeedUpTier1;
            }
        }
        else
        {
            AdvancedSpeed = 1.0f;
            AdvancedSpeedHolddownTime = 0.0f;
        }

        if (PortfolioDisplaying)
        {
            PortfolioTimer = Mathf.Clamp(PortfolioTimer - Time.deltaTime * AdvancedSpeed, 0, PortfolioTime);
            if (PortfolioTimer <= Mathf.Epsilon)
            {
                PortfolioDisplaying = false;
                ProductionImage.transform.GetChild(0).GetComponent<TMP_Text>().alpha = 0;
                ProductionImage.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            else
            {
                ProductionImage.transform.GetChild(0).GetComponent<TMP_Text>().alpha = Mathf.Clamp(6 * Mathf.Sin(Mathf.PI * (1 - PortfolioTimer / PortfolioTime)), 0, 1);
                ProductionImage.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Clamp(6 * Mathf.Sin(Mathf.PI * (1 - PortfolioTimer / PortfolioTime)), 0, 1));
            }
        }

        switch (state)
        {
            case EndingState.TITLE_FADEIN:
                timer = Mathf.Clamp(timer - Time.fixedDeltaTime * AdvancedSpeed, 0, State1Time);
                GameObject.Find("Title").GetComponent<Image>().color = new Color(1, 1, 1, 1 - timer / State1Time);
                if (timer <= Mathf.Epsilon)
                {
                    GameObject.Find("Title").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    timer = State2Time;
                    state += 1;
                }
                break;
            case EndingState.TITLE_SPIRITFLYIN: //MAX X = -1066 ~ 1066   MAX Y = -296 ~ 296
                timer = Mathf.Clamp(timer - Time.deltaTime * AdvancedSpeed, 0, State2Time);
                GameObject.Find("Spirit").GetComponent<RectTransform>().anchoredPosition = new Vector2(-1066 + 2 * 1066 * (1 - timer / State2Time) + 533 * Mathf.Clamp(Mathf.Sin(Mathf.PI * 2 * (1 - timer / State2Time)), -1, 1), -296 * Mathf.Sin(Mathf.PI * 2 * 1.5f * (1 - timer / State2Time)));

                if (timer <= Mathf.Epsilon)
                {
                    if (CreditsMusic != null) CreditsMusic.Play();
                    state += 1;
                    GameObject.Find("Spirit").GetComponent<RectTransform>().anchoredPosition = new Vector2(-1066 + 2 * 1066, 0);
                }
                else if (timer + Time.deltaTime * AdvancedSpeed > 0.5f && timer <= 0.5f)
                {
                    GameObject.Find("Spirit").transform.SetAsLastSibling();
                }
                break;
            case EndingState.TITLE_SCROLLING:
                GetComponent<RectTransform>().anchoredPosition += new Vector2(0, DefaultScrollingSpeed) * Time.deltaTime * AdvancedSpeed;
                if (GetComponent<RectTransform>().anchoredPosition.y >= 3820)
                {
                    timer = State4Time;
                    GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 3820);
                    state += 1;
                }
                break;
            case EndingState.WINNER_FADEIN:
                timer = Mathf.Clamp(timer - Time.deltaTime * AdvancedSpeed, 0, State4Time);
                GameObject.Find("winner").GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1 - timer / State4Time);
                GameObject.Find("MenuButton").GetComponent<Image>().color = new Color(1, 1, 1, 1 - timer / State4Time);
                if (timer <= Mathf.Epsilon)
                {
                    GameObject.Find("winner").GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
                    GameObject.Find("MenuButton").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    GameObject.Find("MenuButton").GetComponent<Button>().enabled = true;
                    state += 1;
                }
                break;
            case EndingState.FINISHED:
                break;
            default:
                break;
        }
    }
}
