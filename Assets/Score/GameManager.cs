
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    #region INITIALISATION:
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public ScoreManager scoreManager;
    

    private string[] pizzaToppings = { "Pepperoni", "Mushrooms", "Onions", "Sausage", "Bacon", "Extra Cheese", "Black Olives", "Green Peppers" };
    public int[] arrToppingsIndex = new int[8];
    private string currentOrder;
    private float timer = 30f;
    private bool bonusAchieved = false;
    public int numToppings;
   
    void Start()
    {
        GenerateOrder();
        UpdateOrderText();
        UpdateScoreText();
        UpdateTimerText();
    }
    #endregion
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (!bonusAchieved && timer <= 0)
            {
                scoreManager.AddScore(5); // Bonus for completing order within 30 seconds
                bonusAchieved = true;
                UpdateScoreText();
            }
        }
        else
        {
            // Time's up, generate a new order and reset the timer
            GenerateOrder();
            timer = 30f;
            bonusAchieved = false;
        }
    }
    #region ORDERS:
    void GenerateOrder()
    {
        currentOrder = "Order: ";
        numToppings = Random.Range(1, 4); // Random number of toppings between 1 and 3

        for (int i = 0; i < numToppings; i++)
        {
            int toppingIndex = Random.Range(0, pizzaToppings.Length);
            arrToppingsIndex[i] = toppingIndex;
            currentOrder += pizzaToppings[toppingIndex] + ", ";
        }

        currentOrder = currentOrder.TrimEnd(' ', ',');
        UpdateOrderText();
    }

    public void CheckOrder(string[] playerToppings,int arrayLength)
    {
        string[] orderToppings = currentOrder.Substring(7).Split(',');
        bool orderCompleted = true;

        if (orderToppings.Length != arrayLength)
            orderCompleted = false;
        else
        {
            foreach (string topping in orderToppings)
            {
                bool toppingFound = false;
               foreach (string playerTopping in playerToppings)
                {
                    if (topping.Trim() == playerTopping.Trim())
                    {
                        toppingFound = true;
                        break;
                   }
                }
                if (!toppingFound)
                {
                    orderCompleted = false;
                    break;
                }
            }
        }

        if (orderCompleted)
        {
            scoreManager.AddScore(10); // Increase score for completing order
            GenerateOrder(); // Generate new order
            timer = 30f; // Reset timer
            bonusAchieved = false;
            orderCompleted = false; //Prevents score from being added without the order being complete
        }
    }
    #endregion

    #region UPDATE TEXT:
    void UpdateOrderText()
    {
        orderText.text = currentOrder;
    }

    void UpdateScoreText()
    {
        scoreText.text = ("Score: " + scoreManager.score.ToString());
    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.Round(timer).ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
#endregion

//values for bonus points + time frame for bonus points + point deduction for colliding with enemies TO CHANGE*

