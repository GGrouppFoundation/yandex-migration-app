using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GGroupp.Yandex.Migration;

partial class TokenReaderMiddleware
{
    internal static EndpointApplication UseTokenReader(this EndpointApplication application)
    {
        _ = ((ISwaggerBuilder)application).Use(ConfigureSwagger);
        _ = application.Use(InvokeAsync);

        return application;
    }

    private static Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.GetTokenValue().Forward(context.GetBearerValue).Map(InnerSaveToken, InnerInvokeFailure).IsSuccess)
        {
            return next.Invoke(context);
        }

        return Task.CompletedTask;

        Unit InnerSaveToken(string token)
            =>
            context.RequestServices.GetRequiredService<ITokenStorage>().SaveToken(token);

        Unit InnerInvokeFailure(Failure<Unit> failure)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            if (string.IsNullOrEmpty(failure.FailureMessage) is false || failure.SourceException is not null)
            {
                context.GetLogger()?.LogError(failure.SourceException, "{tokenFailureMessage}", failure.FailureMessage);
            }

            return default;
        }
    }

    private static void ConfigureSwagger(OpenApiDocument openApiDocument)
    {
        if (openApiDocument is null)
        {
            return;
        }

        openApiDocument.Components ??= new();
        openApiDocument.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        openApiDocument.Components.SecuritySchemes[SecuritySchemeKey] = new()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header
        };

        var referenceScurityScheme = new OpenApiSecurityScheme
        {
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = SecuritySchemeKey
            }
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [referenceScurityScheme] = []
        };

        if (openApiDocument.Paths?.Count is not > 0)
        {
            return;
        }

        foreach (var path in openApiDocument.Paths.Values.SelectMany(GetOperations))
        {
            path.Security ??= [];
            path.Security.Add(securityRequirement);

            path.Responses ??= [];
            if (path.Responses.ContainsKey(UnauthorizedResponseCode))
            {
                continue;
            }

            path.Responses[UnauthorizedResponseCode] = new()
            {
                Description = UnauthorizedResponseDescription
            };
        }

        static IEnumerable<OpenApiOperation> GetOperations(OpenApiPathItem pathItem)
            =>
            pathItem.Operations?.Select(GetValue) ?? [];

        static TValue GetValue<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
            =>
            pair.Value;
    }
}