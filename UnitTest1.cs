using FluentAssertions;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace LoanCalculator
{
    public enum btype
    {
        Chrome,
        Edge
    }

    public class Tests
    {
        IWebDriver? driver;

        [SetUp]
        public void Setup()
        {
            driver = RunBrowser3(btype.Chrome);
            driver.Manage().Window.Maximize();
        }

        //Example 3
        public IWebDriver RunBrowser3(btype type) => type switch
        {
            btype.Chrome => driver = new ChromeDriver(),
            btype.Edge => driver = new EdgeDriver(),
            _=> throw new Exception("Browser not supported")
        };

        //Example 2
        public IWebDriver RunBrowser2(btype type) //tenery conditional statement
        {
            var choice = type.Equals(btype.Chrome)
                ? driver = new ChromeDriver()
                : type.Equals(btype.Edge) ? driver = new EdgeDriver()
                : throw new ArgumentException("Not supported browser");
            return driver;
        }

        //Example 1
        public IWebDriver RunBrowser(btype type)
        {
            if (type == btype.Chrome)
            {
                driver = new ChromeDriver();
            }
            else if(type == btype.Edge)
            {
                driver = new EdgeDriver();
            }
            return driver;
        }

        private static IConfigurationRoot config;
        public static IConfigurationRoot Initialize()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json");
            config = builder.Build();
            return config;
        }

        public static string addedPath(string path, string addedpath) => string.Format(path, addedpath);
        
        [Test]
        public void ValidTest()
        {
            driver?.Navigate().GoToUrl(Environment.loanCalculatorUrl);
            driver?.FindElement(By.Id("dismiss-notification-banner")).Click();
            IWebElement toBorrow = driver.FindElement(By.Id(Locators.toBorrow));
            toBorrow.SendKeys("20,000");
            IWebElement propertValue = driver.FindElement(By.Id(Locators.propertyValue));
            propertValue.SendKeys("150,000");
            IWebElement calculateButton = driver.FindElement(By.Id(Locators.calculateButton));
            calculateButton.Click();

            string loanValue = driver.FindElement(By.Id("ltv_calculator_ltv")).GetAttribute("value");
            Assert.IsTrue(loanValue.Equals("13.3%"));
            loanValue.Should().Be("13.3%");
            driver.Quit();
        }

        [Test]
        public void DemoQaTest()
        {
            //driver.Navigate().GoToUrl(TestContext.Parameters.Get("url"));
            driver?.Navigate().GoToUrl(addedPath(Initialize().GetValue<string>("env:url2"), paths.elements.ToString()));
            Thread.Sleep(1000);
            driver?.FindElement(By.Id("item-0")).Click();
            driver?.FindElement(By.Id("userName")).SendKeys("Victor");
            driver?.FindElement(By.Id("userEmail")).SendKeys("abc@abc.com");
            driver?.FindElement(By.Id("currentAddress")).SendKeys("My current address");
            driver?.FindElement(By.XPath("//*[@id='permanentAddress']")).SendKeys("My parmanent address");
            driver?.FindElement(By.Id("submit")).Click();

            Thread.Sleep(3000);
            var registeredinfo = driver?.FindElements(By.XPath("//*[@class='border col-md-12 col-sm-12']/p"));
            //registeredinfo?[0].Text.Split(":")[1].Should().Be("Victor");
            //registeredinfo?[1].Text.Split(":")[1].Should().Be("abc@abc.com");
            //registeredinfo?[2].Text.Split(":")[1].Should().Be("My current address");
            //registeredinfo?[3].Text.Split(":")[1].Should().Be("My parmanent address");
            //--------------------------------------------------------------------------
            registeredinfo?.FirstOrDefault()?.Text.Split(":").ElementAt(1)?.Should().Be("Victor");
            registeredinfo?.ElementAt((int)index.One)?.Text.Split(":").ElementAt((int)index.One).Should().Be("abc@abc.com");
            registeredinfo?.ElementAt((int)index.Two)?.Text.Split(":").ElementAt((int)index.One).Should().Be("My current address");
            registeredinfo?.ElementAt((int)index.Three)?.Text.Split(":").ElementAt((int)index.One).Should().Be("My parmanent address");
            //--------------------------------------------------------------------------
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Victor", registeredinfo?[(int)index.zero].Text.Split(":")[(int)index.One]);
                Assert.AreEqual("abc@abc.com", registeredinfo?[(int)index.One].Text.Split(":")[(int)index.One]);
                Assert.AreEqual("My current address", registeredinfo?[(int)index.Two].Text.Split(":")[(int)index.One]);
                Assert.AreEqual("My parmanent address", registeredinfo?[(int)index.Three].Text.Split(":")[(int)index.One]);
            });

            Thread.Sleep(3000);

            registeredinfo?.ElementAt((int)index.Three)?.Text.Split(":").ElementAt((int)index.One).Should().NotBeEmpty(string.Empty);
            registeredinfo?.ElementAt((int)index.Three)?.Text.Split(":").ElementAt((int)index.One).Should().NotBeNullOrEmpty(null);

            driver?.Quit();
        }

        public enum paths
        {
            elements
        }

        public enum index
        {
            zero = 0,
            One = 1,
            Two = 2,
            Three = 3
        }
    }
}