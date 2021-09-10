using NUnit.Framework;
using System.Collections.Generic;

namespace InstaHelper.Tests
{
    class SortTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SortMembersIncorrect()
        {
            List<string> input = new List<string>() { "masha", "misha", "vasya", "lena", "kirill" };
            List<string> del_list = new List<string>() { "misha", "lena" };
            List<string> expected = new List<string> { "masha", "vasya", "kirill" };

            ForTests test = new ForTests();
            List<string> actual = test.SortMembers(input, del_list);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SortMembersCorrect()
        {
            List<string> input = new List<string>() { "masha", "vasya", "kirill" };
            List<string> del_list = new List<string>() { "misha", "lena" };
            List<string> expected = new List<string> { "masha", "vasya", "kirill" };

            ForTests test = new ForTests();
            List<string> actual = test.SortMembers(input, del_list);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SortMembersEmpty()
        {
            List<string> input = new List<string>() { "masha", "misha", "vasya", "lena", "kirill" };
            List<string> del_list = new List<string>();
            List<string> expected = new List<string> { "masha", "misha", "vasya", "lena", "kirill" };

            ForTests test = new ForTests();
            List<string> actual = test.SortMembers(input, del_list);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SortMembersFullDel()
        {
            List<string> input = new List<string>() { "masha", "misha", "vasya", "lena", "kirill" };
            List<string> del_list = new List<string>() { "masha", "misha", "vasya", "lena", "kirill" };
            List<string> expected = new List<string> { };

            ForTests test = new ForTests();
            List<string> actual = test.SortMembers(input, del_list);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SortMembersDuplicate()
        {
            List<string> input = new List<string>() { "masha", "masha", "misha", "vasya", "masha", "lena", "kirill", "vasya", "kirill", "vasya" };
            List<string> del_list = new List<string>() { "masha", "vasya"};
            List<string> expected = new List<string> { "misha", "lena", "kirill", "kirill"};

            ForTests test = new ForTests();
            List<string> actual = test.SortMembers(input, del_list);

            Assert.AreEqual(expected, actual);
        }
    }    
}
