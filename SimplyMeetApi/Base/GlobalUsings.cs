global using System;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Data;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Data.Sqlite;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using ConcurrentCollections;
global using Dapper;
global using FluentMigrator;
global using FluentMigrator.Builders.Create.Table;
global using FluentMigrator.Runner;

global using SimplyMeetApi.Attributes;
global using SimplyMeetApi.Base;
global using SimplyMeetApi.Configuration;
global using SimplyMeetApi.Enums;
global using SimplyMeetApi.Extensions;
global using SimplyMeetApi.Hubs;
global using SimplyMeetApi.Middleware;
global using SimplyMeetApi.Models;
global using SimplyMeetApi.Services;
global using SimplyMeetShared.Base;
global using SimplyMeetShared.Constants;
global using SimplyMeetShared.Enums;
global using SimplyMeetShared.Extensions;
global using SimplyMeetShared.Models;
global using SimplyMeetShared.RequestModels;
global using SimplyMeetShared.ResponseModels;
global using SimplyMeetShared.SendModels;