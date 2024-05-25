using Xunit;
using Restaurants.API.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entities;
using FluentAssertions;

namespace Restaurants.API.Tests.Middlewares
{
    public class ErrorHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<RequestDelegate> _requestDelegateMock;
        private readonly ErrorHandlingMiddleware _middleware;
        public ErrorHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            _httpContextMock = new Mock<HttpContext>();
            _requestDelegateMock = new Mock<RequestDelegate>();

            _middleware = new ErrorHandlingMiddleware(_loggerMock.Object);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            //arrange
            await _middleware.InvokeAsync(_httpContextMock.Object, _requestDelegateMock.Object);

            _requestDelegateMock.Verify(r => r.Invoke(_httpContextMock.Object), Times.Once());
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            //arrange
            var restaurantId = Guid.NewGuid();
            var context = new DefaultHttpContext();
            var notFoundException = new NotFoundException(nameof(Restaurant), restaurantId.ToString());

            await _middleware.InvokeAsync(context, _ => throw notFoundException);

            context.Response.StatusCode.Should().Be(404);
        }

        [Fact()]
        public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode404()
        {
            //arrange
            var restaurantId = Guid.NewGuid();
            var context = new DefaultHttpContext();
            var notFoundException = new ForbidException();

            await _middleware.InvokeAsync(context, _ => throw notFoundException);

            context.Response.StatusCode.Should().Be(403);
        }

        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
        {
            //arrange
            var restaurantId = Guid.NewGuid();
            var context = new DefaultHttpContext();
            var notFoundException = new Exception();

            await _middleware.InvokeAsync(context, _ => throw notFoundException);

            context.Response.StatusCode.Should().Be(500);
        }
    }
}