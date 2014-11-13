using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechF.Examples.BasicWebApi.Models;

namespace TechF.Examples.BasicWebApi
{
    public class DummyService : IDummyService
    {
        private IDummyDataSource _dataSource;

        public DummyService(IDummyDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IEnumerable<DummyData> GetData()
        {
            return _dataSource.GetData();
        }
    }
}