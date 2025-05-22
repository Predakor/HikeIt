using Api.Configuration.Cors.Interfaces;

namespace Api.Configuration.Cors.Models;

public abstract record CorsConfig(string Name) : ICorsConfig {
    public record Development(string Name) : CorsConfig(Name);
    public record Production(string Name, string[] Origins) : CorsConfig(Name);
}
