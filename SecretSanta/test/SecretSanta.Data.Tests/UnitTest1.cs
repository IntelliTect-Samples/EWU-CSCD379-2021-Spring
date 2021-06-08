using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using DbContext db = new();
            db.Groups.Add(new Group { Name = "Tyler" });
            db.SaveChanges();
        }
    }
}
