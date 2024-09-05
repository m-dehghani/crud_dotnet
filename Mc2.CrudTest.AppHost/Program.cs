IDistributedApplicationBuilder? builder = DistributedApplication.CreateBuilder(args);


IResourceBuilder<RedisResource>? redis = builder.AddRedis("cache");

IResourceBuilder<PostgresServerResource>? postgres = builder.AddPostgres("NpgsqlConnection");
IResourceBuilder<PostgresDatabaseResource>? postgresdb = postgres.AddDatabase("customers");

builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>("mc2-crudtest-presentation-server").WithReference(redis).WithReference(postgresdb).WithHttpEndpoint(port: 5066)
       .WithHttpsEndpoint(port: 7239); ; ;


builder.Build().Run();
