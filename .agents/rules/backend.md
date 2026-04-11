---
trigger: always_on
---

# ASP.NET Core — Coding Rules

## Stack
- .NET 10+, C# 12, Nullable enable, ImplicitUsings enable
- Clean Architecture: `Domain → Application → Infrastructure → Api`
- Minimal APIs ưu tiên; Controllers khi cần

## Project Structure
```
Domain/        # Entities, Value Objects, Generic Interfaces (zero NuGet deps)
Application/   # Use Cases, Specialized Interfaces, CQRS Handlers, DTOs, Validators
Infrastructure/# EF Core, external services, caching
Api/           # Endpoints, Middleware, DI wiring
```

## API
- REST đúng chuẩn: POST → 201 + Location, DELETE → 204, lỗi → 404/400
- Versioning qua URL: `/api/v1/`
- Dùng `TypedResults` với Minimal APIs
- Không đặt business logic trong Controller

## CQRS
- Mỗi use case = 1 handler (`IRequest<Result<T>>`)
- Dùng `ValidationBehavior` pipeline để validate trước handler

## Result Pattern
```csharp
// Không throw exception cho lỗi nghiệp vụ
Result<T>.Match(onSuccess, onFailure)

public static class ProductErrors
{
    public static readonly Error NotFound = new("Product.NotFound", "Not found.");
}
```

## Validation
- FluentValidation, đăng ký `AddValidatorsFromAssembly`
- Validate ở Application layer, không ở Controller

## EF Core
- Mỗi entity có `IEntityTypeConfiguration<T>` riêng
- Read-only query: `AsNoTracking()`
- Luôn truyền `CancellationToken`
- `SaveChangesAsync` ở handler, không ở repository

## DI
- `Scoped`: DbContext, Repository, UnitOfWork
- `Transient`: stateless service nhỏ
- `Singleton`: cache, HttpClientFactory
- Dùng primary constructor cho injection

## Error Handling
- Global handler với `UseExceptionHandler` → trả về `ProblemDetails` (RFC 7807)
- Không để stack trace lộ ra client

## Config
- Strongly typed options: `IOptions<T>` + `ValidateOnStart()`
- Không inject `IConfiguration` vào Application/Domain layer
- Không commit secret vào source control

## Logging
```csharp
// ✅ Structured logging
logger.LogInformation("Order {OrderId} placed", order.Id);
// ❌ String interpolation
logger.LogInformation($"Order {order.Id} placed");
```

## Security
- HTTPS + HSTS bật trong production
- Rate limiting trên auth endpoint
- Parameterized query / EF Core — không raw SQL với input người dùng
- JWT: expiry ngắn + refresh token

## Testing
- Unit: xUnit + NSubstitute + FluentAssertions
- Integration: `WebApplicationFactory<Program>`

## Conventions
- `sealed` cho concrete class mặc định
- Async method suffix `Async`
- Private field: `_camelCase`
- File-scoped namespace
- 1 class / 1 file, tối đa ~200 LOC

## Không làm
- `async void`, `.Result`, `.Wait()`
- `HttpClient` trực tiếp → dùng `IHttpClientFactory`
- `dynamic`, catch `Exception` không log/rethrow
- Mix sync/async code