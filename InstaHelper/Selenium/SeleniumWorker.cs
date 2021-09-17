using InstaHelper.DataBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace InstaHelper.Selenium
{
    public class SeleniumWorker : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly ChromeOptions options;

        private readonly IDataBaseWorker dataBase;

        public SeleniumWorker(IDataBaseWorker data)
        {
            dataBase = data;

            options = new ChromeOptions();
            options.BinaryLocation = Environment.GetEnvironmentVariable("GOOGLE_CHROME_BIN"); //heroku

            options.AddArgument("window-size=375,812");
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--blink-settings=imagesEnabled=false");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1");

            options.AddArguments($"user-data-dir={Directory.GetCurrentDirectory()}/userdata");

            //driver = new ChromeDriver("D:\\Work\\InstaHelper\\InstaHelper\\bin\\Debug\\netcoreapp3.1", options);
            driver = new ChromeDriver(Environment.GetEnvironmentVariable("CHROMEDRIVER_PATH"), options); //heroku
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            Authorization();
        }

        private void Authorization()
        {
            driver.Navigate().GoToUrl("https://www.instagram.com");
            
            try
            {
                //IWebElement buttonAcceptCookies = driver.FindElement(By.XPath("//*[contains(text(), 'Принять все')]"));
                IWebElement buttonAcceptCookies = driver.FindElement(By.XPath("//*[contains(text(), 'Accept All')]"));
                buttonAcceptCookies.Click();
                Thread.Sleep(5000);
            }
            catch { Console.WriteLine("Error accept cookies"); }

            try
            {
                //IWebElement buttonStartLogin = driver.FindElement(By.XPath("//button[contains(text(), 'Войти')]"));
                IWebElement buttonStartLogin = driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));
                buttonStartLogin.Click();
                Thread.Sleep(5000);
            }
            catch { Console.WriteLine("Error click button start login"); }

            try
            {
                IWebElement loginEl = driver.FindElement(By.XPath("//input[@name='username']"));
                IWebElement passwordEl = driver.FindElement(By.XPath("//input[@name='password']"));
                IWebElement buttonLogin = driver.FindElement(By.XPath("//button[@type='submit']"));

                loginEl.SendKeys(AuthData.INSTAGRAM_LOGIN);
                passwordEl.SendKeys(AuthData.INSTAGRAM_PASSWORD);
                buttonLogin.Click();
                Thread.Sleep(5000);
            }
            catch { Console.WriteLine("Error input username password"); }

            try
            {
                //IWebElement buttonStartLogin = driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]"));
                IWebElement buttonSaveInfo = driver.FindElement(By.XPath("//*[contains(text(), 'Save Info')]"));
                buttonSaveInfo.Click();
                Thread.Sleep(5000);
            }
            catch { Console.WriteLine("Error save username password"); }
        }

        public List<string> SubscribersParser(long id)
        {
            List<string> accounts = new List<string>();

            List<IWebElement> followers = GetFollowers(id);

            if (followers.Count != 0)
            {
                accounts = followers.Select(x => x.Text).ToList();
            }

            return accounts;
        }

        public List<string> Competition(long id, out string winner)
        {
            List<string> members = new List<string>();

            List<IWebElement> comments = GetComments(id);

            winner = "";

            Random r = new Random();

            if (comments.Count != 0)
            {
                members = comments.Select(x => x.Text).ToList();

                string myUsername = GetMyUsername(id);
                members = members.Where(x => x != myUsername).ToList();

                bool unique = dataBase.GetCompetUnique(id);
                bool auth = dataBase.GetCompetAuth(id);
                string username = dataBase.GetCompetUserLogin(id);

                if(unique)
                {
                    members = UniqueUsers(members);
                }

                if(auth)
                {
                    winner = DontSubscribeMember(members, username);
                }
                else
                {
                    winner = members[r.Next(0, members.Count - 1)];
                }
            }

            return members;
        }

        private List<IWebElement> GetFollowers(long id)
        {
            List<IWebElement> followers = new List<IWebElement>();
            Random r = new Random();

            if (CheckUser(id))
            {
                try
                {
                    IWebElement buttonSubscribers = driver.FindElement(By.XPath("//li//a[contains(@href, 'followers')]"));
                    buttonSubscribers.Click();
                }
                catch { Console.WriteLine("Error open followers"); }

                followers = driver.FindElements(By.XPath("//li[@class='wo9IH']//div[@class='enpQJ']//a")).ToList();
                int countFollowers = 0;

                while (countFollowers != followers.Count && followers.Count < 1000)
                {
                    countFollowers = followers.Count;

                    IWebElement anyElement = driver.FindElement(By.XPath("//a"));

                    anyElement.SendKeys(Keys.PageDown);
                    Thread.Sleep(r.Next(500, 1000));
                    anyElement.SendKeys(Keys.PageDown);
                    Thread.Sleep(r.Next(500, 1000));
                    anyElement.SendKeys(Keys.PageDown);
                    Thread.Sleep(r.Next(500, 1000));
                    anyElement.SendKeys(Keys.PageDown);
                    Thread.Sleep(r.Next(500, 1000));
                    anyElement.SendKeys(Keys.PageDown);

                    followers.Clear();
                    followers = driver.FindElements(By.XPath("//li[@class='wo9IH']//div[@class='enpQJ']//a")).ToList();
                }
            }

            return followers;
        }

        private bool CheckUser(long id)
        {
            string username = dataBase.GetParser(id);

            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            else
            {
                driver.Navigate().GoToUrl($"https://www.instagram.com/{username}");

                //List<IWebElement> error = driver.FindElements(By.XPath($"//h2[contains(text(), 'недоступна')]")).ToList();
                List<IWebElement> error = driver.FindElements(By.XPath($"//h2[contains(text(),'t available')]")).ToList();

                if (error.Count == 0)
                {
                    //error = driver.FindElements(By.XPath($"//h2[contains(text(), 'закрытый')]")).ToList();
                    error = driver.FindElements(By.XPath($"//h2[contains(text(), 'Account is Private')]")).ToList();

                    if(error.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private List<IWebElement> GetComments(long id)
        {
            List<IWebElement> comments = new List<IWebElement>();
            Random r = new Random();

            if(CheckPostlink(id))
            {
                comments = driver.FindElements(By.XPath("//h3//a")).ToList();
                int countComments = 0;

                while (countComments != comments.Count)
                {
                    countComments = comments.Count;

                    try
                    {
                        //IWebElement buttonMoreComments = driver.FindElement(By.XPath("//span[@aria-label='Загрузить ещё комментарии']"));
                        IWebElement buttonMoreComments = driver.FindElement(By.XPath("//span[@aria-label='Load more comments']"));
                        buttonMoreComments.Click();

                        Thread.Sleep(r.Next(500, 1000));
                    }
                    catch { Console.WriteLine("Error open more comments"); }
                    finally
                    {
                        comments.Clear();
                        comments = driver.FindElements(By.XPath("//h3//a")).ToList();
                    }
                }
            }

            return comments;
        }

        private bool CheckPostlink(long id)
        {
            string postlink = dataBase.GetCompetPostlink(id);

            if (string.IsNullOrEmpty(postlink))
            {
                return false;
            }
            else
            {
                driver.Navigate().GoToUrl($"{postlink}comments");

                //List<IWebElement> error = driver.FindElements(By.XPath($"//h2[contains(text(), 'недоступна')]")).ToList();
                List<IWebElement> error = driver.FindElements(By.XPath($"//h2[contains(text(),'t available')]")).ToList();

                if (error.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string DontSubscribeMember(List<string> users, string username)
        {
            Random r = new Random();

            while(users.Count != 0)
            {
                users = users.Where(x => x != username).ToList();

                string potentialWinner = users[r.Next(0, users.Count - 1)];

                if(CheckMemberSubscribe(potentialWinner, username))
                {
                    return potentialWinner;
                }
                else
                {
                    users = users.Where(x => x != potentialWinner).ToList();
                }
            }

            return "";
        }

        private bool CheckMemberSubscribe(string member, string myUsername)
        {
            List<IWebElement> followings = new List<IWebElement>();
            Random r = new Random();

            driver.Navigate().GoToUrl($"https://www.instagram.com/{member}");

            try
            {
                IWebElement buttonSubscribes = driver.FindElement(By.XPath("//li//a[contains(@href, 'following')]"));
                buttonSubscribes.Click();
            }
            catch
            {
                Console.WriteLine("Error open users following");
                return false;
            }

            followings = driver.FindElements(By.XPath("//li[@class='wo9IH']//div[@class='enpQJ']//a")).ToList();
            int countFollowings = 0;

            bool trigger = false;

            while (countFollowings != followings.Count && followings.Count < 1000)
            {
                countFollowings = followings.Count;

                IWebElement anyElement = driver.FindElement(By.XPath("//a"));

                anyElement.SendKeys(Keys.PageDown);
                Thread.Sleep(r.Next(500, 1000));
                anyElement.SendKeys(Keys.PageDown);
                Thread.Sleep(r.Next(500, 1000));
                anyElement.SendKeys(Keys.PageDown);

                followings.Clear();
                followings = driver.FindElements(By.XPath("//li[@class='wo9IH']//div[@class='enpQJ']//a")).ToList();

                foreach (IWebElement el in followings)
                {
                    if (el.Text == myUsername)
                    {
                        trigger = true;
                    }
                }

                if (trigger)
                {
                    break;
                }
            }

            if (!trigger)
            {
                return false;
            }

            return true;
        }

        private string GetMyUsername(long id)
        {
            string postlink = dataBase.GetCompetPostlink(id);
            string MyUsername = "";

            driver.Navigate().GoToUrl($"{postlink}");

            try
            {
                MyUsername = driver.FindElement(By.XPath("//header//a[contains(@class, 'sqdOP')]")).Text;
            }
            catch { Console.WriteLine("Error get MyUsername"); }

            return MyUsername;
        }

        private List<string> UniqueUsers(List<string> users)
        {
            users = users.Distinct().ToList();
            return users;
        }

        public void Dispose()
        {
            if(driver!=null)
            {
                driver.Quit();
            }
        }
    }
}
