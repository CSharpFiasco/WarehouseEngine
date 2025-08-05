# Vendor API Implementation

This document describes the vendor endpoint implementation following the customer controller pattern.

## Endpoints Implemented

### 1. GET /api/v1/Vendor?id={id}
- Retrieves a single vendor by ID
- Returns 200 with VendorResponseDto on success
- Returns 404 if vendor not found
- Requires authorization

### 2. GET /api/v1/Vendor/list
- Retrieves all vendors
- Returns 200 with array of VendorResponseDto
- Requires authorization

### 3. GET /api/v1/Vendor/count
- Returns the total count of vendors
- Returns 200 with integer count
- Requires authorization

### 4. POST /api/v1/Vendor
- Creates a new vendor
- Accepts PostVendorDto in request body
- Returns 200 with created VendorResponseDto on success
- Returns 409 if vendor already exists
- Requires authorization

### 5. DELETE /api/v1/Vendor?id={id}
- Deletes a vendor by ID
- Returns 204 on success
- Requires authorization

### 6. PUT /api/v1/Vendor/{id}
- Updates an existing vendor
- Accepts Vendor entity in request body
- Returns 200 with updated VendorResponseDto on success
- Returns 404 if vendor not found
- Requires authorization

## DTOs

### VendorResponseDto
```csharp
public class VendorResponseDto
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
}
```

### PostVendorDto
```csharp
public class PostVendorDto
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    public string? Name { get; init; }
}
```

## Service Layer

### IVendorService Interface
- GetByIdAsync(Guid id)
- GetAllAsync()
- AddAsync(PostVendorDto vendor, string username)
- UpdateAsync(Guid id, Vendor entity)
- DeleteAsync(Guid id)
- GetCount()

### VendorService Implementation
- Follows the same pattern as CustomerService
- Uses Entity Framework for data access
- Implements proper error handling with OneOf pattern
- Uses IIdGenerator for ID generation

## Integration Tests

Comprehensive integration tests covering:
- Authentication scenarios (authorized/unauthorized)
- CRUD operations
- Error cases (not found)
- Success scenarios

## Notes

- Implementation follows the existing customer controller pattern exactly
- Uses the simple Vendor entity structure (only Id and Name fields)
- Vendor entity does not include audit fields like Customer (DateCreated, CreatedBy, etc.)
- Database schema supports only Id and Name fields for vendors
- All endpoints require JWT authentication