
IDistributedApplicationBuilder? builder = 
       DistributedApplication.CreateBuilder(args);

IResourceBuilder<RedisResource>? redis = 
       builder.AddRedis("cache");

IResourceBuilder<ParameterResource> username = builder.AddParameter("username", secret: true);
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", secret: true);

IResourceBuilder<PostgresServerResource>? postgres = 
       builder.AddPostgres("NpgsqlConnection").WithPgWeb();

IResourceBuilder<PostgresDatabaseResource>? postgresdb = 
       postgres.AddDatabase("customers");

var messaging = builder.AddKafka("messaging")
                       .WithKafkaUI();


builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>
              ("mc2-crudtest-presentation-server")
       .WithReference(redis)
       .WithReference(postgresdb).WithReference(messaging);
       .WithHttpEndpoint(port: 5066);
       .WithHttpsEndpoint(port: 7239);

builder.Build().Run();