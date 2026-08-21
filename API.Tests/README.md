# DEMS Backend Test Suite

Run the backend tests from the repository root with:

```powershell
dotnet test .\API.Tests\API.Tests.csproj
```

## Test Layout

- `Services/`: isolated service tests using mocked repositories.
- `Controllers/`: controller tests using mocked services.
- `Repositories/`: repository tests against a test database provider.
- `Integration/`: HTTP tests through the API pipeline, including authentication and permissions.

## Delivery Order

1. Department service and controller tests.
2. Authentication service and HTTP authentication tests.
3. Permission and authorization HTTP tests.
4. Employee, user, asset, document, designation, and remaining department services.
5. Core API integration coverage.

Unit tests stay fast and deterministic. Integration tests will use an isolated database and test authentication identities so they do not depend on a developer's local SQL Server data.