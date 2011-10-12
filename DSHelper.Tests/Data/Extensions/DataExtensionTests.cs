using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DSHelper.Data.Extensions
{
    [TestFixture]
    public class DataExtensionTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsDirty_Null_Throws()
        {
            DataRow row = null;
            row.IsDirty();
        }

        [Test]
        public void IsDirty_Detached_IsFalse()
        {
            var table = new DataTable();
            var row = table.NewRow();

            Assert.That(row.IsDirty(), Is.False);
        }

        [Test]
        public void IsDirty_Added_IsTrue()
        {
            var table = new DataTable();
            var row = table.NewRow();
            table.Rows.Add(row);

            Assert.That(row.IsDirty(), Is.True);
        }
    }
}
