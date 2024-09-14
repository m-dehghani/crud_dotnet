IDistributedApplicationBuilder builder = 
       DistributedApplication.CreateBuilder(args);

IResourceBuilder<RedisResource> redis = 
       builder.AddRedis("cache");

IResourceBuilder<ParameterResource> username = 
       builder.AddParameter("username", secret: true);

IResourceBuilder<ParameterResource> password = 
       builder.AddParameter("password", secret: true);

IResourceBuilder<PostgresServerResource> postgres = 
       builder.AddPostgres("postgres", username, password)
       .WithPgAdmin();

IResourceBuilder<PostgresDatabaseResource> postgresdb = 
       postgres.AddDatabase("customers");


IResourceBuilder<KafkaServerResource> messaging = 
       builder.AddKafka("messaging")
       .WithKafkaUI();

builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>
              ("server")
       //.WithReference(redis)
       .WithReference(postgresdb)
       .WithReference(messaging);

builder
       .Build()
       .Run();
