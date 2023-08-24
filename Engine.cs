using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParserEngine
{
    public class Engine
    {
        public static IWebDriver driver;

        /// <summary>
        /// Запуск движка Chrome
        /// </summary>
        public static void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--headless");
            options.AddArgument("--disable-dev-shm-usage");
            var driverPath = Environment.CurrentDirectory + @"\chromedriver.exe";
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath);
            service.HideCommandPromptWindow = true;
            driver = new ChromeDriver(service);
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Переход по ссылке из команды
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool GoToUri(string uri)
        {
            try
            {
                driver.Navigate().GoToUrl(uri);
                Thread.Sleep(10000);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Получение значения со страницы по XPath из команды
        /// </summary>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static string ParseInfo(string XPath)
        {
            try
            {
                string _element = "";
                List<IWebElement> element = driver.FindElements(By.XPath(XPath)).ToList();
                for (int z = 0; z < element.Count; z++)
                {
                    _element = System.Text.RegularExpressions.Regex.Match(element[z].Text, ".*").Value;
                }
                return _element;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "!!!ERROR!!!";
            }
        }
    }
}
