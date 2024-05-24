using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using ApiCrudPersonajesAWS.Repositories;
using ApiCrudPersonajesAWS.Models;
using System.Text.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ApiCrudPersonajesAWS;

public class Functions
{
    private RepositoryPersonajes repo;
    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public Functions(RepositoryPersonajes repo)
    {
        this.repo = repo;
    }


    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <remarks>
    /// This uses the <see href="https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.Annotations/README.md">Lambda Annotations</see> 
    /// programming model to bridge the gap between the Lambda programming model and a more idiomatic .NET model.
    /// 
    /// This automatically handles reading parameters from an APIGatewayProxyRequest
    /// as well as syncing the function definitions to serverless.template each time you build.
    /// 
    /// If you do not wish to use this model and need to manipulate the API Gateway 
    /// objects directly, see the accompanying Readme.md for instructions.
    /// </remarks>
    /// <param name="context">Information about the invocation, function, and execution environment</param>
    /// <returns>The response as an implicit <see cref="APIGatewayProxyResponse"/></returns>
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/")]
    public async Task<IHttpResult> Get(ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'Get' Request");
        List<Personaje> personajes= await this.repo.GetPersonajesAsync();
        return HttpResults.Ok(personajes);
    }    
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/find/{id}")]
    public async Task<IHttpResult> Find(int id, ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'Get' Request");
        Personaje personaje= await this.repo.FindPersonajeAsync(id);
        return HttpResults.Ok(personaje);
    }    
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/post")]
    public async Task<IHttpResult> Post([FromBody] Personaje personaje, ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'POST' Request");
        Personaje personajeNew = await this.repo.CreatePersonajeAsync(personaje.Nombre, personaje.Imagen);
        return HttpResults.Ok(personajeNew);
    }    
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Put, "/put")]
    public async Task<IHttpResult> Put([FromBody] Personaje personaje, ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'Put' Request");
        await this.repo.UpdatePersonajeAsync(personaje.IdPersonaje, personaje.Nombre, personaje.Imagen);
        return HttpResults.Ok("Todo OK Jose luis");
    }    
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Delete, "/delete/{id}")]
    public async Task<IHttpResult> Delete(int id, ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'Put' Request");
        await this.repo.DeletePersonajeAsync(id);
        return HttpResults.Ok("Todo OK Jose luis");
    }
}
