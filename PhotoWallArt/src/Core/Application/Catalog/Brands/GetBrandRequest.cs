using PhotoWallArt.Application.Common.ResponseObject;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhotoWallArt.Application.Catalog.Brands;

public class GetBrandRequest : IRequest<ApiResponse<BrandDto>>
{
    public Guid Id { get; set; }

    public GetBrandRequest(Guid id) => Id = id;
}

public class BrandByIdSpec : Specification<Brand, BrandDto>, ISingleResultSpecification
{
    public BrandByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetBrandRequestHandler : IRequestHandler<GetBrandRequest, ApiResponse<BrandDto>>
{
    private readonly IRepository<Brand> _repository;
    private readonly IStringLocalizer _t;

    public GetBrandRequestHandler(IRepository<Brand> repository, IStringLocalizer<GetBrandRequestHandler> localizer) => (_repository, _t) = (repository, localizer);

    public async Task<ApiResponse<BrandDto>> Handle(GetBrandRequest request, CancellationToken cancellationToken)
    {
        var response = new ApiResponse<BrandDto>();
        try
        {
            var data = await _repository.FirstOrDefaultAsync(
          (ISpecification<Brand, BrandDto>)new BrandByIdSpec(request.Id), cancellationToken);

            if(data != null)
            {
                response.Message = "Brand Found";
                response.Data = data;
                response.Status = true;
                response.StatusCode = ResponseStatusCode.Found;
            }

            response.Message = "Brand not Found";
            response.Data = null;
            response.Status = false;
            response.StatusCode = ResponseStatusCode.NotFound;

            return response;
        }
        catch(Exception ex)
        {
            response.Message = ex.Message;
            response.Data = null;
            response.Status = false;
            response.StatusCode = ResponseStatusCode.InternalServerError;
            return response;
        }
    }

}