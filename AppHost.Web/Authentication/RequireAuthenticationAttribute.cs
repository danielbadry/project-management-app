namespace AppHost.Web.Authentication;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RequireAuthenticationAttribute : Attribute
{
}
