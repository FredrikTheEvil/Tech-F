using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechF.Examples.BasicWebApi.Models;

namespace TechF.Examples.BasicWebApi
{
    public class DummyDataSource : IDummyDataSource
    {
        public string Text { get; set; }

        public IEnumerable<DummyData> GetData()
        {
            return new DummyData[]
            {
                new DummyData() { Text = Text }
            };
        }
    }
}