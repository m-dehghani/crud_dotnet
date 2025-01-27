## Description 
A simple project, leveraging Event sourcing and cqrs patterns. Backend has written with .net core and the frontend is Blazor.

The project has a complete set of tests(Bdd, unit tests, ui test. It's developed with microsoft Aspire for easy containerization and deployment into the clouds.

Logics, entities and services are based on DDD(domain driven design) and are event oriented.

## projects
The main projects are in presentation folder (both the backend and frontend)
## Practices and patterns:

- [TDD](https://docs.microsoft.com/en-us/visualstudio/test/quick-start-test-driven-development-with-test-explorer?view=vs-2022)
- [BDD](https://en.wikipedia.org/wiki/Behavior-driven_development)
- [DDD](https://en.wikipedia.org/wiki/Domain-driven_design)
- [Clean architecture](https://github.com/jasontaylordev/CleanArchitecture)
- [CQRS](https://en.wikipedia.org/wiki/Command%E2%80%93query_separation#Command_query_responsibility_separation) pattern ([Event sourcing](https://en.wikipedia.org/wiki/Domain-driven_design#Event_sourcing)).
- Clean git commits

### Validations

- During Create; validate the phone number to be a valid *mobile* number (https://github.com/google/libphonenumber) to validate number at the backend).

- A Valid email and a valid bank account number must be checked before submitting the form.

- Customers must be unique in the database: By `Firstname`, `Lastname`, and `DateOfBirth`.

- Email must be unique in the database.

### Storage

- phone numbers are stored in a database with minimized space storage (choose `varchar`/`string`, or `ulong` whichever store less space).
docker compose and docker file are also created and can be used.
