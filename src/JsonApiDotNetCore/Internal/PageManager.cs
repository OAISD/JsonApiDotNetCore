using System;
using JsonApiDotNetCore.Builders;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;

namespace JsonApiDotNetCore.Internal
{
  public class PageManager
  {
    public int? TotalRecords { get; set; }
    public int PageSize { get; set; }
    public int DefaultPageSize { get; set; }
    public int CurrentPage { get; set; }
    public bool IsPaginated => PageSize > 0;
    public int TotalPages => (TotalRecords == null) ? -1 : (int)Math.Ceiling(decimal.Divide(TotalRecords.Value, PageSize));

    public RootLinks GetPageLinks(IJsonApiContext jsonApiContext)
    {
      var linkBuilder = jsonApiContext.LinkBuilder;
      if (ShouldIncludeLinksObject())
        return null;

      var rootLinks = new RootLinks();

      if (CurrentPage > 1)
        rootLinks.First = linkBuilder.GetPageLink(jsonApiContext, 1, PageSize);

      if (CurrentPage > 1)
        rootLinks.Prev = linkBuilder.GetPageLink(jsonApiContext, CurrentPage - 1, PageSize);

      if (CurrentPage < TotalPages)
        rootLinks.Next = linkBuilder.GetPageLink(jsonApiContext, CurrentPage + 1, PageSize);

      if (TotalPages > 0)
        rootLinks.Last = linkBuilder.GetPageLink(jsonApiContext, TotalPages, PageSize);

      return rootLinks;
    }

    private bool ShouldIncludeLinksObject() => (!IsPaginated || ((CurrentPage == 1 || CurrentPage == 0) && TotalPages <= 0));
  }
}
