var builder = DistributedApplication.CreateBuilder(args);


var redis = builder.AddRedis("cache");

var postgres = builder.AddPostgres("NpgsqlConnection");
var postgresdb = postgres.AddDatabase("events");

builder.AddProject<Projects.Mc2_CrudTest_Presentation_Server>("mc2-crudtest-presentation-server").WithReference(redis).WithReference(postgresdb); ;


builder.Build().Run();
