using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Domain.Catalog;
using PhotoWallArt.Domain.Common.Events;

namespace PhotoWallArt.Application.Catalog.Products;
public class CreateProductRequest : IRequest<ApiResponse<Guid>>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public Guid BrandId { get; set; }
    public FileUploadRequest? Image { get; set; }
}

public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, ApiResponse<Guid>>
{
    private readonly IRepository<Product> _repository;
    private readonly IFileStorageService _file;

    public CreateProductRequestHandler(IRepository<Product> repository, IFileStorageService file) =>
        (_repository, _file) = (repository, file);

    public async Task<ApiResponse<Guid>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var response = new ApiResponse<Guid>();

        try
        {

            string productImagePath = await _file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken);

            var product = new Product(request.Name, request.Description, request.Rate, request.BrandId, productImagePath);

            // Add Domain Events to be raised after the commit
            product.DomainEvents.Add(EntityCreatedEvent.WithEntity(product));
            await _repository.AddAsync(product, cancellationToken);

            response.Message = " Product created";
            response.Status = ResponseStatus.True;
            response.StatusCode = ResponseStatusCode.Created;
            response.Data = product.Id;


            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Status = ResponseStatus.Error;
            response.StatusCode = ResponseStatusCode.InternalServerError;
            response.Data = Guid.Empty;
            return response;
        }
    }
}