using Microsoft.AspNetCore.Mvc;
using Tennis.Model.Models;
using Tennis.Model.Results;

namespace Tennis.Api.Controllers;
public class BaseController : ControllerBase
{
    protected async Task<IActionResult> ExecuteAsync<TSuccess, TFailure>(Task<Result<TSuccess, TFailure>> resultTask)
    {
        if (resultTask == null)
        {
            return BadRequest();
        }

        var result = await resultTask.ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return StatusCode(result.StatusCode, new { isSuccess = result.IsSuccess, statusCode = result.StatusCode, success = result.Success });
        }
        else
        {
            if (result.Failure is ResponseModel failureResponse)
            {
                return StatusCode(result.StatusCode, new { isSuccess = result.IsSuccess, statusCode = result.StatusCode, failure = result.Failure });
            }
            else
            {
                return StatusCode(result.StatusCode, new { isSuccess = result.IsSuccess, statusCode = result.StatusCode, failure = result.Failure });
            }
        }
    }
    protected async Task<IActionResult> ExecuteAsync(Task task)
    {
        await task.ConfigureAwait(false);
        return NoContent();
    }
}