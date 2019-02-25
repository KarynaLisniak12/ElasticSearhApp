using Nest;
using NestApp.Entities;
using System;
using System.Collections.Generic;

namespace NestApp
{
    public class ElasticSearchConfigurator
    {
        private readonly ConnectionSettings _settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("users");
        private readonly ElasticClient _client;

        public ElasticSearchConfigurator()
        {
            _client = new ElasticClient(_settings);
        }

        public void IndexPerson(Person person)
        {
            var indexResponse = _client.IndexDocument(person);
        }

        public void IndexPeople(List<Person> people)
        {
            var indexResponse = _client.IndexMany(people);
        }

        public void SearchByName(string name)
        {
            var response = _client.Search<Person>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(name)
                    )
                )
            );

            var result = response.Documents;
        }

        public void AggregateByAge()
        {
            var response = _client.Search<Person>(s => s
                .From(0)
                .Size(10)
                .Aggregations(a => a
                    .Terms("age", t => t
                        .Field(f => f.Age)
                    )
                )
            );

            var result = response.Aggregations.Terms("age");
        }
    }
}