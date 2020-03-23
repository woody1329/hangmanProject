using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hman
{ 

/// <summary>
/// Class containing Main method.
/// </summary>
public static class MainClass
	{
		static void Main()
		{
			bool playGame = true;
			
			while (playGame)
			{	
				Hangman currentGame = new Hangman();
				currentGame.RunGame();
				Console.WriteLine("\n Would you like to quit? Press 'n' to quit, or any other key to continue.");
				if (Console.ReadKey().KeyChar == ('n')) playGame = false;
				Console.Write("\n");
			}
				
		}
	}

/// <summary>
/// Hangman class, containing methods and fields for starting a hangman game.
/// </summary>
	public class Hangman
	{

		/// <summary>
		/// word field contains the randomly selected word from the wordsList.
		/// </summary>
		public string word;
		/// <summary>
		/// wordsList field contains a List of strings representing possible words for the hangman game.
		/// </summary>
		public List<string> wordsList = new List<string>();
		/// <summary>
		///<c>minWordLength:</c> field for excluding words from the wordsList below a certain length.
		/// </summary>
		public int minWordLength;
		/// <summary>
		/// <c>maxWordLength:</c> field for excluding words from the wordsList above a certain length.
		/// </summary>
		public int maxWordLength;

		public int guessesLeft;

		public char currentLetter;
		public string updatedWord;
		public string LettersLeftInAlphabet = "abcdefghijklmnopqrstuvwxyz";
		public List<char> missedLetters = new List<char>();
		public string hungWord;

		public Hangman(int minWLength = 3, int maxWLength = 12, int guesses = 6, string wordFilePath = @"c:\Users\Homeworking\Desktop\words.txt")
		{
			minWordLength = minWLength;
			maxWordLength = maxWLength;
			guessesLeft = guesses;
			
			WordListFromFile(wordFilePath);
			PopulateWord();
			hungWord = CreateRepeatedString("_ ", word.Length);

		}
		/// <summary>
		/// LoadWordsFromFile 
		/// Supply path of file containing lower case strtings, one per line
		/// </summary>
		/// <param name="filePath"></param>
		public void WordListFromFile(string filePath)
		{
			IEnumerable wordsIterator = System.IO.File.ReadLines(filePath);
			foreach (string w in wordsIterator)
			{
				if (w.Length >= minWordLength && w.Length <= maxWordLength)
				{ wordsList.Add(w); }
			}

		}
		/// <summary>
		/// loops waiting for the enter key and then returns the preceeding char
		/// </summary>
		/// <returns></returns>
		public char SubmittedGuess()
		{
			char enteredLetter =' ';

			while(Console.KeyAvailable==false)
			{
				enteredLetter = Console.ReadKey().KeyChar;
				if (validLetterGuess(enteredLetter))
				{
					Console.WriteLine();
					break;
				}				
			}
			
			return enteredLetter;
		}

		/// <summary>
		/// returns True of char v is in string word
		/// </summary>
		/// <param name="v"></param>
		/// <param name="word"></param>
		/// <returns></returns>
		public bool LetterInWord(char v, string word)
		{
			return word.Contains(v);
		}
		/// <summary>
		/// Supplies a pseudorandom word from the worsList
		/// </summary>
		/// <returns>string word</returns>
		public string PopulateWord()
		{
			System.Random randomGenerator = new System.Random();
			word = wordsList[(int)(randomGenerator.NextDouble() * wordsList.Count)];
			updatedWord = word;
			return word;
		}

		/// <summary>
		/// Returns true if the supplied char guessedLetter is 
		/// </summary>
		/// <param name="guessedLetter"></param>
		/// <returns></returns>
		public bool validLetterGuess(char guessedLetter)
		{
			char[] validLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
			foreach (char c in validLetters)
			{
				if (guessedLetter == c) return true;

			}
			Console.WriteLine("\r");
			return false;

		}
		/// <summary>
		/// As correct letters are gussed, a string of the remaining letters to be guessed is updated.
		/// You must check the original word contains the letter, before updating the remaining letters.
		/// </summary>
		/// <param name="v"></param>
		public void updateLettersLeft(char v)
		{
			if (LettersLeftInAlphabet.Contains(v))
				LettersLeftInAlphabet = LettersLeftInAlphabet.Remove(LettersLeftInAlphabet.IndexOf(v), 1);
			
		}
/// <summary>
/// updates the remaining letters to be guessed, returning the index that corresponds to the supplied chars position
/// in the <b>original word</b>
/// </summary>
/// <param name="w"></param>
/// <param name="v"></param>
/// <returns></returns>
		public List<int> UpdateWord(string w, char v)
		{

			//so we find the char in the shortened string.
			//halt -> hlt 0,0,1
			//halt -> hal 0,0,1 -> al 1,0,2
			List<int> letterIndexes = new List<int>();
			int charCounter = 0;
			foreach (char a in w)
			{
				if (a.Equals(v)) letterIndexes.Add(charCounter);

				charCounter++;
			}
			int indexUpdated = this.updatedWord.IndexOf(v);
			int indexWord = word.IndexOf(v);
			int index = indexUpdated;
			int difference = word.Length - this.updatedWord.Length;
			this.updatedWord = this.updatedWord.Remove(this.updatedWord.IndexOf(v),1);
			
			return letterIndexes;
		}

		/// <summary>
		/// Generates a sequence of the supplied string, multiplied by n.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public string CreateRepeatedString(string s, int n)
		{

			return new StringBuilder(s.Length * n)

						.Insert(0, s, n)

						.ToString();


		}
/// <summary>
/// Updates the HangWord string with a correct guess.
/// e.g. "_ _ _ " -> "_ a _ " -> "c a _ " -> "c a t " 
/// </summary>
/// <param name="index"></param>
		public void updateHangGuess(List<int> letterIndexes)
		{
			//int x = 4;
			//System.Console.WriteLine(index.ToString());
			foreach (int i in letterIndexes)
			{
				this.hungWord = this.hungWord.Remove(i == 0 ? 0 : i * 2, 1)
												.Insert(i == 0 ? 0 : i * 2, this.currentLetter.ToString());
				if (!this.hungWord.Contains("_"))
				{
				//System.Console.WriteLine("Congratulations!");
				guessesLeft = -1;

				}
			}
			
			//so, 0,1,2,3,4,5 would go to 0,2,4,6,8,10
			//HangWord = HangWord.
		}

		public void RunGame()
		{
			hungWord = CreateRepeatedString("_ ", word.Length);
			System.Console.WriteLine(CreateRepeatedString("_ ", word.Length));
			System.Console.WriteLine($"Your word is {word.Length} letters long! You have 6 guesses, good luck! ");
			//currentLetter = currentGame.submittedGuess();
			do
			{
				currentLetter = SubmittedGuess();
				processCurrentLetter();

				
			} while (guessesLeft > 0);

			if (guessesLeft < 0) Console.WriteLine("Congratulations, you are a winner!");
			else Console.WriteLine($"Sorry, you lost! The word was {word}");

		}

		private void processCurrentLetter()
		{
			if (validLetterGuess(currentLetter))
			{
				CheckForMultipleLetters();
				if (!missedLetters.Contains(currentLetter) && !word.Contains(currentLetter))
				{
					missedLetters.Add(currentLetter);
					guessesLeft--;
					Console.WriteLine($"You have {guessesLeft} guesses left.");
				}
				//else if (HangWord.Contains(currentLetter)) System.Console.WriteLine("You already guessed that letter!");
				System.Console.WriteLine(hungWord);
				PrintMissedLetters();
			}
			else System.Console.WriteLine($"*{currentLetter}* is not a valid letter");
		}

		private int CheckForMultipleLetters()
		{
			int i = 0;
			while (LetterInWord(currentLetter, updatedWord))
			{
				i++;
				if (i == 1) System.Console.WriteLine("Letter is in word");
				else Console.WriteLine("Letter is in word again, luck is on your side!");
				updateHangGuess(UpdateWord(word, currentLetter));
				updateLettersLeft(currentLetter);
			}

			return i;
		}

		private void PrintMissedLetters()
		{
			Console.Write("Misses: ");
			foreach (char a in missedLetters) Console.Write($"{a} ");
			Console.Write("\n");
		}
	}


}
