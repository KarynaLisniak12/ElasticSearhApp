using System.Threading;
using Nest;
using NestApp.Entities;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace NestApp
{
    public class SqlTableDependencyConfigurator
    {
        private readonly string _connectionString = @"data source=localhost;
                                                      initial catalog = TestStoreDb;  
                                                      Integrated Security = True;";

        private readonly ElasticClient _elasticClient;

        public SqlTableDependencyConfigurator(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;

            using (var sqlDependency = new SqlTableDependency<Product>(_connectionString, "Products"))
            {
                sqlDependency.OnChanged += Changed;
                sqlDependency.Start();

                Thread.Sleep(50000);
                
                sqlDependency.Stop();
            }
        }

        private void Changed(object sender, RecordChangedEventArgs<Product> e)
        {
            Product changedEntity = e.Entity;

            if (e.ChangeType == ChangeType.Update)
            {
                _elasticClient.Update(DocumentPath<Product>.Id(changedEntity.Id),
                    u => u
                        .Index("store")
                        .Type("product")
                        .DocAsUpsert(true)
                        .Doc(changedEntity));
            }

            if (e.ChangeType == ChangeType.Insert)
            {
                _elasticClient.IndexDocument(changedEntity);
            }

            if (e.ChangeType == ChangeType.Delete)
            {
                _elasticClient.Delete<Product>(changedEntity.Id);
            }
        }
    }
}