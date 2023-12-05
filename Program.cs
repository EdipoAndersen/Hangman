using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        HangmanGame hangmanGame = new HangmanGame();
        hangmanGame.OnGameStart += () => Console.WriteLine("START GAME");
        hangmanGame.OnGameOver += () => Console.WriteLine("GAME OVER");
        hangmanGame.OnGameWin += () => Console.WriteLine("THE WORD WAS CORRECT!");
        hangmanGame.OnGameRestart += () => Console.WriteLine("STARTING NEW GAME...");
        hangmanGame.OnGameRestart += () => Thread.Sleep(new TimeSpan(0, 0, 2));
        hangmanGame.OnGameRestart += () => Console.Clear();

        hangmanGame.Start();
    }
}