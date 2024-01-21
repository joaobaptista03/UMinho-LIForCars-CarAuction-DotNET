function navigateToPage(page) {
    window.location.href = '/CurrentAuctions?CurrentPage=' + page + '&PageSize=@Model.PageSize';
}