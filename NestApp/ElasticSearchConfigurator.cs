using Nest;
using NestApp.Entities;
using System;
using System.Collections.Generic;

namespace NestApp
{
    public class ElasticSearchConfigurator
    {
        private readonly ConnectionSettings _settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("store");
        private readonly ElasticClient _client;
        private readonly SqlTableDependencyConfigurator _sqlConfigurator;

        public ElasticSearchConfigurator()
        {
            _client = new ElasticClient(_settings);
            _sqlConfigurator = new SqlTableDependencyConfigurator(_client);
        }

        public void IndexProduct(Product product)
        {
            var indexResponse = _client.IndexDocument(product);
        }

        public void IndexProducts(List<Product> products)
        {
            var indexResponse = _client.IndexMany(products);
        }

        public void SearchByTitle(string name)
        {
            var response = _client.Search<Product>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Title)
                        .Query(name)
                    )
                )
            );

            var result = response.Documents;
        }

        public void AggregateByCount()
        {
            var response = _client.Search<Product>(s => s
                .From(0)
                .Size(10)
                .Aggregations(a => a
                    .Terms("age", t => t
                        .Field(f => f.Count)
                    )
                )
            );

            var result = response.Aggregations.Terms("count");
        }
    }
}