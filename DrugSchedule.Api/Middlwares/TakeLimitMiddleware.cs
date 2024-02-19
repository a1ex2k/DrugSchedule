using System.Text;
using System.Text.Json;

namespace DrugSchedule.Api.Middlwares;

public class TakeLimitMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;

    public TakeLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == "POST")
        {
            // Read the request body
            string requestBody;
            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            // Parse the JSON request body into a dynamic object
            dynamic requestBodyObject = JsonSerializer.Deserialize<dynamic>(requestBody);

            // Check if the request body contains the nested object with the "Take" property
            if (requestBodyObject.ContainsKey("Data") && requestBodyObject["Data"].ContainsKey("Take"))
            {
                int takeValue = requestBodyObject["Data"]["Take"];
                // Limit the value to 200 if it exceeds
                if (takeValue > 200)
                {
                    takeValue = 200;
                    // Update the value in the request body
                    requestBodyObject["Data"]["Take"] = takeValue;

                    // Serialize the updated object back to JSON
                    string updatedRequestBody = JsonSerializer.Serialize(requestBodyObject);

                    // Replace the request body with the updated value
                    byte[] bytes = Encoding.UTF8.GetBytes(updatedRequestBody);
                    context.Request.Body = new MemoryStream(bytes);
                    context.Request.ContentLength = bytes.Length;
                }
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method != "POST")
        {
            await _next(context);
        }


    }
}