using System;
using System.Linq;
using FluentAssertions;
using TodoListAPI.Domain;
using TodoListAPI.Dtos;

namespace TodoListAPI.Tests.Unit.Domain
{
    internal class PaginatedResultTestContext
    {
        public void given_no_results()
        {
            _data = new string[] { };
            _pageNumber = 1;
        }

        public void given_one_page_of_results()
        {
            _data = new[] { "foo", "bar" };
            _pageNumber = 1;
        }

        public void given_three_pages_of_results()
        {
            _data = new[] { "foo1", "bar1", "foo2", "bar2", "foo3", "bar3" };
            _pageNumber = 1;
        }

        public void given_first_page_requested()
        {
            _pageNumber = 1;
        }

        public void given_second_page_requested()
        {
            _pageNumber = 2;
        }

        public void given_third_page_requested()
        {
            _pageNumber = 3;
        }

        public void when_get_pagination_links()
        {
            var paginationInfo = new PaginationInfo(new PageRequest(_pageNumber, pageSize: 2), 
                                                    itemsTotalCount: _data.Length);

            var sut = new PaginatedResult<string>(_data, paginationInfo);
            
            _links = sut.GetPaginationLinks(_requestUri).ToArray();
        }

        public void should_return_no_links()
        {
            _links.Length.Should().Be(0);
        }

        public void should_return_two_links()
        {
            _links.Length.Should().Be(2);
        }

        public void should_return_four_links()
        {
            _links.Length.Should().Be(4);
        }

        public void should_return_first_link()
        {
            PaginationLinkInfo linkInfo = _links.Single(link => link.Relation == LinkRelationType.First);

            linkInfo.Url.ToString().Should().Be(_requestUri.ToString());
            linkInfo.PageSize.Should().Be(2);
            linkInfo.PageNumber.Should().Be(1);
        }

        public void should_return_prev_link()
        {
            PaginationLinkInfo linkInfo = _links.Single(link => link.Relation == LinkRelationType.Prev);

            linkInfo.Url.ToString().Should().Be(_requestUri.ToString());
            linkInfo.PageSize.Should().Be(2);
            linkInfo.PageNumber.Should().Be(_pageNumber - 1);
        }

        public void should_return_next_link()
        {
            PaginationLinkInfo linkInfo = _links.Single(link => link.Relation == LinkRelationType.Next);

            linkInfo.Url.ToString().Should().Be(_requestUri.ToString());
            linkInfo.PageSize.Should().Be(2);
            linkInfo.PageNumber.Should().Be(_pageNumber + 1);
        }

        public void should_return_last_link()
        {
            PaginationLinkInfo linkInfo = _links.Single(link => link.Relation == LinkRelationType.Last);

            linkInfo.Url.ToString().Should().Be(_requestUri.ToString());
            linkInfo.PageSize.Should().Be(2);
            linkInfo.PageNumber.Should().Be(3);
        }

        private readonly Uri _requestUri = new Uri("http://foo.bar?abc=123");

        private string[] _data;
        private PaginationLinkInfo[] _links;
        private int _pageNumber;
    }
}
