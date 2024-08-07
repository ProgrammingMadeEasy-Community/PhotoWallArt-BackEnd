﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PhotoWallArt.Application.Common.ResponseObject;

public class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("statusCode")]
    public int? StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("Errors")]
    public string[]? Errors { get; set; }
}

public class ApiResponse
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("statusCode")]
    public int? StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("Errors")]
    public string[]? Errors { get; set; }
}

public class ResponseStatusCode
{
    public const int OK = 200;
    public const int Created = 201;
    public const int Accepted = 202;
    public const int NoContent = 204;

    public const int Found = 302;
    public const int SeeOther = 303;
    public const int NotModified = 304;

    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int Forbidden = 403;
    public const int NotFound = 404;
    public const int MethodNotAllowed = 405;
    public const int Conflict = 409;
    public const int Gone = 410;
    public const int PreconditionFailed = 412;
    public const int UnsupportedMediaType = 415;
    public const int UnprocessableEntity = 422;
    public const int Locked = 423;
    public const int TooManyRequests = 429;

    public const int InternalServerError = 500;
    public const int NotImplemented = 501;
    public const int ServiceUnavailable = 503;
    public const int GatewayTimeout = 504;
}
