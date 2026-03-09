namespace Infrastructure.Config
{
    public static class Builder_CORS
    {
        public static IServiceCollection AddCORSService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFront",
                    policy => policy.WithOrigins(configuration["FrontApp:Url"]!)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });

            return services;
        }
    }
}
