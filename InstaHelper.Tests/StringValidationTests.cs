using NUnit.Framework;

namespace InstaHelper.Tests
{
    public class StringValidationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UsernameIncorrectInput()
        {
            string input = "/uSe;r!naM e@";
            bool expected = false;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramUsername(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UsernameCorrectInput()
        {
            string input = "username.user";
            bool expected = true;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramUsername(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UsernameEmptyInput()
        {
            string input = "";
            bool expected = false;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramUsername(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UsernameCyrillicInput()
        {
            string input = "username_Кириллица_user";
            bool expected = false;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramUsername(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UsernameChangeIncorrectInput()
        {
            string input = "uSernaMe";
            string expected = "username";

            ForTests test = new ForTests();
            string actual = test.ValidateInstagramUsername(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UsernameChangeCorrectInput()
        {
            string input = "username";
            string expected = "username";

            ForTests test = new ForTests();
            string actual = test.ValidateInstagramUsername(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkIncorrectInput()
        {
            string input = "https://www.habr.com/";
            bool expected = false;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkCorrectInput()
        {
            string input = "https://www.instagram.com/p/CQyfZfyAa-9/";
            bool expected = true;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkCyrillicInput()
        {
            string input = "https://www.instagram.com/p/ссылка-на-пост/";
            bool expected = false;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkEmptyInput()
        {
            string input = "";
            bool expected = false;

            ForTests test = new ForTests();
            bool actual = test.IsValidInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkChangeCorrect()
        {
            string input = "https://www.instagram.com/p/CQyfZfyAa-9/";
            string expected = "https://www.instagram.com/p/CQyfZfyAa-9/";

            ForTests test = new ForTests();
            string actual = test.ValidateInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkChangeIncorrect()
        {
            string input = "https://www.instagram.com/p/CQyfZfyAa-9/?utm_medium=copy_lnk";
            string expected = "https://www.instagram.com/p/CQyfZfyAa-9/";

            ForTests test = new ForTests();
            string actual = test.ValidateInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PostlinkChangeIncorrectWithArgs()
        {
            string input = "https://www.instagram.com/p/CQyfZfyAa-9/?utm_medium=copy_lnk?utm=123&crm=66";
            string expected = "https://www.instagram.com/p/CQyfZfyAa-9/";

            ForTests test = new ForTests();
            string actual = test.ValidateInstagramPostlink(input);

            Assert.AreEqual(expected, actual);
        }
    }
}