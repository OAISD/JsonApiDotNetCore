

using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace JsonApiDotNetCore.Builders
{

  public interface ILinkBuilder
  {
    string GetSelfRelationLink(string basePath, string parent, string parentId, string child);
    string GetRelatedRelationLink(string basePath, string parent, string parentId, string child);
    string GetPageLink(IJsonApiContext jsonApiContext, int pageOffset, int pageSize);
    string GetBasePath(HttpContext context, JsonApiOptions jsonApiOptions, string entityName);
  }
}