using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Test_CinemaArt
    {
        private IWebDriver _driver;
        [SetUp]
        public void SetupDriver()
        {
            _driver = new ChromeDriver("C:\\Users\\USER\\Desktop\\chromedriver_win32 (2)");
        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

        [Test]
        public void EmagGeniusTextExists()
        {
            _driver.Url = "http://localhost:4200/movies";
            bool foundGenius = false;
            foreach (var span in _driver.FindElements(By.TagName("span")))
            {
                if (span.Text == "eMAG Genius")
                {
                    foundGenius = true;
                    break;
                }
            }
            Assert.IsTrue(foundGenius);
        }

        [Test]
        public void EmagGeniusPageHasLogo()
        {
            _driver.Url = "http://localhost:4200/movies";
            bool foundGenius = false;
            foreach (var span in _driver.FindElements(By.TagName("span")))
            {
                if (span.Text == "eMAG Genius")
                {
                    foundGenius = true;
                    span.Click();
                    try
                    {
                        _driver.FindElement(By.XPath("//img[@class='g-logo']"));
                        //Assert.True(true);
                    }
                    catch (NoSuchElementException)
                    {
                        Assert.Fail("eMAG Genius logo not found.");
                    }
                    break;
                }
            }
            Assert.IsTrue(foundGenius);
        }
    }
}
