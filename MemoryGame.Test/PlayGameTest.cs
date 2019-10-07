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
    public class PlayGameTest
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
        
        private void InitializeGame(SlideCategories cat)
        {
            startMenuDataContext.SelectedCategory = (int)cat;
            startMenuDataContext.PlayCommand.Execute(startMenuDataContext.CanPlay);
            currDataContext = mainWindow.DataContext;
            gameDataContext = currDataContext as GameViewModel;
        }

        [DataTestMethod]
        [DataRow(SlideCategories.Animals)]
        [DataRow(SlideCategories.Cars)]
        public void PlayGame(SlideCategories cat)
        {
            InitializeGame(cat);
        }
    }
}