using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.DTOs.Configurations;
using ECommerceAPI.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ECommerceAPI.Infrastructure.Services.Configurations
{
    public class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type assemblyType)
        {
            Assembly assembly = Assembly.GetAssembly(assemblyType);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<Menu> menus = new();
            if (controllers != null)
            {
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDeifinitionAttribute)));
                    if (actions != null)
                    {
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes != null)
                            {
                                Menu menu = null;
                                var authorizeDeifinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDeifinitionAttribute)) as AuthorizeDeifinitionAttribute;
                                if (!menus.Any(m => m.Name == authorizeDeifinitionAttribute.Menu))
                                {
                                    menu = new() { Name = authorizeDeifinitionAttribute.Menu };
                                    menus.Add(menu);
                                }
                                else
                                {
                                    menu = menus.FirstOrDefault(m => m.Name == authorizeDeifinitionAttribute.Menu);
                                }

                                Application.DTOs.Configurations.Action _action = new()
                                {

                                    ActionType = Enum.GetName(typeof(ActionType), authorizeDeifinitionAttribute.ActionType),
                                    Definition = authorizeDeifinitionAttribute.Definition,
                                };

                                var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;

                                if (httpAttribute != null)
                                {
                                    _action.HttpType = httpAttribute.HttpMethods.First();
                                }
                                else
                                {
                                    _action.HttpType = HttpMethods.Get;
                                }
                                _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ","")}";
                                menu.Actions.Add(_action);
                            }
                        }
                    }
                }

            }
            return menus;
        }
    }
}
