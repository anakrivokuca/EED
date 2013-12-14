using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EED.Domain;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace EED.Unit.Tests
{
    [TestFixture]
    public class GenerateSchema
    {
        [Test]
        public void Can_generate_schema()
        {
            var config = new Configuration();
            config.Configure();
            config.AddAssembly(typeof(User).Assembly);

            new SchemaExport(config).Execute(false, true, false);
        }
    }
}
