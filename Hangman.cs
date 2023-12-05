class HangmanGame
{
    private string[]? words;
    private string selectedWord = String.Empty;
    private List<char>? correctGuesses;
    private List<char>? incorrectGuesses;
    private int attemptsLeft;
    private int maxAttempts;
    private char heartSymbol = '♥';
    int lives;

    public delegate void eventHandler();
    public event eventHandler? OnGameStart;
    public event eventHandler? OnGameOver;
    public event eventHandler? OnGameWin;
    public event eventHandler? OnGameRestart;

    public HangmanGame()
    {
        LoadWordsFromFile("wordlist.txt");
    }

    public void defineLives()
    {
        do
        {
            Console.Write("Enter the number of lives for the game: ");
        } while (!int.TryParse(Console.ReadLine(), out lives) || lives <= 0);
        maxAttempts = lives;
    }

    public void Start()
    {
        OnGameStart?.Invoke();
        defineLives();
        Play();
    }

    public void Restart()
    {
        correctGuesses?.Clear();
        incorrectGuesses?.Clear();
        attemptsLeft = maxAttempts;
        OnGameRestart?.Invoke();
        Start();
    }

    private void LoadWordsFromFile(string fileName)
    {
        try
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FilesFolder", fileName);
            words = File.ReadAllText(filePath).Split('\n');
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Wordlist file not found. Please create a {fileName} file with one word per line.");
            Environment.Exit(1);
        }
    }

    private int CountUniqueLetters(string word)
    {
        return word.Distinct().Count(char.IsLetter);
    }

    private void Play()
    {
        Random random = new Random();
        selectedWord = words[random.Next(0, words.Length)].ToUpper();
        correctGuesses = new List<char>();
        incorrectGuesses = new List<char>();
        attemptsLeft = maxAttempts;

        Console.WriteLine("Let`s play a hangman game!");
        Console.WriteLine($"Hint: It has {CountUniqueLetters(selectedWord)} distinct letters.");

        while (true)
        {
            DisplayHangman();
            DisplayWord();

            Console.Write("\nEnter a letter: ");
            char guess = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (char.IsLetter(guess))
            {
                if (selectedWord.Contains(guess) && !correctGuesses.Contains(guess))
                {
                    correctGuesses.Add(guess);

                    // Check if all unique letters have been guessed
                    if (correctGuesses.Count == CountUniqueLetters(selectedWord))
                    {
                        OnGameWin?.Invoke();
                        break;
                    }
                }
                else if (!correctGuesses.Contains(guess) && !selectedWord.Contains(guess))
                {
                    incorrectGuesses.Add(guess);

                    // Move the attemptsLeft decrement outside the if block
                    attemptsLeft--;

                    if (attemptsLeft == 0)
                    {
                        OnGameOver?.Invoke();
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid letter.");
            }
        }

        Console.WriteLine($"The word was: {selectedWord}");
        Console.Write("Press Enter to play again...");
        Console.ReadLine();
        Restart();
    }

    private void DisplayWord()
    {
        Console.Write(" Word: ");

        foreach (char letter in selectedWord)
        {
            if (char.IsLetter(letter) && correctGuesses.Contains(letter))
            {
                Console.Write(letter + " ");
            }
            else if (char.IsLetter(letter))
            {
                Console.Write("_ ");
            }
            else
            {
                Console.Write(letter + " "); // Display spaces or special characters as is
            }
        }

        Console.WriteLine($"\nAttempts left: {attemptsLeft}");
        Console.WriteLine("Incorrect guesses: " + string.Join(", ", incorrectGuesses));
    }

    private void DisplayHangman()
    {
        Console.WriteLine($"\nLives: {new string(heartSymbol, attemptsLeft)}");
    }
}