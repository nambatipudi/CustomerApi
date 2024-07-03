[![CodeQL](https://github.com/nambatipudi/CustomerApi/actions/workflows/codeql.yml/badge.svg)](https://github.com/nambatipudi/CustomerApi/actions/workflows/codeql.yml)

[![Docker Image CI](https://github.com/nambatipudi/CustomerApi/actions/workflows/docker-image.yml/badge.svg)](https://github.com/nambatipudi/CustomerApi/actions/workflows/docker-image.yml)

[![.NET](https://github.com/nambatipudi/CustomerApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nambatipudi/CustomerApi/actions/workflows/dotnet.yml)

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

### Prerequisites

Ensure you have Helm installed and have access to a Kubernetes cluster.

### Navigate to the Helm Directory

```bash
cd ./customer-api-helm
```

### Install the Helm Chart

1. **Build and Push Docker Image**

   Build the Docker image for the Customer API application and push it to your container registry:

   ```bash
   # Navigate to the application source code directory
   cd /path/to/customer-api

   # Build the Docker image
   docker build -t <your-registry>/customer-api:latest .

   # Push the Docker image to your container registry
   docker push <your-registry>/customer-api:latest
   ```

2. **Update Helm Chart Values**

   Update the `values.yaml` file to set the correct image repository and tag:

   ```yaml
   image:
     repository: <your-registry>/customer-api
     tag: "latest"
     pullPolicy: IfNotPresent
   ```

3. **Deploy the Helm Chart**

   Install the Helm chart in your Kubernetes cluster:

   ```bash
   helm install customerapi ./customer-api-helm
   ```

   To upgrade an existing release, use:

   ```bash
   helm upgrade customerapi ./customer-api-helm
   ```

### Verify the Deployment

Check the status of your deployment:

```bash
kubectl get deployments
kubectl get pods
kubectl get services
```

### Access the Application

If you have Ingress configured, access the application via the configured host. Otherwise, you can port-forward to access the application:

```bash
kubectl port-forward svc/customerapi 8080:80
```

Open your browser and navigate to `http://localhost:8080`.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.

