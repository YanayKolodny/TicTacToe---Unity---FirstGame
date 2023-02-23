using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int whoseTurn; // 0 = X and 1 = O
    public int turnCounter; // counts the number of turns played
    public GameObject[] turnIcons; // displays who's turn it is
    public Sprite[] playIcons; // 0 = x icon and 1 = O icon
    public Button[] tictactoeSpaces; // playable space for our game
    public int[] markedSpaces; //ID's which space was marked by whice player
    public Text winnerText; // Hold the text component of the winner text
    public GameObject[] winningLine; // Hold all the different line to show the winning sequence
    public GameObject winnerPanel;
    public int xPlayerScore;
    public int oPlayerScore;
    public Text xPlayerScoreText;
    public Text oPlayerScoreText;
    public Button xPlayerButton;
    public Button oPlayerButton;
    public GameObject cImage;

    // Game Sounds
    public AudioSource player1ClickSound;
    public AudioSource player2ClickSound;
    public AudioSource WinningSound;
    public AudioSource DrawSound;

    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
    }

    void GameSetup()
    {
        whoseTurn = 0;
        turnCounter = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);

        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }

        for (int i = 0; i < markedSpaces.Length; i++)
        {
            markedSpaces[i] = -100;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

public void TicTacToeButton(int WhichNumber) 
{
    xPlayerButton.interactable = false;
    oPlayerButton.interactable = false;

    tictactoeSpaces[WhichNumber].image.sprite = playIcons[whoseTurn];
    tictactoeSpaces[WhichNumber].interactable = false;

    markedSpaces[WhichNumber] = whoseTurn+1;  //Setting the marked spaces as either 1 or 2 
    turnCounter++;
    if(turnCounter > 4)
    {
        bool isWinner = WinnerCheck();
        if(turnCounter == 9 && isWinner == false)
        {
        Draw();
        DrawSound.Play();
        }
    }

    if(whoseTurn == 0)
    {
        whoseTurn = 1;
        turnIcons[1].SetActive(true);
        turnIcons[0].SetActive(false);
    }
    else
    {
        whoseTurn = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);
    }
}

    bool WinnerCheck()
    {
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];
        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];
        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];
        var solutions = new int[] {s1, s2 ,s3, s4, s5, s6, s7, s8};
        for(int i = 0; i < solutions.Length; i++)
        {
            if(solutions[i] == 3*(whoseTurn+1))
            {
                WinnerDisplay(i);
                WinningSound.Play();
                return true;
            }
        }
        return false;
    }

    void WinnerDisplay(int indexIn)
    {
        winnerPanel.gameObject.SetActive(true);
        if(whoseTurn == 0)
        {
            xPlayerScore++;
            xPlayerScoreText.text = xPlayerScore.ToString();
            winnerText.text = "Player X Wins!";
        }
        else if(whoseTurn == 1)
        {
            oPlayerScore++;
            oPlayerScoreText.text = oPlayerScore.ToString();
            winnerText.text = "Player O Wins!";
        }
        winningLine[indexIn].SetActive(true);
    }

    public void Rematch()
    {
        GameSetup();
        for (int i = 0; i < winningLine.Length; i++)
        {
            winningLine[i].SetActive(false);            
        }
        winnerPanel.SetActive(false);

        xPlayerButton.interactable = true;
        oPlayerButton.interactable = true;

        cImage.SetActive(false);
    }

    public void Restart()
    {
        Rematch();
        xPlayerScore = 0;
        oPlayerScore = 0;
        xPlayerScoreText.text = "0";
        oPlayerScoreText.text = "0";
    }

    public void SwitchPlayer(int whichPlayer)
    {
        if(whichPlayer == 0)
        {
            whoseTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
        else if (whichPlayer == 1)
        {
            whoseTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
    }

    void Draw()
    {
      winnerPanel.SetActive(true);
      cImage.SetActive(true);
      winnerText.text = "Its a Draw!";  
    }

    public void PlayButtonClick()
    {
        if(whoseTurn == 0)
        {
        player1ClickSound.Play();
        }
        else if(whoseTurn == 1)
        {
        player2ClickSound.Play();
        }
    }
}