var builder = DistributedApplication.CreateBuilder(args);


IResourceBuilder<ParameterResource> username = builder.AddParameter("username", secret: true);
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", secret: true);

var redis = builder.AddRedis("cache");

var postgres = builder.AddPostgres("NpgsqlConnection").WithPgAdmin();
var postgresdb = postgres.AddDatabase("customers");

IResourceBuilder<KafkaServerResource> messaging = builder.AddKafka("messaging")
       .WithKafkaUI();

builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>("mc2-crudtest-presentation-server")
       .WithReference(redis)
       .WithReference(postgresdb)
       .WithReference(messaging)
       .WithHttpEndpoint(port: 5066)
       .WithHttpsEndpoint(port: 7239); 


builder.Build().Run();
