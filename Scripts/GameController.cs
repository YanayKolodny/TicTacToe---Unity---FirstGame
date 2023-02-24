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
    public int[] markedSpaces; //ID's which space was marked by whice player - receives values that helps to calculate the game's resulte
    public Text winnerText; // Hold the text component of the winner text
    public GameObject[] winningLine; // Hold all the different line to show the winning sequence
    public GameObject winnerPanel; // Hold the panel that appear at the end of the game with the result
    public int xPlayerScore; // The variable that keep count of player X's score
    public int oPlayerScore; // The variable that keep count of player O's score
    public Text xPlayerScoreText; // Hold the text object that presents player X's score
    public Text oPlayerScoreText; // Hold the text object that presents player O's score
    public Button xPlayerButton; // Before Each game start, the user can press on either the X or O sprites...          }
    public Button oPlayerButton; // and choose which player get the first turn. These are the buttons for that feture.  }
    public GameObject cImage; // Hold the image that appears in the center of the panel in case the game end's with a draw

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

    // GameSetup is a function that initiates the elements of the game when it starts 
    void GameSetup()
    {
        whoseTurn = 0;
        turnCounter = 0;
        turnIcons[0].SetActive(true);       // Activating the dot icon for player X
        turnIcons[1].SetActive(false);      // De-activating the dot icon for player O

        // Looping over the tictactoeSpaces array to initiate the grid's buttons
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = true;                     // Setting each button as interactable
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;     // Erasing the image that appears when the button is selected
        }

        // Looping over the markedSpaces array to reset it's values. the "defult" value is -100
        for (int i = 0; i < markedSpaces.Length; i++)
        {
            markedSpaces[i] = -100;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TicTacToeButton is a method that happens whenever a players select a place on the grid - 
    public void TicTacToeButton(int WhichNumber) 
    {
    
    // When a player makes a turn we de-activating the option to select who's turn it is - only needs to happen on the first turn
    if(turnCounter == 0)
    {
    xPlayerButton.interactable = false;
    oPlayerButton.interactable = false;
    }

    // We provide the image to that space on the grid according to whoseTurn it is. And De-activating that button
    tictactoeSpaces[WhichNumber].image.sprite = playIcons[whoseTurn];
    tictactoeSpaces[WhichNumber].interactable = false;


    markedSpaces[WhichNumber] = whoseTurn+1;  //Setting the marked spaces as either 1 or 2 
    turnCounter++;  // Updating the turn counter

    // Once there's a chance of a win we check if a win has happened
    if(turnCounter > 4)
    {
        bool isWinner = WinnerCheck();              // Checking if the there's a winner using the WinnerCheck function
        if(turnCounter == 9 && isWinner == false)   // If all the turnes were taken and there's no winner - it is a draw
        {
        Draw();                     // Calling the draw function
        DrawSound.Play();           // Playing the draw sound
        }
    }

    // Checking who's turn it is and switching accordingly. Also switching the turnIcons dot accordingly.
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

    // The WinnerCheck function checks if there's a winner and returnes true/false accordingly
    bool WinnerCheck()
    {
        // Assigning all the possible solutions with the calculated values from the markedSpaces array
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];
        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];
        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];

        // Creating an array of the solutions values and looping over it to check if one of the possible solutions had happened
        var solutions = new int[] {s1, s2 ,s3, s4, s5, s6, s7, s8};
        for(int i = 0; i < solutions.Length; i++)
        {
            // If the value of the solution is equal to either 3 or 6 - we have a winner. 3 would be X and 6 would be O. 
            if(solutions[i] == 3*(whoseTurn+1))
            {
                WinnerDisplay(i);           // Calling the WinnerDisplay function and providing it with index of the current iteration 
                WinningSound.Play();        // Playing the winning sound
                return true;
            }
        }
        return false;
    }

    // The WinnerDisplay function is diplaying the winning panel and winning sequence
    void WinnerDisplay(int indexIn)
    {
        winnerPanel.gameObject.SetActive(true); // Avtivating the winner panel for the wiining GUI

        // Checking who's turn it is to identify the winner
        if(whoseTurn == 0)
        {
            xPlayerScore++;                                         // Adding one point to the winner
            xPlayerScoreText.text = xPlayerScore.ToString();        // Updating the score display with the updated score
            winnerText.text = "Player X Wins!";                     // Updating the panel's text
        }
        else if(whoseTurn == 1)
        {
            oPlayerScore++;                                         // Adding one point to the winner
            oPlayerScoreText.text = oPlayerScore.ToString();        // Updating the score display with the updated score
            winnerText.text = "Player O Wins!";                     // Updating the panel's text
        }
        winningLine[indexIn].SetActive(true);           // Activating the winning sequence line according to the number we received
    }

    // The Rematch function restarts the game but keeping the score as is
    public void Rematch()
    {
        GameSetup();  // Calling the GameSetup function to set a new game

        // Looping over the winningLine array to de-activate all the winning lines game objects off the GUI
        for (int i = 0; i < winningLine.Length; i++)
        {
            winningLine[i].SetActive(false);            
        }

        winnerPanel.SetActive(false); // De-activating the winner panel off the GUI
        cImage.SetActive(false);      // De-activating the cImage off the panel

        // Re-activating the option to select which player starts the game
        xPlayerButton.interactable = true;
        oPlayerButton.interactable = true;
    }

    // The Restart function restarts the game and the score
    public void Restart()
    {
        Rematch();
        xPlayerScore = 0;
        oPlayerScore = 0;
        xPlayerScoreText.text = "0";
        oPlayerScoreText.text = "0";
    }

    // The SwitchPlayer function allows to select which player starts the game - it's being used before the first play is made
    // by clicking on the X or O. This function recieves the number of the player according to its sprites.
    public void SwitchPlayer(int whichPlayer)
    {
        // Checking if X or O was selected
        if(whichPlayer == 0)
        {
            whoseTurn = 0;                      // Setting X to play first
            turnIcons[0].SetActive(true);       // Setting the dot-icon to be turn on/off according to the player
            turnIcons[1].SetActive(false);
            player2ClickSound.Play();           // Playing the play sound of the X player
        }
        else if (whichPlayer == 1)
        {
            whoseTurn = 1;                      // Setting X to play first
            turnIcons[0].SetActive(false);      // Setting the dot-icon to be turn on/off according to the player
            turnIcons[1].SetActive(true);
            player1ClickSound.Play();           // Playing the play sound of the X player
        }
    }

    // The Draw function is called in case the game end with no winner - it updates the winnerPanel  
    void Draw()
    {
      winnerPanel.SetActive(true);          // Activating the winner panel to appear
      cImage.SetActive(true);               // Activating the cImage to appear in the panel
      winnerText.text = "Its a Draw!";      // Updating the winnerText
    }

    // Method the generate the click sound according to whoseTurn it is
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