﻿using System.Net.Mime;
using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Core.Queries.GetSentNotifications;
using FamilyHubs.Notification.Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.Notification.Api.Endpoints;

public class MinimalNotifyEndPoints
{
    public void RegisterMinimalNotifyEndPoints(WebApplication app)
    {
        app.MapPost("api/notify", [Authorize] async ([FromBody] MessageDto request, CancellationToken cancellationToken, ISender _mediator) =>
        {
            CreateNotificationCommand command = new CreateNotificationCommand(request);
            var result = await _mediator.Send(command, cancellationToken);
            return result;

        }).WithMetadata(new SwaggerOperationAttribute("Notifications", "Send Notification") { Tags = new[] { "Notifications" } });
        
        app.MapGet("api/notify", [Authorize] async (ApiKeyType? apiKeyType, NotificationOrderBy ? orderBy, bool? isAscending, int? pageNumber, int? pageSize, CancellationToken cancellationToken, ISender _mediator) =>
        {
            GetNotificationsCommand request = new(apiKeyType, orderBy, isAscending, pageNumber, pageSize);
            var result = await _mediator.Send(request, cancellationToken);
            return result;

        })
            .WithMetadata(new SwaggerOperationAttribute("Get Notifications", "Get Paginated Notification List") { Tags = new[] { "Notifications" } })
            .Produces<PaginatedList<MessageDto>>(contentType: MediaTypeNames.Application.Json);

        app.MapGet("api/notify/{id}", [Authorize] async (long id, CancellationToken cancellationToken, ISender _mediator) =>
        {
            GetNotificationByIdCommand request = new(id);
            var result = await _mediator.Send(request, cancellationToken);
            return result;

        })
            .WithMetadata(new SwaggerOperationAttribute("Get Notification By Id", "Get Notification By Id") { Tags = new[] { "Notifications" } })
            .Produces<MessageDto>(contentType: MediaTypeNames.Application.Json);
    }
}
