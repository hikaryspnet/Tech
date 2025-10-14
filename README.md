# Tech Project

# Description
This project will include Company and User Profile, mini-CRM, mini-ERP and auth module using JWT Bearer Token.

# Structure
- Tech.Core: Domain rich models, enums and some interface for other modules such as ITransactionalExecutor for UnitOfWork and ResultExtension.
- Tech.Application: Services and DTOs
- Tech.Infrastructure: Repositories and DbContext settings
- Tech.API: Controllers, Api

## Тестування
- Swagger: `/swagger
- Endpoint: `POST /api/auth/register-company` для реєстрації компанії.