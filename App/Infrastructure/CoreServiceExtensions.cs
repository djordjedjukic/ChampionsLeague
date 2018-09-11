namespace App.Infrastructure
{
    using System.Reflection;

    //using App.Infrastructure.Authorization;
    //using App.Infrastructure.Command.Authorization;
    using App.Infrastructure.Command.Validation;
    //using App.Infrastructure.Query.Authorization;
    using App.Infrastructure.Query.Validation;
    using App.Infrastructure.Telemetry;

    using FluentValidation;

    using Microsoft.Extensions.DependencyInjection;

    using Paramore.Brighter.AspNetCore;
    using Paramore.Darker.AspNetCore;

    public static class CoreServiceExtensions
    {
        public static void ConfigureUclCore(this IServiceCollection services)
        {
            Assembly uclCore = Assembly.Load("App");

            services.AddDarker()
                .AddHandlersFromAssemblies(uclCore)
                .RegisterDecorator(typeof(QueryValidationDecorator<,>))
                //.RegisterDecorator(typeof(QueryAuthorizationDecorator<,>))
                .RegisterDecorator(typeof(QueryTelemetryDecorator<,>))
                .RegisterDecorator(typeof(CommandValidationDecorator<>))
                //.RegisterDecorator(typeof(CommandAuthorizationDecorator<>))
                .RegisterDecorator(typeof(CommandTelemetryDecorator<>))
                //.AddJsonQueryLogging()
                //.AddDefaultPolicies()
                ;

            services.AddBrighter()
                .AsyncHandlersFromAssemblies(uclCore)
                .HandlersFromAssemblies(uclCore);

            FluentValidation.AssemblyScanner.FindValidatorsInAssembly(uclCore)
                .ForEach(x => services.AddTransient(typeof(IValidator), x.ValidatorType));

            //services.RegisterQueryAuthorizers(haremCore);            
        }

        //public static void RegisterQueryAuthorizers(this IServiceCollection services, Assembly assembly)
        //{
        //    services.Scan(scan => scan
        //        .FromAssemblies(assembly)
        //        .AddClasses(classes => classes.AssignableTo<IAuthorizer>())
        //        .AsImplementedInterfaces()
        //        .WithTransientLifetime());

        //    services.Scan(scan => scan
        //        .FromAssemblies(assembly)
        //        .AddClasses(classes => classes.AssignableTo<IQueryAuthorizer>())
        //        .AsImplementedInterfaces()
        //        .WithTransientLifetime());

        //    services.Scan(scan => scan
        //        .FromAssemblies(assembly)
        //        .AddClasses(classes => classes.AssignableTo<ICommandAuthorizer>())
        //        .AsImplementedInterfaces()
        //        .WithTransientLifetime());
        //}
    }
}
