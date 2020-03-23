using System;
using Xunit;
using System.IO;
using Hman;
using System.Collections.Generic;


namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private const char V = 'r';

        [Fact]
        public void Test1()
        {
            Hangman game = new Hangman();
            Assert.NotNull(game.wordsList);
        }

        [Fact]
        public void selectWord()
        {
            Hangman game = new Hangman();
    
            //game.newWord();
            Assert.NotNull(game.word);
        }
        [Fact]
        public void wordLength()
        {
            Hangman game = new Hangman();
            foreach(string w in game.wordsList)
            {
                Assert.InRange(w.Length, game.minWordLength, game.maxWordLength);
            }
            //Assert.Equal(game.word.Length, game.word.Length);
            
        }
        [Fact]
        public void guesesLeft()
        {
            Hangman game = new Hangman();
            Assert.Equal(6, game.guessesLeft);
        }
        
        [Fact]
        public void guessIsValidLetter()
        {
            Hangman game = new Hangman();
            char testString = 'r';
            Assert.True(game.ValidLetterGuess(testString));
        }
        [Fact]
        public void testLetterInWord()
        {
            Hangman game = new Hangman();
            string testString = "r\r";
            var sr = new StringReader(testString);
            Console.SetIn(sr);
            game.word = "road";
            Assert.True(game.LetterInWord('r', game.word));

        }
        [Fact]
        public void testUpdateWord()
        {
            Hangman game = new Hangman();
            game.word = "road";
            game.UnguessedLetters = game.word;
            game.IndexesOfLetters("road", 'r');
            Assert.Equal("oad", game.UnguessedLetters);
        }

        [Fact]
        public void testUpdateHangWord()
        {
            Hangman game = new Hangman();
            string testString = "r\r";
            var sr = new StringReader(testString);
            Console.SetIn(sr);
            game.word = "road";
            game.UnguessedLetters = "road";
            game.currentLetter = 'r';
            List<int> index = game.IndexesOfLetters(game.word, 'r');
            Assert.Equal("oad", game.UnguessedLetters);
            game.UpdateLettersLeft('r');
            //int index = game.UpdateWord(game.word, 'r');
            Assert.Equal(new List<int>() { 0 },index);
            game.hungWord = game.CreateRepeatedString("_ ", game.word.Length);
            game.UpdateHangGuess(index);
            Assert.Equal("r _ _ _ ", game.hungWord);
        }
    }
}
