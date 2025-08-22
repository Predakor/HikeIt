using Domain.Common;
using Domain.Common.Geography.ValueObjects;
using Domain.Common.Result;
using Domain.FileReferences.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Trips.GpxFile.Services;

internal class GpxService : IGpxService {
    readonly IGpxParser _parser;

    public GpxService(IGpxParser parser) {
        _parser = parser;
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(Stream stream) {
        try {
            return await _parser.ParseAsync(stream);
        }
        catch (Exception ex) {
            return Errors.Unknown(ex.Message);
        }
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(FileContent file) {
        try {
            using var ms = new MemoryStream(file.Content);
            return await _parser.ParseAsync(ms);
        }
        catch (Exception ex) {
            return Errors.Unknown(ex.Message);
        }
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(IFormFile file) {
        try {
            using var stream = file.OpenReadStream();

            return await _parser.ParseAsync(stream);
        }
        catch (Exception ex) {
            return Errors.Unknown(ex.Message);
        }
    }
}
