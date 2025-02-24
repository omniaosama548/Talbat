namespace Talbat.APIs.Extensions
{
    public static class AddSwaggerApp
    {
        public static WebApplication AddSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
