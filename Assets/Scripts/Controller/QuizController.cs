using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{

    public List<QuizModle> Quiz;
    [Space(10)]
    public GameObject levelPanal;
    public GameObject Question;
    public GameObject[] wordOptions;
    public GameObject wordHolder;
    public GameObject optionHolder;
    public GameObject fadeInOut;
    public GameObject Menu;
    public GameObject nextButton;
    public GameObject hintPanal;
    public GameObject scoreBox;
    public GameObject hintButton;
    public Image hintImage;
    public Text hintText;
    public Text gameScore;
    public Text finalScore;
    [Space(10)]
    public List<int> randomIndex = new List<int>();
    public List<int> randomOptionIndex = new List<int>();

    public static int incrementScore = 10;

    private int currentQuestion = 0;
    public static int currentLevel = 0;

    public Vector2[] firstLastQuestion;

    public static bool[] isLevel = { false, false, false };
    public int length;
    private int randomNumber = 0;
    private int maxQuestions = 3;
    private int initialIndex;
    private int finalIndex;
    private int holderIndex;
    public string correctAnswer;
    public string Answer;
    private int score;
    private float miliCount = 0;
    private int seconds = 20;
    private int tapsOnHint;

    void Start()
    {
        
        gameScore.text = "0"; 
        RandomIndexGenerator();
        LoadLevel();
    }

    void Update()
    {
        //Caluculating the time 
        miliCount += Time.deltaTime * 10;
        if (seconds == 0)
        {
            seconds = 20;
            //Activating HintButton
            hintButton.SetActive(true);
            
        }
        else if (miliCount >= 10)
        {
            seconds -= 1;
            miliCount = 0;
        }
        //Removing Next Level Butoon When it is last level
        if (currentLevel == 3)
        {
            nextButton.SetActive(false);
        }
    }

    //Hint button functionatality
    public void HintBoxButton()
    {
        tapsOnHint += 1;
        StartCoroutine(WrongAnswer());
    }

    //loads the levels and rounds
    public void LoadLevel()
    {
        Answer = null;
        holderIndex = 0;
        levelPanal.SetActive(false);
        for (int i = 0; i < wordOptions.Length; i++)
        {
            wordOptions[i].SetActive(false);
        }
        for (int i = 0; i < length; i++)
        {
            wordOptions[i].transform.SetParent(optionHolder.transform);
            wordOptions[i].GetComponent<Button>().interactable = true;
        }
        length = Quiz[randomIndex[currentQuestion] - 1].Alphabets.Length;
        levelPanal.SetActive(true);
        Question.GetComponent<Image>().sprite = Quiz[randomIndex[currentQuestion] - 1].questionObject;
        RandomOptionIndexGenerator(1, length + 1);
        correctAnswer = Quiz[randomIndex[currentQuestion] - 1].questionObject.name.ToUpper();
        for (int i = 0; i < length; i++)
        {
            wordOptions[i].GetComponent<Image>().sprite = Quiz[randomIndex[currentQuestion] - 1].Alphabets[randomOptionIndex[i] - 1];
            wordOptions[i].SetActive(true);
        }
    }

    //functionality of Level complete menu panal
    IEnumerator LevelFinsh()
    {
        int tempScore = 0;
        tempScore = PlayerPrefs.GetInt("tempScore") + score;
        scoreBox.SetActive(false);
        PlayerPrefs.SetInt("tempScore", tempScore);
        finalScore.text = "" + tempScore;
        isLevel[currentLevel] = false;
        currentLevel += 1;
        fadeInOut.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        Menu.SetActive(true);
        fadeInOut.SetActive(false);
        levelPanal.SetActive(true);
    }

    //checking wethere the arrangment of the word is correct or not
    public void TapResult(string ObName)
    {
        seconds = 20;
        for (int i = 0; i < wordOptions.Length; i++)
        {
            if (ObName == wordOptions[i].name)
            {
                wordOptions[i].transform.position = wordHolder.transform.position;
                wordOptions[i].transform.SetParent(wordHolder.transform);
                wordOptions[i].GetComponent<Button>().interactable = false;
                holderIndex += 1;
                if (length == holderIndex)
                {
                    for (int j = 0; j < length; j++)
                    {
                        Answer = Answer + wordHolder.transform.GetChild(j).GetComponent<Image>().sprite.name.ToUpper();
                    }
                    if (correctAnswer == Answer)
                    {
                        hintButton.SetActive(false);
                        score += incrementScore;
                        gameScore.text = "" + score;
                        currentQuestion += 1;
                        if (currentQuestion != 3)
                        {
                            LoadLevel();
                        }
                        else
                        {
                            
                            StartCoroutine(LevelFinsh());
                        }
                    }
                    else
                    {
                        StartCoroutine(WrongAnswer());
                    }
                }
            }
        }
    }

    //hint display
    IEnumerator WrongAnswer()
    {
        hintImage.sprite = Quiz[randomIndex[currentQuestion] - 1].questionObject;
        hintText.text = correctAnswer;
        hintPanal.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        hintPanal.SetActive(false);
        LoadLevel();
    }

    //Random question index
    private void RandomIndexGenerator()
    {
        Debug.Log("0");
        Debug.Log(isLevel[currentLevel] + "&&" + currentQuestion);
        if (isLevel[currentLevel] && currentQuestion == 0)
        {
            Debug.Log("1");
            initialIndex = (int)firstLastQuestion[currentLevel].x;
            finalIndex = (int)firstLastQuestion[currentLevel].y;
            RandomIndexListGenerator(initialIndex, finalIndex);
        }
    }

    //random option index
    public void RandomOptionIndexGenerator(int initial, int last)
    {
        randomOptionIndex = new List<int>(new int[length]);
        for (int i = 0; i < length; i++)
        {
            randomNumber = Random.Range(initial, last);
            while (randomOptionIndex.Contains(randomNumber))
            {
                randomNumber = Random.Range(initial, last);
            }
            randomOptionIndex[i] = randomNumber;
        }
    }

    //random no. genrator
    private void RandomIndexListGenerator(int initialIndex, int lastIndex)
    {
        randomIndex = new List<int>(new int[maxQuestions]);
        for (int i = 0; i < maxQuestions; i++)
        {
            randomNumber = Random.Range(initialIndex, lastIndex);
            while (randomIndex.Contains(randomNumber))
            {
                randomNumber = Random.Range(initialIndex, lastIndex);
            }
            randomIndex[i] = randomNumber;
        }
    }

}
