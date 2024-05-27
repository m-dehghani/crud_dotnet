var builder = DistributedApplication.CreateBuilder(args);


var redis = builder.AddRedis("RedisConnection");

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("customers");

builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>("mc2-crudtest-presentation-server").WithReference(redis).WithReference(postgresdb); ;


builder.Build().Run();
