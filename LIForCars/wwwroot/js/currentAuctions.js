function navigateToPage(page) {
    window.location.href = '/CurrentAuctions?CurrentPage=' + page + '&PageSize=@Model.PageSize';
}

function navigateToAuction(auctionId) {
    window.location.href = '/Auction/?id=' + auctionId;
}