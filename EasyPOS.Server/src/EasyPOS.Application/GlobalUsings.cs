global using Microsoft.EntityFrameworkCore;
global using FluentValidation;
global using MediatR;
global using Microsoft.Extensions.Logging;
global using Dapper;

global using EasyPOS.Domain.Shared;
global using EasyPOS.Application.Common.DapperQueries;
global using EasyPOS.Application.Common.Security;
global using EasyPOS.Application.Common.Models;
global using EasyPOS.Application.Common.Abstractions.Messaging;
global using EasyPOS.Application.Common.Abstractions;
global using EasyPOS.Application.Common.Extensions;
global using System.Text.Json.Serialization;
global using EasyPOS.Application.Common.Abstractions.Caching;
