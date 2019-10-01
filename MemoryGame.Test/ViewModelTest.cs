using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoryGame;
using MemoryGame.Views;
using MemoryGame.ViewModels;
using MemoryGame.Models;
using System.IO;
using System.Linq;

namespace MemoryGame.Test
{
    [TestClass]
    public class ViewModelTests
    {
        static MainWindow mainWindow;
        static object currDataContext;
        StartMenuViewModel startMenuDataContext;
        GameViewModel gameDataContext;
        //Obtain the maximum number (category) allowed
        static readonly int maxCategory = Enum.GetValues(typeof(SlideCategories)).Cast<int>().Max();

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
            startMenuDataContext.StartNewGame();
            currDataContext = mainWindow.DataContext;

            Assert.IsTrue(CheckType(currDataContext, typeof(StartMenuViewModel)), "Expected DataContext not to change, but it did.");
        }

        [TestMethod]
        public void SelectValidCategory()
        {
            for (int cat = 1; cat <= maxCategory; cat++)
            {
                startMenuDataContext.SelectedCategory = cat;
                startMenuDataContext.StartNewGame();
                currDataContext = mainWindow.DataContext;
                gameDataContext = currDataContext as GameViewModel;

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

            //If if can't find a named category (1-3 have names), it looks for the directory
            // called Assets/##, which should be an exception
            Assert.ThrowsException<DirectoryNotFoundException>(new Action(startMenuDataContext.StartNewGame));
        }

        //Test out the game initialization with Animals
        [TestMethod]
        public void InitializeGame()
        {
            startMenuDataContext.SelectedCategory = (int)SlideCategories.Animals;
            startMenuDataContext.StartNewGame();
            currDataContext = mainWindow.DataContext;
            gameDataContext = currDataContext as GameViewModel;

            //Make sure the correct category is chosen
            Assert.AreEqual(gameDataContext.Category, (SlideCategories)startMenuDataContext.SelectedCategory, "Expected category not selected.");
            //Make sure the Timer is created and set to 0 sec
            Assert.AreEqual(gameDataContext.Timer.Time.Seconds, 0);
        }
    }
}