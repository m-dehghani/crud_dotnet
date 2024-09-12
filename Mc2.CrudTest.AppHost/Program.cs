IDistributedApplicationBuilder builder = 
       DistributedApplication.CreateBuilder(args);

IResourceBuilder<RedisResource> redis = 
       builder.AddRedis("cache");

IResourceBuilder<PostgresServerResource> postgres = 
       builder.AddPostgres("NpgsqlConnection")
       .WithPgAdmin();

IResourceBuilder<PostgresDatabaseResource> postgresdb = 
       postgres.AddDatabase("customers");

IResourceBuilder<KafkaServerResource> messaging = 
       builder.AddKafka("messaging")
       .WithKafkaUI();

builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>
              ("mc2-crudtest-presentation-server")
       .WithReference(redis)
       .WithReference(postgresdb)
       .WithReference(messaging);

builder
       .Build()
       .Run();
