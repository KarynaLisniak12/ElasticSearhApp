using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var configurator = new ElasticSearchConfigurator();
            configurator.SearchByName("Polly");

            //Person person = new Person()
            //{
            //    Id = 6, 
            //    Name = "Polly",
            //    Age = 3
            //};
            //configurator.IndexPerson(person);

            //Person person1 = new Person()
            //{
            //    Id = 7,
            //    Name = "Jolly",
            //    Age = 15
            //};
            //Person person2 = new Person()
            //{
            //    Id = 8,
            //    Name = "Holly",
            //    Age = 22
            //};
            //configurator.IndexPeople(new List<Person>() { person1, person2 });

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
