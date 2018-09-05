namespace BankOcr
{
    using System;
    using System.IO;

    using NUnit.Framework;

    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var dir = Path.GetDirectoryName(typeof(BankOcrShould).Assembly.Location);
            TestContext.WriteLine($"Current dir = '{dir}'");
            Environment.CurrentDirectory = dir;
        }
    }
}