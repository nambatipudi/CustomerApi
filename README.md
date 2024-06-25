<style>
pre,
code {
  font-family: Menlo, Monaco, "Courier New", monospace;
}

pre {
  padding: .5rem;
  line-height: 1.25;
  overflow-x: scroll;
}

@media print {
  *,
  *:before,
  *:after {
    background: transparent !important;
    color: #000 !important;
    box-shadow: none !important;
    text-shadow: none !important;
  }

  a,
  a:visited {
    text-decoration: underline;
  }

  a[href]:after {
    content: " (" attr(href) ")";
  }

  abbr[title]:after {
    content: " (" attr(title) ")";
  }

  a[href^="#"]:after,
  a[href^="javascript:"]:after {
    content: "";
  }

  pre,
  blockquote {
    border: 1px solid #999;
    page-break-inside: avoid;
  }

  thead {
    display: table-header-group;
  }

  tr,
  img {
    page-break-inside: avoid;
  }

  img {
    max-width: 100% !important;
  }

  p,
  h2,
  h3 {
    orphans: 3;
    widows: 3;
  }

  h2,
  h3 {
    page-break-after: avoid;
  }
}

a,
a:visited {
  color: #01ff70;
}

a:hover,
a:focus,
a:active {
  color: #2ecc40;
}

.retro-no-decoration {
  text-decoration: none;
}

html {
  font-size: 12px;
}

@media screen and (min-width: 32rem) and (max-width: 48rem) {
  html {
    font-size: 15px;
  }
}

@media screen and (min-width: 48rem) {
  html {
    font-size: 16px;
  }
}

body {
  line-height: 1.85;
}

p,
.retro-p {
  font-size: 1rem;
  margin-bottom: 1.3rem;
}

h1,
.retro-h1,
h2,
.retro-h2,
h3,
.retro-h3,
h4,
.retro-h4 {
  margin: 1.414rem 0 .5rem;
  font-weight: inherit;
  line-height: 1.42;
}

h1,
.retro-h1 {
  margin-top: 0;
  font-size: 3.998rem;
}

h2,
.retro-h2 {
  font-size: 2.827rem;
}

h3,
.retro-h3 {
  font-size: 1.999rem;
}

h4,
.retro-h4 {
  font-size: 1.414rem;
}

h5,
.retro-h5 {
  font-size: 1.121rem;
}

h6,
.retro-h6 {
  font-size: .88rem;
}

small,
.retro-small {
  font-size: .707em;
}

/* https://github.com/mrmrs/fluidity */

img,
canvas,
iframe,
video,
svg,
select,
textarea {
  max-width: 100%;
}

html,
body {
  background-color: #222;
  min-height: 100%;
}

html {
  font-size: 18px;
}

body {
  color: #fafafa;
  font-family: "Courier New";
  line-height: 1.45;
  margin: 6rem auto 1rem;
  max-width: 48rem;
  padding: .25rem;
}

pre {
  background-color: #333;
}

blockquote {
  border-left: 3px solid #01ff70;
  padding-left: 1rem;
}
</style>
[![CodeQL](https://github.com/nambatipudi/CustomerApi/actions/workflows/codeql.yml/badge.svg)](https://github.com/nambatipudi/CustomerApi/actions/workflows/codeql.yml)
# CustomerApi

CustomerApi is a RESTful web service for managing customer data. This project includes a .NET 8.0 Web API with SQLite for data storage, Swagger for API documentation, and unit tests for ensuring code quality.

## Prerequisites

- .NET 8.0 SDK
- Docker (for containerization)
- Helm (for Kubernetes deployment)
- SQLite (for local development)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/your-username/CustomerApi.git
cd CustomerApi
```

### Setup and Run the Application

1. **Restore Dependencies**

    ```bash
    dotnet restore
    ```

2. **Apply Migrations**

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

3. **Build and Run the Application**

    ```bash
    dotnet build
    dotnet run
    ```

4. **Access the API Documentation**

    Open your browser and navigate to `http://localhost:5000/swagger` to view and interact with the API documentation.

## API Endpoints

### Get All Customers

- **URL**: `/api/customers`
- **Method**: `GET`
- **Description**: Retrieves a list of all customers.
- **Response**: JSON array of customer objects.

### Get Customer by ID

- **URL**: `/api/customers/{id}`
- **Method**: `GET`
- **Description**: Retrieves a specific customer by their ID.
- **Response**: JSON object of the customer.

### Get Customer by Email

- **URL**: `/api/customers/email/{email}`
- **Method**: `GET`
- **Description**: Retrieves a specific customer by their email address.
- **Response**: JSON object of the customer.

### Create a Customer

- **URL**: `/api/customers`
- **Method**: `POST`
- **Description**: Creates a new customer.
- **Request Body**: JSON object of the customer details.
- **Response**: JSON object of the created customer.

### Update a Customer

- **URL**: `/api/customers/{id}`
- **Method**: `PUT`
- **Description**: Updates an existing customer.
- **Request Body**: JSON object of the updated customer details.
- **Response**: No content.

### Delete a Customer

- **URL**: `/api/customers/{id}`
- **Method**: `DELETE`
- **Description**: Deletes a specific customer by their ID.
- **Response**: No content.

## Model Validation

The `Customer` model includes the following validations:

- `FirstName` and `LastName` are required and have a maximum length of 50 characters.
- `MiddleName` has a maximum length of 50 characters.
- `Email` is required and must be a valid email address.
- `PhoneNumber` must be a valid phone number format.
- `Prefix` and `Suffix` must be one of the predefined values.

## Running Tests

To run the unit tests and generate a coverage report:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

Coverage reports will be available in the `TestResults` directory.

## Building and Publishing Docker Image

1. **Build the Docker Image**

    ```bash
    docker build -t your-local-registry/customer-service:latest .
    ```

2. **Push the Docker Image to Your Local Registry**

    ```bash
    docker push your-local-registry/customer-service:latest
    ```

## Deploying with Helm

1. **Navigate to the `helm` Directory**

    ```bash
    cd helm
    ```

2. **Install the Helm Chart**

    ```bash
    helm install customer-service customer-service
    ```

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.