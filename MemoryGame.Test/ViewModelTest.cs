using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoryGame;
using MemoryGame.Views;
using MemoryGame.ViewModels;
using MemoryGame.Models;
using System.IO;
using System.Linq;
using System.Windows;

namespace MemoryGame.Test
{
    [TestClass]
    public class ViewModelTest
    {
        static MainWindow mainWindow;
        static object currDataContext;
        StartMenuViewModel startMenuDataContext;
        GameViewModel gameDataContext;
        //Obtain the maximum number (category) allowed
        static readonly Array allCategories = Enum.GetValues(typeof(SlideCategories));
        static readonly int maxCategory = allCategories.Cast<int>().Max();

        //This method is called before every test. 
        [TestInitialize]
        public void Setup()
        {
            mainWindow = new MainWindow();
            currDataContext = mainWindow.DataContext as StartMenuViewModel;
            startMenuDataContext = currDataContext as StartMenuViewModel;
            gameDataContext = null;
        }

        public bool CheckType(object obj, Type t)
        {
            if (obj != null && obj.GetType() == t)
            {
                return true;
            }
            return false;
        }

        [TestMethod]
        public void CheckStartingVM()
        {
            Assert.IsTrue(CheckType(currDataContext, typeof(StartMenuViewModel)));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-20)]
        public void SelectNothing(int belowRange)
        {
            startMenuDataContext.SelectedCategory = belowRange;

            //Make sure the Play Button is disabled
            Assert.IsFalse(startMenuDataContext.PlayCommand.CanExecute(startMenuDataContext.CanPlay));
        }

        [TestMethod]
        public void SelectValidCategory()
        {
            foreach (SlideCategories cat in allCategories)
            {
                startMenuDataContext.SelectedCategory = (int)cat;
                startMenuDataContext.StartNewGame();
                currDataContext = mainWindow.DataContext;

                //Make sure the DataContext correctly changed to a GameViewModel
                Assert.IsTrue(CheckType(currDataContext, typeof(GameViewModel)), "Expected DataContext to become a GameViewModel, but it did not.");
            }
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(1000)]
        public void SelectError(int errorNum)
        {
            //Add the passed integer to the max value of the Categories enum,
            // so we are guaranteed that the number we try is above the range of allowed categories
            startMenuDataContext.SelectedCategory = maxCategory + errorNum;

            //Make sure the Play Button is disabled
            Assert.IsFalse(startMenuDataContext.PlayCommand.CanExecute(startMenuDataContext.CanPlay));
        }

        //Test out the game initialization with a category
        [DataTestMethod]
        [DataRow(SlideCategories.Animals)]
        [DataRow(SlideCategories.Foods)]
        public void InitializeGame(SlideCategories cat)
        {
            startMenuDataContext.SelectedCategory = (int)cat;
            startMenuDataContext.PlayCommand.Execute(startMenuDataContext.CanPlay);
            currDataContext = mainWindow.DataContext;
            gameDataContext = currDataContext as GameViewModel;
            GameInfoViewModel info = gameDataContext.GameInfo;

            //Make sure the correct category is chosen
            Assert.AreEqual(gameDataContext.Category, (SlideCategories)startMenuDataContext.SelectedCategory, "Expected category not selected.");
            //Make sure the Timer is created and set to 0 sec
            Assert.AreEqual(gameDataContext.Timer.Time.Seconds, 0);
            //Make sure the GameInfo context is set up correctly
            Assert.IsTrue(CheckType(info, typeof(GameInfoViewModel)));
            //Make sure you have themax match attempts
            Assert.AreEqual(info.MatchAttempts, 4);
            //Make sure the score is set to 0
            Assert.AreEqual(info.Score, 0);
            //Make sure win and lost message are hidden
            Assert.AreEqual(info.WinMessage, Visibility.Hidden);
            Assert.AreEqual(info.LostMessage, Visibility.Hidden);
        }
    }
}