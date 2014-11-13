using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TechF.Examples.BasicWebApi.Controllers
{
    public class DummyController : ApiController
    {
        private IDummyService _dummyService;

        public DummyController(IDummyService service)
        {
            _dummyService = service;
        }

        public IEnumerable<Models.DummyData> GetDummyData()
        {
            return _dummyService.GetData();
        }
    }
}
