using NUnit.Framework;

namespace TodoListAPI.Tests.Unit.Domain
{
    [TestFixture]
    public class PaginatedResultTests
    {
        [Test]
        public void when_get_pagination_links_given_no_results_should_return_no_links()
        {
            var _ = new PaginatedResultTestContext();

            _.given_no_results();
            _.when_get_pagination_links();
            _.should_return_no_links();
        }

        [Test]
        public void when_get_pagination_links_given_one_page_of_results_should_return_no_links()
        {
            var _ = new PaginatedResultTestContext();

            _.given_one_page_of_results();
            _.when_get_pagination_links();
            _.should_return_no_links();
        }

        [Test]
        public void when_get_pagination_links_given_three_pages_of_results_and_requested_first_page_should_return_next_and_last_links()
        {
            var _ = new PaginatedResultTestContext();

            _.given_three_pages_of_results();
            _.given_first_page_requested();

            _.when_get_pagination_links();
            
            _.should_return_two_links();
            _.should_return_next_link();
            _.should_return_last_link();
        }

        [Test]
        public void when_get_pagination_links_given_three_pages_of_results_and_requested_second_page_should_return_first_prev_next_and_last_links()
        {
            var _ = new PaginatedResultTestContext();

            _.given_three_pages_of_results();
            _.given_second_page_requested();

            _.when_get_pagination_links();
            
            _.should_return_four_links();
            _.should_return_first_link();
            _.should_return_prev_link();
            _.should_return_next_link();
            _.should_return_last_link();
        }

        [Test]
        public void when_get_pagination_links_given_three_pages_of_results_and_requested_third_page_should_return_first_prev_links()
        {
            var _ = new PaginatedResultTestContext();

            _.given_three_pages_of_results();
            _.given_third_page_requested();

            _.when_get_pagination_links();
            
            _.should_return_two_links();
            _.should_return_first_link();
            _.should_return_prev_link();
        }
    }
}
